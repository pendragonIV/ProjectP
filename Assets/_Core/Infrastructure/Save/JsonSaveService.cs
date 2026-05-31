using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Things.Core.Infrastructure
{
    /// <summary>
    /// JSON-based save service using Application.persistentDataPath.
    /// Each key maps to a separate .json file for easy debugging and partial saves.
    /// Thread-safe for single-threaded Unity usage.
    /// 
    /// Future: Can be replaced with a CloudSaveService that implements
    /// the same ISaveService interface for multiplayer cloud saves.
    /// </summary>
    public class JsonSaveService : ISaveService, IDisposable
    {
        private readonly string saveDirectory;
        private readonly Dictionary<string, string> cache = new Dictionary<string, string>();
        private readonly HashSet<string> dirtyKeys = new HashSet<string>();

        /// <summary>
        /// Create a JsonSaveService.
        /// </summary>
        /// <param name="subFolder">Subfolder within persistentDataPath (e.g., "saves").</param>
        public JsonSaveService(string subFolder = "saves")
        {
            saveDirectory = Path.Combine(Application.persistentDataPath, subFolder);

            if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
            }

            Debug.Log($"[JsonSaveService] Save directory: {saveDirectory}");
        }

        public void Save<T>(string key, T data)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Save key cannot be null or empty.", nameof(key));

            string json = JsonUtility.ToJson(data, true);
            cache[key] = json;
            dirtyKeys.Add(key);
        }

        public T Load<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Load key cannot be null or empty.", nameof(key));

            // Check cache first
            if (cache.TryGetValue(key, out string cachedJson))
            {
                return JsonUtility.FromJson<T>(cachedJson);
            }

            // Load from file
            string filePath = GetFilePath(key);
            if (!File.Exists(filePath))
            {
                return default;
            }

            try
            {
                string json = File.ReadAllText(filePath);
                cache[key] = json;
                return JsonUtility.FromJson<T>(json);
            }
            catch (Exception e)
            {
                Debug.LogError($"[JsonSaveService] Failed to load '{key}': {e.Message}");
                return default;
            }
        }

        public bool HasKey(string key)
        {
            if (cache.ContainsKey(key)) return true;
            return File.Exists(GetFilePath(key));
        }

        public void Delete(string key)
        {
            cache.Remove(key);
            dirtyKeys.Remove(key);

            string filePath = GetFilePath(key);
            if (File.Exists(filePath))
            {
                try
                {
                    File.Delete(filePath);
                }
                catch (Exception e)
                {
                    Debug.LogError($"[JsonSaveService] Failed to delete '{key}': {e.Message}");
                }
            }
        }

        public void DeleteAll()
        {
            cache.Clear();
            dirtyKeys.Clear();

            try
            {
                if (Directory.Exists(saveDirectory))
                {
                    var files = Directory.GetFiles(saveDirectory, "*.json");
                    foreach (var file in files)
                    {
                        File.Delete(file);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[JsonSaveService] Failed to delete all saves: {e.Message}");
            }
        }

        /// <summary>
        /// Write all dirty (modified) keys to disk.
        /// Call this periodically or on application pause/quit.
        /// </summary>
        public void Flush()
        {
            if (dirtyKeys.Count == 0) return;

            foreach (var key in dirtyKeys)
            {
                if (cache.TryGetValue(key, out string json))
                {
                    try
                    {
                        string filePath = GetFilePath(key);
                        File.WriteAllText(filePath, json);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"[JsonSaveService] Failed to flush '{key}': {e.Message}");
                    }
                }
            }

            dirtyKeys.Clear();
        }

        public void Dispose()
        {
            Flush();
            cache.Clear();
        }

        private string GetFilePath(string key)
        {
            // Sanitize key for filesystem
            string safeKey = key.Replace('/', '_').Replace('\\', '_').Replace(':', '_');
            return Path.Combine(saveDirectory, $"{safeKey}.json");
        }
    }
}
