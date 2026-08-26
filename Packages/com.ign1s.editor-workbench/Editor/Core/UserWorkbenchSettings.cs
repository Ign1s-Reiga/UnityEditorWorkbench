using UnityEditor;
using UnityEngine;

namespace Ign1s.EditorWorkbench;

[FilePath("UserSettings/Ign1s.EditorWorkbench.user.asset", FilePathAttribute.Location.ProjectFolder)]
internal sealed class UserWorkbenchSettings : ScriptableSingleton<UserWorkbenchSettings>
{
    [SerializeField] private FavoriteFolderStore favoriteFolders = new();

    internal FavoriteFolderStore FavoriteFolders => favoriteFolders;

    internal void SaveSettings()
    {
        Save(true);
    }
}
