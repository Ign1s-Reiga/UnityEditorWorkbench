using UnityEditor;
using UnityEngine;

namespace Ign1s.EditorWorkbench;

internal static class FolderContextMenuCommands
{
    [MenuItem(WorkbenchConstants.AssetsMenu + "Edit Folder Appearance...", false, 2000)]
    private static void EditFolderAppearance()
    {
        if (AssetGuidUtility.TryGetSelectedFolder(out string guid, out _))
        {
            FolderAppearanceWindow.Open(guid);
        }
    }

    [MenuItem(WorkbenchConstants.AssetsMenu + "Edit Folder Appearance...", true)]
    private static bool ValidateEditFolderAppearance()
    {
        return AssetGuidUtility.TryGetSelectedFolder(out _, out _);
    }

    [MenuItem(WorkbenchConstants.AssetsMenu + "Toggle Favorite", false, 2001)]
    private static void ToggleFavorite()
    {
        if (!AssetGuidUtility.TryGetSelectedFolder(out string guid, out _))
        {
            return;
        }

        UserWorkbenchSettings.instance.FavoriteFolders.Toggle(guid);
        UserWorkbenchSettings.instance.SaveSettings();
        EditorApplication.RepaintProjectWindow();
        FolderFavoritesWindow.RepaintOpenWindows();
    }

    [MenuItem(WorkbenchConstants.AssetsMenu + "Toggle Favorite", true)]
    private static bool ValidateToggleFavorite()
    {
        return AssetGuidUtility.TryGetSelectedFolder(out _, out _);
    }

    [MenuItem(WorkbenchConstants.AssetsMenu + "Clear Folder Appearance", false, 2002)]
    private static void ClearFolderAppearance()
    {
        if (!AssetGuidUtility.TryGetSelectedFolder(out string guid, out _))
        {
            return;
        }

        ProjectWorkbenchSettings.instance.FolderAppearances.Remove(guid);
        ProjectWorkbenchSettings.instance.SaveSettings();
        EditorApplication.RepaintProjectWindow();
    }

    [MenuItem(WorkbenchConstants.AssetsMenu + "Clear Folder Appearance", true)]
    private static bool ValidateClearFolderAppearance()
    {
        return AssetGuidUtility.TryGetSelectedFolder(out string guid, out _) &&
               ProjectWorkbenchSettings.instance.FolderAppearances.TryGet(guid, out _);
    }
}
