using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace MartonioJunior.Core.Editor
{
    public static class MissingScriptTool
    {
        #region Menu Items
        [MenuItem("Tools/Log Missing Scripts/Project")]
        public static void LogMissingScriptsForProject()
        {
            foreach (var prefabPath in AssetDatabase.GetAllAssetPaths().Where(path => path.EndsWith(".prefab", System.StringComparison.OrdinalIgnoreCase))) {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                Check(prefab);
            }
        }

        [MenuItem("Tools/Log Missing Scripts/Scene")]
        public static void LogMissingScriptsForScene()
        {
            foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>(true)) {
                Check(obj);
            }
        }
        #endregion
        #region Methods
        public static void Check(GameObject gameObject)
        {
            if (gameObject.GetComponentsInChildren<Component>().Any(x => x == null))
                Debug.Log($"Missing script(s) found in {gameObject.name}", gameObject);
        }
        #endregion
    }
}