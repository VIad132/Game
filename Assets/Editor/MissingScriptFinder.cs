using UnityEngine;
using UnityEditor;

// Editor utility to find and optionally remove missing (null) MonoBehaviour components
// which commonly cause inspector exceptions like SerializedObjectNotCreatableException
// or GameObjectInspector null-reference exceptions.
public static class MissingScriptFinder
{
    [MenuItem("Tools/GameObject/Find Missing Scripts In Scene")]
    public static void FindMissingInScene()
    {
        var gos = Object.FindObjectsOfType<GameObject>(true);
        int found = 0;
        foreach (var go in gos)
        {
            var comps = go.GetComponents<Component>();
            for (int i = 0; i < comps.Length; i++)
            {
                if (comps[i] == null)
                {
                    Debug.LogWarning($"Missing script on GameObject '{GetGameObjectPath(go)}' (component index {i})", go);
                    found++;
                    // ping first found
                    EditorGUIUtility.PingObject(go);
                }
            }
        }
        EditorUtility.DisplayDialog("Missing Script Scan", $"Found {found} missing script component(s) in the scene.", "OK");
    }

    [MenuItem("Tools/GameObject/Find Missing Scripts In Project Prefabs")]
    public static void FindMissingInPrefabs()
    {
        var guids = AssetDatabase.FindAssets("t:Prefab");
        int found = 0;
        foreach (var g in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(g);
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null) continue;
            var nodes = prefab.GetComponentsInChildren<Transform>(true);
            foreach (var t in nodes)
            {
                var go = t.gameObject;
                var comps = go.GetComponents<Component>();
                for (int i = 0; i < comps.Length; i++)
                {
                    if (comps[i] == null)
                    {
                        Debug.LogWarning($"Prefab '{path}' contains GameObject '{GetGameObjectPath(go)}' with missing script (component index {i})", prefab);
                        found++;
                        EditorGUIUtility.PingObject(prefab);
                    }
                }
            }
        }
        EditorUtility.DisplayDialog("Missing Script Scan", $"Found {found} missing script component(s) in prefabs.", "OK");
    }

    [MenuItem("Tools/GameObject/Remove Missing Scripts From Scene...")]
    public static void RemoveMissingInScene()
    {
        if (!EditorUtility.DisplayDialog("Confirm removal", "Remove all missing-script components from the scene? This will remove the missing component entries (Undoable).", "Remove", "Cancel"))
            return;

        var gos = Object.FindObjectsOfType<GameObject>(true);
        int removedEntries = 0;
        foreach (var go in gos)
        {
            var comps = go.GetComponents<Component>();
            bool hasMissing = false;
            for (int i = 0; i < comps.Length; i++)
            {
                if (comps[i] == null)
                {
                    hasMissing = true;
                    break;
                }
            }
            if (hasMissing)
            {
                // Use Unity's helper to remove missing script entries from the GameObject
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
                removedEntries++;
                Undo.RegisterFullObjectHierarchyUndo(go, "Remove missing scripts");
            }
        }
        EditorUtility.DisplayDialog("Missing Script Cleanup", $"Cleaned {removedEntries} GameObject(s) (removed missing script entries).", "OK");
    }

    private static string GetGameObjectPath(GameObject go)
    {
        if (go == null) return "<null>";
        string path = go.name;
        var t = go.transform.parent;
        while (t != null)
        {
            path = t.name + "/" + path;
            t = t.parent;
        }
        return path;
    }
}
