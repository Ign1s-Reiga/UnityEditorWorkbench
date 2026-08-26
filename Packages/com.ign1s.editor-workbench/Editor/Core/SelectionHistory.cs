using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ign1s.EditorWorkbench;

[Serializable]
internal sealed class SelectionHistory
{
    [SerializeField] private List<PersistentObjectReference> entries = new();
    [SerializeField] private int currentIndex = -1;

    internal bool CanMoveBack => currentIndex > 0;
    internal bool CanMoveForward => currentIndex >= 0 && currentIndex < entries.Count - 1;
    internal int Count => entries.Count;

    internal void Push(Object target)
    {
        if (target == null)
        {
            return;
        }

        Object current = Current();
        if (current == target)
        {
            return;
        }

        if (currentIndex < entries.Count - 1)
        {
            entries.RemoveRange(currentIndex + 1, entries.Count - currentIndex - 1);
        }

        PersistentObjectReference entry = new();
        entry.Capture(target);
        entries.Add(entry);

        if (entries.Count > WorkbenchConstants.MaximumHistoryEntries)
        {
            entries.RemoveAt(0);
        }

        currentIndex = entries.Count - 1;
    }

    internal Object MoveBack()
    {
        if (!CanMoveBack)
        {
            return Current();
        }

        currentIndex--;
        return Current();
    }

    internal Object MoveForward()
    {
        if (!CanMoveForward)
        {
            return Current();
        }

        currentIndex++;
        return Current();
    }

    internal Object Current()
    {
        if (currentIndex < 0 || currentIndex >= entries.Count)
        {
            return null;
        }

        return entries[currentIndex].Resolve();
    }
}
