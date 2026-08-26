using UnityEditor;
using UnityEngine;

namespace Ign1s.EditorWorkbench;

[InitializeOnLoad]
internal static class ProjectWindowDecorator
{
    static ProjectWindowDecorator()
    {
        EditorApplication.projectWindowItemOnGUI -= DrawProjectItem;
        EditorApplication.projectWindowItemOnGUI += DrawProjectItem;
    }

    private static void DrawProjectItem(string guid, Rect selectionRect)
    {
        if (Event.current.type != EventType.Repaint || !ProjectWorkbenchSettings.instance.ProjectBrowserDecorationsEnabled)
        {
            return;
        }

        string path = AssetDatabase.GUIDToAssetPath(guid);
        if (string.IsNullOrEmpty(path) || !AssetDatabase.IsValidFolder(path))
        {
            return;
        }

        bool hasAppearance = ProjectWorkbenchSettings.instance.FolderAppearances.TryGet(guid, out FolderAppearance appearance);
        bool isFavorite = UserWorkbenchSettings.instance.FavoriteFolders.Contains(guid);
        if (!hasAppearance && !isFavorite)
        {
            return;
        }

        if (hasAppearance && appearance.HasCustomTint)
        {
            Color tint = appearance.Tint;
            tint.a = Mathf.Min(tint.a, 0.28f);
            EditorGUI.DrawRect(selectionRect, tint);

            Rect accentRect = new(selectionRect.x, selectionRect.y, 3f, selectionRect.height);
            Color accent = appearance.Tint;
            accent.a = 0.9f;
            EditorGUI.DrawRect(accentRect, accent);
        }

        if (hasAppearance && !string.IsNullOrEmpty(appearance.IconName))
        {
            GUIContent icon = EditorGUIUtility.IconContent(appearance.IconName);
            if (icon.image != null)
            {
                float size = Mathf.Min(16f, selectionRect.height - 2f);
                Rect iconRect = new(selectionRect.x + 3f, selectionRect.y + 1f, size, size);
                GUI.DrawTexture(iconRect, icon.image, ScaleMode.ScaleToFit, true);
            }
        }

        if (isFavorite)
        {
            GUIContent favoriteIcon = EditorGUIUtility.IconContent("Favorite");
            if (favoriteIcon.image != null)
            {
                float size = Mathf.Min(14f, selectionRect.height - 2f);
                Rect favoriteRect = new(selectionRect.xMax - size - 3f, selectionRect.y + 1f, size, size);
                GUI.DrawTexture(favoriteRect, favoriteIcon.image, ScaleMode.ScaleToFit, true);
            }
        }
    }
}
