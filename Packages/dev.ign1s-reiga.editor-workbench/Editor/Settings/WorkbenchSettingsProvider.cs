using UnityEditor;
using UnityEngine;

namespace Ign1s.EditorWorkbench;

internal static class WorkbenchSettingsProvider
{
    [SettingsProvider]
    private static SettingsProvider CreateProvider()
    {
        return new SettingsProvider(WorkbenchConstants.SettingsPath, SettingsScope.Project)
        {
            label = "Editor Workbench",
            guiHandler = DrawSettings,
            keywords = new System.Collections.Generic.HashSet<string>
            {
                "folder", "inspector", "hierarchy", "favorite", "workbench"
            }
        };
    }

    private static void DrawSettings(string searchContext)
    {
        ProjectWorkbenchSettings settings = ProjectWorkbenchSettings.instance;
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.LabelField("Decorations", EditorStyles.boldLabel);
        settings.ProjectBrowserDecorationsEnabled = EditorGUILayout.Toggle(
            "Project Browser",
            settings.ProjectBrowserDecorationsEnabled);
        settings.HierarchyDecorationsEnabled = EditorGUILayout.Toggle(
            "Hierarchy",
            settings.HierarchyDecorationsEnabled);

        if (EditorGUI.EndChangeCheck())
        {
            settings.SaveSettings();
            EditorApplication.RepaintProjectWindow();
            EditorApplication.RepaintHierarchyWindow();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Tools", EditorStyles.boldLabel);

        if (GUILayout.Button("Open Workbench Inspector", GUILayout.Width(220f)))
        {
            WorkbenchInspectorWindow.OpenWindow();
        }

        if (GUILayout.Button("Open Folder Favorites", GUILayout.Width(220f)))
        {
            FolderFavoritesWindow.OpenWindow();
        }

        EditorGUILayout.Space();
        EditorGUILayout.HelpBox(
            "Shared folder appearance is stored in ProjectSettings. Folder favorites are stored per user in UserSettings.",
            MessageType.Info);
    }
}
