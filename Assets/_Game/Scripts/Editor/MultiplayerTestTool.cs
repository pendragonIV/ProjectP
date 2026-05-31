#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

namespace Things.Game.Editor
{
    public class MultiplayerTestTool : EditorWindow
    {
        [MenuItem("Tools/Multiplayer Test Tool")]
        public static void ShowWindow()
        {
            GetWindow<MultiplayerTestTool>("Multiplayer Test");
        }

        private int clientsToLaunch = 1;
        private string buildName = "Things_Client.exe";

        private void OnGUI()
        {
            GUILayout.Label("Multiplayer Build & Run Tool", EditorStyles.boldLabel);
            
            EditorGUILayout.HelpBox("Công cụ này tự động Build game ra file .exe thu nhỏ và mở lên nhiều cửa sổ để bạn test Multiplayer (Client) trong khi vẫn có thể dùng Unity Editor làm Host.", MessageType.Info);
            
            clientsToLaunch = EditorGUILayout.IntSlider("Số lượng Client muốn mở", clientsToLaunch, 1, 4);
            
            GUILayout.Space(10);

            if (GUILayout.Button("1. Build & Tự Động Mở Client", GUILayout.Height(30)))
            {
                BuildAndLaunch();
            }
            
            if (GUILayout.Button("2. Mở Client (Không cần Build lại)", GUILayout.Height(30)))
            {
                LaunchClients();
            }
        }

        private void BuildAndLaunch()
        {
            string buildFolder = Path.Combine(Application.dataPath, "../Builds");
            if (!Directory.Exists(buildFolder))
            {
                Directory.CreateDirectory(buildFolder);
            }

            string buildPath = Path.Combine(buildFolder, buildName);

            // Get scenes from build settings
            var scenes = EditorBuildSettings.scenes;
            var scenePaths = new List<string>();
            foreach (var scene in scenes)
            {
                if (scene.enabled) scenePaths.Add(scene.path);
            }
            
            // Fallback to active scene if build settings is empty
            if (scenePaths.Count == 0)
            {
                scenePaths.Add(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().path);
            }

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = scenePaths.ToArray(),
                locationPathName = buildPath,
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.Development // Build nhanh hơn & cho phép debug
            };

            UnityEngine.Debug.Log("[MultiplayerTool] Đang Build Game, vui lòng đợi...");
            var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            
            if (report.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
            {
                UnityEngine.Debug.Log($"[MultiplayerTool] Build thành công!");
                LaunchClients();
            }
            else
            {
                UnityEngine.Debug.LogError("[MultiplayerTool] Build thất bại! Hãy check lại lỗi trong Console.");
            }
        }

        private void LaunchClients()
        {
            string buildPath = Path.Combine(Application.dataPath, "../Builds", buildName);
            if (!File.Exists(buildPath))
            {
                UnityEngine.Debug.LogError($"[MultiplayerTool] Không tìm thấy file chạy tại {buildPath}. Bạn cần Build trước!");
                return;
            }

            for (int i = 0; i < clientsToLaunch; i++)
            {
                Process.Start(buildPath);
            }
            
            UnityEngine.Debug.Log($"[MultiplayerTool] Đã mở {clientsToLaunch} cửa sổ Client.");
        }
    }
}
#endif
