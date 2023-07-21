#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FunGames.Tools.Utils
{
    public class SceneBuilder
    {
        public static void AddPrefabsToScene(List<GameObject> prefabs)
        {
            foreach (var prefab in prefabs)
            {
                AddPrefabToScene(prefab);
            }
        }

        public static void AddPrefabToScene(GameObject prefab)
        {
            bool isInstantiated = false;
            foreach (var go in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                if (PrefabUtility.GetCorrespondingObjectFromSource(go) == prefab)
                {
                    isInstantiated = true;
                    Debug.LogWarning(prefab.name + " already in scene !");
                }
            }

            if (!isInstantiated)
                PrefabUtility.InstantiatePrefab(prefab);
        }
    }
}
#endif  