using UnityEditor;
using UnityEngine;

namespace Ign1s.EditorWorkbench;

internal sealed class FolderAppearanceWindow : EditorWindow
{
    [SerializeField] private string folderGuid = string.Empty;
    [SerializeField] private bool hasCustomTint;
    [SerializeField] private Color tint = default;
    [SerializeField] private string iconName = string.Empty;

    internal static void Open(string guid)
    {
        FolderAppearanceWindow window = GetWindow<FolderAppearanceWindow>(true, "Folder Appearance", true);
        window.minSize = new Vector2(360f, 190f);
        window.Load(guid);
        window.Show();
    }

    private void OnGUI()
    {
        string path = AssetDatabase.GUIDToAssetPath(folderGuid);
        if (string.IsNullOrEmpty(path) || !AssetDatabase.IsValidFolder(path))
        {
            EditorGUILayout.HelpBox("Select a valid folder from the Project window.", MessageType.Info);
            if (GUILayout.Button("Use Selected Folder") && AssetGuidUtility.TryGetSelectedFolder(out string selectedGuid, out _))
            {
                Load(selectedGuid);
            }
            return;
        }

        EditorGUILayout.LabelField("Folder", path);
        EditorGUILayout.Space();

        hasCustomTint = EditorGUILayout.Toggle("Use Tint", hasCustomTint);
        using (new EditorGUI.DisabledScope(!hasCustomTint))
        {
            tint = EditorGUILayout.ColorField("Tint", tint);
        }

        iconName = EditorGUILayout.TextField(new GUIContent("Built-in Icon", "For example: Folder Icon, Favorite, Prefab Icon"), iconName);
        DrawIconPreview();

        GUILayout.FlexibleSpace();
        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Clear"))
            {
                Clear();
            }

            if (GUILayout.Button("Apply"))
            {
                Apply();
            }
        }
    }

    private void DrawIconPreview()
    {
        if (string.IsNullOrWhiteSpace(iconName))
        {
            return;
        }

        GUIContent content = EditorGUIUtility.IconContent(iconName);
        using (new EditorGUILayout.HorizontalScope())
        {
            EditorGUILayout.PrefixLabel("Preview");
            if (content.image == null)
            {
                EditorGUILayout.LabelField("Icon not found");
            }
            else
            {
                GUILayout.Label(content.image, GUILayout.Width(20f), GUILayout.Height(20f));
            }
        }
    }

    private void Load(string guid)
    {
        folderGuid = guid;
        if (ProjectWorkbenchSettings.instance.FolderAppearances.TryGet(guid, out FolderAppearance appearance))
        {
            hasCustomTint = appearance.HasCustomTint;
            tint = appearance.Tint;
            iconName = appearance.IconName;
        }
        else
        {
            hasCustomTint = true;
            tint = WorkbenchConstants.DefaultFolderTint;
            iconName = "Folder Icon";
        }

        Repaint();
    }

    private void Apply()
    {
        FolderAppearance appearance = ProjectWorkbenchSettings.instance.FolderAppearances.GetOrCreate(folderGuid);
        appearance.HasCustomTint = hasCustomTint;
        appearance.Tint = tint;
        appearance.IconName = iconName.Trim();
        ProjectWorkbenchSettings.instance.SaveSettings();
        EditorApplication.RepaintProjectWindow();
    }

    private void Clear()
    {
        ProjectWorkbenchSettings.instance.FolderAppearances.Remove(folderGuid);
        ProjectWorkbenchSettings.instance.SaveSettings();
        EditorApplication.RepaintProjectWindow();
        Load(folderGuid);
    }
}
