using UnityEngine;
using Unity.Netcode;

namespace Things.Game.Bootstrap
{
    public class NetworkBootstrapper : MonoBehaviour
    {
        private void Start()
        {
            // Ensures NetworkManager is initialized
            if (NetworkManager.Singleton == null)
            {
                Debug.LogError("[NetworkBootstrapper] NetworkManager is missing from the scene!");
            }
            else
            {
                Debug.Log("[NetworkBootstrapper] NetworkManager is ready.");
            }
        }
    }
}
