using UnityEditor;

namespace Ign1s.EditorWorkbench;

internal static class WorkbenchMenu
{
    [MenuItem(WorkbenchConstants.RootMenu + "Project Settings", false, 100)]
    private static void OpenProjectSettings()
    {
        SettingsService.OpenProjectSettings(WorkbenchConstants.SettingsPath);
    }
}
