using UnityEngine;
using UnityEditor;

// Utility to find Animator components with missing/null RuntimeAnimatorController.
// Use the menu items under Tools/Animator to scan current scene or project prefabs.
public static class AnimatorNullFinder
{
    [MenuItem("Tools/Animator/Find Null Animator Controllers in Scene")]
    public static void FindInScene()
    {
        var animators = Object.FindObjectsOfType<Animator>(true);
        int count = 0;
        foreach (var a in animators)
        {
            if (a == null) continue;
            if (a.runtimeAnimatorController == null)
            {
                Debug.LogWarning($"Animator with null controller: {GetGameObjectPath(a.gameObject)}", a.gameObject);
                count++;
            }
        }
        EditorUtility.DisplayDialog("Animator Scan", $"Found {count} animator(s) with null controller in the scene.", "OK");
    }

    [MenuItem("Tools/Animator/Find Null Animator Controllers in Project Prefabs")]
    public static void FindInProjectPrefabs()
    {
        var guids = AssetDatabase.FindAssets("t:Prefab");
        int count = 0;
        foreach (var g in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(g);
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null) continue;
            var animators = prefab.GetComponentsInChildren<Animator>(true);
            foreach (var a in animators)
            {
                if (a == null) continue;
                if (a.runtimeAnimatorController == null)
                {
                    Debug.LogWarning($"Prefab '{path}' has Animator with null controller: {GetGameObjectPath(a.gameObject)}", prefab);
                    count++;
                }
            }
        }
        EditorUtility.DisplayDialog("Animator Scan", $"Found {count} animator(s) with null controller in prefabs.", "OK");
    }

    [MenuItem("Tools/Animator/Remove Null Animator Components From Scene...")]
    public static void RemoveNullAnimatorsInScene()
    {
        if (!EditorUtility.DisplayDialog("Confirm", "Remove all Animator components with null controller from the scene? This cannot be undone (except via Undo)", "Remove", "Cancel"))
            return;

        var animators = Object.FindObjectsOfType<Animator>(true);
        int removed = 0;
        foreach (var a in animators)
        {
            if (a == null) continue;
            if (a.runtimeAnimatorController == null)
            {
                Undo.DestroyObjectImmediate(a);
                removed++;
            }
        }
        EditorUtility.DisplayDialog("Animator Cleanup", $"Removed {removed} Animator component(s) from scene.", "OK");
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
