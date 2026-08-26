using UnityEditor;
using UnityEngine;

namespace Ign1s.EditorWorkbench;

[FilePath("ProjectSettings/Ign1s.EditorWorkbench.asset", FilePathAttribute.Location.ProjectFolder)]
internal sealed class ProjectWorkbenchSettings : ScriptableSingleton<ProjectWorkbenchSettings>
{
    [SerializeField] private bool projectBrowserDecorationsEnabled = true;
    [SerializeField] private bool hierarchyDecorationsEnabled = true;
    [SerializeField] private FolderAppearanceStore folderAppearances = new();

    internal bool ProjectBrowserDecorationsEnabled
    {
        get => projectBrowserDecorationsEnabled;
        set => projectBrowserDecorationsEnabled = value;
    }

    internal bool HierarchyDecorationsEnabled
    {
        get => hierarchyDecorationsEnabled;
        set => hierarchyDecorationsEnabled = value;
    }

    internal FolderAppearanceStore FolderAppearances => folderAppearances;

    internal void SaveSettings()
    {
        Save(true);
    }
}
