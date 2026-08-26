using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ign1s.EditorWorkbench;

[Serializable]
internal sealed class FolderAppearanceStore
{
    [SerializeField] private List<FolderAppearance> entries = new();

    internal IReadOnlyList<FolderAppearance> Entries => entries;

    internal bool TryGet(string guid, out FolderAppearance appearance)
    {
        for (int index = 0; index < entries.Count; index++)
        {
            FolderAppearance candidate = entries[index];
            if (candidate.Guid == guid)
            {
                appearance = candidate;
                return true;
            }
        }

        appearance = null;
        return false;
    }

    internal FolderAppearance GetOrCreate(string guid)
    {
        if (string.IsNullOrWhiteSpace(guid))
        {
            throw new ArgumentException("A folder GUID is required.", nameof(guid));
        }

        if (TryGet(guid, out FolderAppearance existing))
        {
            return existing;
        }

        FolderAppearance created = new(guid);
        entries.Add(created);
        return created;
    }

    internal bool Remove(string guid)
    {
        for (int index = entries.Count - 1; index >= 0; index--)
        {
            if (entries[index].Guid != guid)
            {
                continue;
            }

            entries.RemoveAt(index);
            return true;
        }

        return false;
    }
}
