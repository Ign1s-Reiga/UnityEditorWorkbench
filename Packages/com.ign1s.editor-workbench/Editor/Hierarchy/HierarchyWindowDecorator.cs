using UnityEditor;
using UnityEngine;

namespace Ign1s.EditorWorkbench;

[InitializeOnLoad]
internal static class HierarchyWindowDecorator
{
    static HierarchyWindowDecorator()
    {
        EditorApplication.hierarchyWindowItemOnGUI -= DrawHierarchyItem;
        EditorApplication.hierarchyWindowItemOnGUI += DrawHierarchyItem;
    }

    private static void DrawHierarchyItem(int instanceId, Rect selectionRect)
    {
        if (!ProjectWorkbenchSettings.instance.HierarchyDecorationsEnabled)
        {
            return;
        }

        GameObject gameObject = EditorUtility.InstanceIDToObject(instanceId) as GameObject;
        if (gameObject == null)
        {
            return;
        }

        Rect activeRect = new(selectionRect.xMax - 18f, selectionRect.y + 2f, 14f, selectionRect.height - 4f);

        if (Event.current.type == EventType.Repaint)
        {
            Color indicatorColor = gameObject.activeSelf
                ? new Color(0.35f, 0.85f, 0.42f, 0.9f)
                : new Color(0.55f, 0.55f, 0.55f, 0.55f);
            EditorGUI.DrawRect(new Rect(activeRect.center.x - 3f, activeRect.center.y - 3f, 6f, 6f), indicatorColor);

            if (PrefabUtility.IsPartOfPrefabInstance(gameObject))
            {
                GUIContent prefabIcon = EditorGUIUtility.IconContent("Prefab Icon");
                if (prefabIcon.image != null)
                {
                    Rect prefabRect = new(selectionRect.xMax - 36f, selectionRect.y + 1f, 16f, 16f);
                    GUI.DrawTexture(prefabRect, prefabIcon.image, ScaleMode.ScaleToFit, true);
                }
            }
        }

        if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && activeRect.Contains(Event.current.mousePosition))
        {
            Undo.RecordObject(gameObject, gameObject.activeSelf ? "Deactivate GameObject" : "Activate GameObject");
            gameObject.SetActive(!gameObject.activeSelf);
            EditorUtility.SetDirty(gameObject);
            Event.current.Use();
            EditorApplication.RepaintHierarchyWindow();
        }
    }
}
