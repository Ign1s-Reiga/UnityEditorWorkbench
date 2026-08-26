using System;
using UnityEngine;

namespace Ign1s.EditorWorkbench;

[Serializable]
internal sealed class FolderAppearance
{
    [SerializeField] private string guid = string.Empty;
    [SerializeField] private bool hasCustomTint;
    [SerializeField] private Color tint = default;
    [SerializeField] private string iconName = string.Empty;

    internal FolderAppearance(string folderGuid)
    {
        guid = folderGuid;
        tint = WorkbenchConstants.DefaultFolderTint;
    }

    internal string Guid => guid;
    internal bool HasCustomTint { get => hasCustomTint; set => hasCustomTint = value; }
    internal Color Tint { get => tint; set => tint = value; }
    internal string IconName { get => iconName; set => iconName = value ?? string.Empty; }
}
