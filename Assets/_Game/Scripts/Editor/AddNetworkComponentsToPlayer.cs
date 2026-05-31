#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

namespace Things.Game.Editor
{
    public static class AddNetworkComponentsToPlayer
    {
        [MenuItem("Tools/Add Network Components to Player")]
        public static void AddComponents()
        {
            string path = "Assets/_Game/Prefabs/Legacy/Prefabs/Player/Player.prefab";
            
            try
            {
                using (var editingScope = new PrefabUtility.EditPrefabContentsScope(path))
                {
                    var prefabRoot = editingScope.prefabContentsRoot;
                    bool changed = false;
                    
                    if (prefabRoot.GetComponent<NetworkObject>() == null)
                    {
                        prefabRoot.AddComponent<NetworkObject>();
                        changed = true;
                    }

                    if (prefabRoot.GetComponent<NetworkTransform>() == null)
                    {
                        prefabRoot.AddComponent<NetworkTransform>();
                        changed = true;
                    }

                    if (changed)
                    {
                        Debug.Log("Successfully added NetworkObject and NetworkTransform and saved prefab!");
                    }
                    else
                    {
                        Debug.Log("Player prefab already has NetworkObject and NetworkTransform.");
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error editing prefab: " + e.Message);
            }
        }
    }
}
#endif
