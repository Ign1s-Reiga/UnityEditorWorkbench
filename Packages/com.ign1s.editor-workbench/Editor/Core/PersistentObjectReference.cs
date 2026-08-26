using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Ign1s.EditorWorkbench;

[Serializable]
internal sealed class PersistentObjectReference
{
    [SerializeField] private Object directReference;
    [SerializeField] private string globalObjectId = string.Empty;
    [SerializeField] private string fallbackName = string.Empty;

    internal Object DirectReference => directReference;
    internal string FallbackName => fallbackName;
    internal bool IsEmpty => directReference == null && string.IsNullOrEmpty(globalObjectId);

    internal void Capture(Object target)
    {
        directReference = target;
        fallbackName = target == null ? string.Empty : target.name;
        globalObjectId = string.Empty;

        if (target == null)
        {
            return;
        }

        GlobalObjectId identifier = GlobalObjectId.GetGlobalObjectIdSlow(target);
        globalObjectId = identifier.ToString();
    }

    internal Object Resolve()
    {
        if (directReference != null)
        {
            return directReference;
        }

        if (string.IsNullOrEmpty(globalObjectId) || !GlobalObjectId.TryParse(globalObjectId, out GlobalObjectId identifier))
        {
            return null;
        }

        directReference = GlobalObjectId.GlobalObjectIdentifierToObjectSlow(identifier);
        return directReference;
    }
}
