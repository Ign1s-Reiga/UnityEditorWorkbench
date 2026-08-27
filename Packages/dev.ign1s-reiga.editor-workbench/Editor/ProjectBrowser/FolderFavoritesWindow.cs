using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Ign1s.EditorWorkbench;

internal sealed class FolderFavoritesWindow : EditorWindow
{
    private static readonly List<FolderFavoritesWindow> OpenWindows = new();
    [SerializeField] private Vector2 scrollPosition;

    [MenuItem(WorkbenchConstants.RootMenu + "Folder Favorites", false, 20)]
    internal static void OpenWindow()
    {
        GetWindow<FolderFavoritesWindow>("Folder Favorites");
    }

    internal static void RepaintOpenWindows()
    {
        for (int index = OpenWindows.Count - 1; index >= 0; index--)
        {
            FolderFavoritesWindow window = OpenWindows[index];
            if (window == null)
            {
                OpenWindows.RemoveAt(index);
                continue;
            }

            window.Repaint();
        }
    }

    private void OnEnable()
    {
        if (!OpenWindows.Contains(this))
        {
            OpenWindows.Add(this);
        }
    }

    private void OnDisable()
    {
        OpenWindows.Remove(this);
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Add Selected Folder"))
        {
            AddSelectedFolder();
        }

        EditorGUILayout.Space();
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        IReadOnlyList<string> favorites = UserWorkbenchSettings.instance.FavoriteFolders.FolderGuids;
        if (favorites.Count == 0)
        {
            EditorGUILayout.HelpBox("No favorite folders yet.", MessageType.Info);
        }

        for (int index = 0; index < favorites.Count; index++)
        {
            DrawFavorite(favorites[index]);
        }

        EditorGUILayout.EndScrollView();
    }

    private static void AddSelectedFolder()
    {
        if (!AssetGuidUtility.TryGetSelectedFolder(out string guid, out _))
        {
            return;
        }

        if (!UserWorkbenchSettings.instance.FavoriteFolders.Contains(guid))
        {
            UserWorkbenchSettings.instance.FavoriteFolders.Toggle(guid);
            UserWorkbenchSettings.instance.SaveSettings();
            EditorApplication.RepaintProjectWindow();
            RepaintOpenWindows();
        }
    }

    private static void DrawFavorite(string guid)
    {
        string path = AssetDatabase.GUIDToAssetPath(guid);
        using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
        {
            if (string.IsNullOrEmpty(path))
            {
                EditorGUILayout.LabelField("Missing folder", GUILayout.ExpandWidth(true));
            }
            else if (GUILayout.Button(new GUIContent(path, EditorGUIUtility.IconContent("Folder Icon").image), EditorStyles.label))
            {
                Object folder = AssetGuidUtility.LoadFolder(guid);
                Selection.activeObject = folder;
                EditorGUIUtility.PingObject(folder);
            }

            if (GUILayout.Button("Remove", GUILayout.Width(64f)))
            {
                UserWorkbenchSettings.instance.FavoriteFolders.Remove(guid);
                UserWorkbenchSettings.instance.SaveSettings();
                EditorApplication.RepaintProjectWindow();
                RepaintOpenWindows();
                GUIUtility.ExitGUI();
            }
        }
    }
}
