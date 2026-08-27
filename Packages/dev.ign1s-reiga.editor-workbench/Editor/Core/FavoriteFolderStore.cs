using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ign1s.EditorWorkbench;

[Serializable]
internal sealed class FavoriteFolderStore
{
    [SerializeField] private List<string> folderGuids = new();

    internal IReadOnlyList<string> FolderGuids => folderGuids;

    internal bool Contains(string guid)
    {
        return folderGuids.Contains(guid);
    }

    internal bool Toggle(string guid)
    {
        if (string.IsNullOrWhiteSpace(guid))
        {
            throw new ArgumentException("A folder GUID is required.", nameof(guid));
        }

        if (folderGuids.Remove(guid))
        {
            return false;
        }

        folderGuids.Add(guid);
        return true;
    }

    internal bool Remove(string guid)
    {
        return folderGuids.Remove(guid);
    }
}
