using UnityEditor;
using UnityEngine;

namespace Ign1s.EditorWorkbench;

internal static class AssetGuidUtility
{
    internal static bool TryGetSelectedFolder(out string guid, out string path)
    {
        Object selectedObject = Selection.activeObject;
        if (selectedObject == null)
        {
            guid = string.Empty;
            path = string.Empty;
            return false;
        }

        path = AssetDatabase.GetAssetPath(selectedObject);
        if (string.IsNullOrEmpty(path) || !AssetDatabase.IsValidFolder(path))
        {
            guid = string.Empty;
            return false;
        }

        guid = AssetDatabase.AssetPathToGUID(path);
        return !string.IsNullOrEmpty(guid);
    }

    internal static Object LoadFolder(string guid)
    {
        string path = AssetDatabase.GUIDToAssetPath(guid);
        return string.IsNullOrEmpty(path) ? null : AssetDatabase.LoadAssetAtPath<DefaultAsset>(path);
    }
}
