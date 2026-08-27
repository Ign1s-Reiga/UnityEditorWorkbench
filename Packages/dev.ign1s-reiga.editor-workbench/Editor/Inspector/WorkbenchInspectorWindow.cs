using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ign1s.EditorWorkbench;

internal sealed class WorkbenchInspectorWindow : EditorWindow
{
    [SerializeField] private List<InspectorTabState> tabs = new();
    [SerializeField] private int activeTabIndex = -1;

    private readonly List<UnityEditor.Editor> targetEditors = new();
    private Object editorTarget;
    private bool editorsDirty = true;

    [MenuItem(WorkbenchConstants.RootMenu + "Inspector", false, 10)]
    internal static void OpenWindow()
    {
        GetWindow<WorkbenchInspectorWindow>("Workbench Inspector");
    }

    private void OnEnable()
    {
        Selection.selectionChanged -= FollowSelection;
        Selection.selectionChanged += FollowSelection;
        AssemblyReloadEvents.beforeAssemblyReload -= DestroyTargetEditors;
        AssemblyReloadEvents.beforeAssemblyReload += DestroyTargetEditors;
        EditorApplication.hierarchyChanged -= MarkEditorsDirty;
        EditorApplication.hierarchyChanged += MarkEditorsDirty;
        Undo.undoRedoPerformed -= MarkEditorsDirty;
        Undo.undoRedoPerformed += MarkEditorsDirty;

        EnsureTabExists();
        RefreshTargetEditors();
    }

    private void OnDisable()
    {
        Selection.selectionChanged -= FollowSelection;
        AssemblyReloadEvents.beforeAssemblyReload -= DestroyTargetEditors;
        EditorApplication.hierarchyChanged -= MarkEditorsDirty;
        Undo.undoRedoPerformed -= MarkEditorsDirty;
        DestroyTargetEditors();
    }

    private void OnGUI()
    {
        EnsureTabExists();
        DrawTabStrip();
        DrawNavigationToolbar();
        DrawActiveInspector();
    }

    private void DrawTabStrip()
    {
        using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
        {
            for (int index = 0; index < tabs.Count; index++)
            {
                InspectorTabState tab = tabs[index];
                bool selected = index == activeTabIndex;
                GUIStyle style = selected ? EditorStyles.toolbarButton : EditorStyles.miniButton;

                if (GUILayout.Toggle(selected, tab.GetTitle(), style, GUILayout.MinWidth(64f)) && !selected)
                {
                    activeTabIndex = index;
                    RefreshTargetEditors();
                    GUI.FocusControl(null);
                }
            }

            GUILayout.FlexibleSpace();
            if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Plus"), EditorStyles.toolbarButton, GUILayout.Width(28f)))
            {
                AddTab(Selection.activeObject);
            }

            using (new EditorGUI.DisabledScope(tabs.Count <= 1))
            {
                if (GUILayout.Button(EditorGUIUtility.IconContent("Toolbar Minus"), EditorStyles.toolbarButton, GUILayout.Width(28f)))
                {
                    CloseActiveTab();
                }
            }
        }
    }

    private void DrawNavigationToolbar()
    {
        InspectorTabState tab = ActiveTab;
        using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
        {
            using (new EditorGUI.DisabledScope(!tab.History.CanMoveBack))
            {
                if (GUILayout.Button(EditorGUIUtility.IconContent("back"), EditorStyles.toolbarButton, GUILayout.Width(28f)))
                {
                    SetActiveTarget(tab.History.MoveBack(), false);
                }
            }

            using (new EditorGUI.DisabledScope(!tab.History.CanMoveForward))
            {
                if (GUILayout.Button(EditorGUIUtility.IconContent("forward"), EditorStyles.toolbarButton, GUILayout.Width(28f)))
                {
                    SetActiveTarget(tab.History.MoveForward(), false);
                }
            }

            Object currentTarget = tab.ResolveTarget();
            Object requestedTarget = EditorGUILayout.ObjectField(currentTarget, typeof(Object), true);
            if (requestedTarget != currentTarget)
            {
                SetActiveTarget(requestedTarget, true);
                GUIUtility.ExitGUI();
            }

            GUIContent lockContent = EditorGUIUtility.IconContent(tab.Locked ? "LockIcon-On" : "LockIcon");
            lockContent.tooltip = tab.Locked ? "Unlock tab" : "Lock tab";
            if (GUILayout.Button(lockContent, EditorStyles.toolbarButton, GUILayout.Width(28f)))
            {
                tab.Locked = !tab.Locked;
            }
        }
    }

    private void DrawActiveInspector()
    {
        InspectorTabState tab = ActiveTab;
        Object target = tab.ResolveTarget();

        if (target == null)
        {
            EditorGUILayout.HelpBox("Select an object, or add a tab while an object is selected.", MessageType.Info);
            return;
        }

        if (editorsDirty || editorTarget != target || targetEditors.Count == 0)
        {
            RefreshTargetEditors();
        }

        if (targetEditors.Count == 0)
        {
            EditorGUILayout.HelpBox("Unity could not create an editor for this target.", MessageType.Warning);
            return;
        }

        tab.ScrollPosition = EditorGUILayout.BeginScrollView(tab.ScrollPosition);
        EditorGUI.BeginChangeCheck();

        for (int index = 0; index < targetEditors.Count; index++)
        {
            UnityEditor.Editor targetEditor = targetEditors[index];
            if (targetEditor == null || targetEditor.target == null)
            {
                editorsDirty = true;
                continue;
            }

            targetEditor.DrawHeader();
            targetEditor.OnInspectorGUI();

            if (index < targetEditors.Count - 1)
            {
                EditorGUILayout.Space(2f);
            }
        }

        if (EditorGUI.EndChangeCheck())
        {
            Repaint();
        }

        EditorGUILayout.EndScrollView();
    }

    private void FollowSelection()
    {
        EnsureTabExists();
        if (ActiveTab.Locked)
        {
            return;
        }

        SetActiveTarget(Selection.activeObject, true);
    }

    private void SetActiveTarget(Object target, bool addToHistory)
    {
        ActiveTab.SetTarget(target, addToHistory);
        RefreshTargetEditors();
        Repaint();
    }

    private void AddTab(Object target)
    {
        InspectorTabState tab = new();
        tab.SetTarget(target, true);
        tabs.Add(tab);
        activeTabIndex = tabs.Count - 1;
        RefreshTargetEditors();
    }

    private void CloseActiveTab()
    {
        if (tabs.Count <= 1)
        {
            return;
        }

        tabs.RemoveAt(activeTabIndex);
        activeTabIndex = Mathf.Clamp(activeTabIndex, 0, tabs.Count - 1);
        RefreshTargetEditors();
    }

    private void EnsureTabExists()
    {
        if (tabs.Count == 0)
        {
            AddTab(Selection.activeObject);
        }

        activeTabIndex = Mathf.Clamp(activeTabIndex, 0, tabs.Count - 1);
    }

    private InspectorTabState ActiveTab => tabs[activeTabIndex];

    private void RefreshTargetEditors()
    {
        DestroyTargetEditors();
        editorsDirty = false;

        if (tabs.Count == 0 || activeTabIndex < 0 || activeTabIndex >= tabs.Count)
        {
            return;
        }

        editorTarget = ActiveTab.ResolveTarget();
        if (editorTarget == null)
        {
            return;
        }

        if (editorTarget is GameObject gameObject)
        {
            AddTargetEditor(gameObject);

            Component[] components = gameObject.GetComponents<Component>();
            foreach (Component component in components)
            {
                if (component != null)
                {
                    AddTargetEditor(component);
                }
            }
        }
        else
        {
            AddTargetEditor(editorTarget);
        }
    }

    private void AddTargetEditor(Object target)
    {
        UnityEditor.Editor targetEditor = UnityEditor.Editor.CreateEditor(target);
        if (targetEditor != null)
        {
            targetEditors.Add(targetEditor);
        }
    }

    private void DestroyTargetEditors()
    {
        foreach (UnityEditor.Editor targetEditor in targetEditors)
        {
            if (targetEditor != null)
            {
                DestroyImmediate(targetEditor);
            }
        }

        targetEditors.Clear();
        editorTarget = null;
    }

    private void MarkEditorsDirty()
    {
        editorsDirty = true;
        Repaint();
    }
}
