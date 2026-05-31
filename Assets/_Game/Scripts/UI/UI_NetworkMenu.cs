using UnityEngine;
using Unity.Netcode;

namespace Things.Game.UI
{
    public class UI_NetworkMenu : MonoBehaviour
    {
        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            {
                if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
                if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
                if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
            }
            else
            {
                GUILayout.Label("Mode: " + (NetworkManager.Singleton.IsHost ?
                    "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client"));
            }
            GUILayout.EndArea();
        }
    }
}
