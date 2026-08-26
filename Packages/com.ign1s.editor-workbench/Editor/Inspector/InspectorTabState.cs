using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ign1s.EditorWorkbench;

[Serializable]
internal sealed class InspectorTabState
{
    [SerializeField] private PersistentObjectReference target = new();
    [SerializeField] private SelectionHistory history = new();
    [SerializeField] private bool locked;
    [SerializeField] private Vector2 scrollPosition;

    internal bool Locked { get => locked; set => locked = value; }
    internal Vector2 ScrollPosition { get => scrollPosition; set => scrollPosition = value; }
    internal SelectionHistory History => history;

    internal Object ResolveTarget()
    {
        return target.Resolve();
    }

    internal void SetTarget(Object value, bool addToHistory)
    {
        target.Capture(value);
        if (addToHistory)
        {
            history.Push(value);
        }
    }

    internal string GetTitle()
    {
        Object value = ResolveTarget();
        if (value != null)
        {
            return ObjectNames.NicifyVariableName(value.name);
        }

        return string.IsNullOrEmpty(target.FallbackName) ? "Empty" : target.FallbackName;
    }
}
