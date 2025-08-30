using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    private const string FolderName = "Saves";
    private const int CurrentVersion = 1;

    [Serializable]
    private class SaveEntry
    {
        public string id;      
        public string type;    
        public string json;    
    }

    [Serializable]
    private class SaveFile
    {
        public int version;
        public List<SaveEntry> entries = new List<SaveEntry>();
        public string savedAtIsoUtc;
        public string scene; 
    }

    public static string GetSavePath(string slot)
    {
        var dir = Path.Combine(Application.persistentDataPath, FolderName);
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        return Path.Combine(dir, $"{slot}.json");
    }

    public static bool Save(string slot)
    {
        try
        {
            var entries = new List<SaveEntry>();

            // Find all saveables in scene
            var saveables = UnityEngine.Object.FindObjectsByType<MonoBehaviour>(true);
            foreach (var mb in saveables)
            {
                if (mb is ISaveable s)
                {
                    var uid = mb.GetComponent<UniqueId>();
                    if (uid == null)
                    {
                        Debug.LogWarning($"ISaveable on {mb.name} has no UniqueId. Skipping.");
                        continue;
                    }

                    var state = s.CaptureState();
                    if (state == null) continue;

                    
                    string payloadJson = JsonUtility.ToJson(state);
                    entries.Add(new SaveEntry
                    {
                        id = uid.Id,
                        type = state.GetType().AssemblyQualifiedName, 
                        json = payloadJson
                    });
                }
            }

            var save = new SaveFile
            {
                version = CurrentVersion,
                entries = entries,
                savedAtIsoUtc = DateTime.UtcNow.ToString("o"),
                scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
            };

            string fileJson = JsonUtility.ToJson(save, prettyPrint: true);
            File.WriteAllText(GetSavePath(slot), fileJson);
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Save failed: {ex}");
            return false;
        }
    }

    public static bool Load(string slot)
    {
        try
        {
            var path = GetSavePath(slot);
            if (!File.Exists(path))
            {
                Debug.LogWarning($"No save file at {path}");
                return false;
            }

            var fileJson = File.ReadAllText(path);
            var save = JsonUtility.FromJson<SaveFile>(fileJson);

            var byId = new Dictionary<string, SaveEntry>();
            foreach (var e in save.entries) byId[e.id] = e;

            var saveables = UnityEngine.Object.FindObjectsByType<MonoBehaviour>(true);
            foreach (var mb in saveables)
            {
                if (mb is ISaveable s)
                {
                    var uid = mb.GetComponent<UniqueId>();
                    if (uid == null) continue;

                    if (byId.TryGetValue(uid.Id, out var entry))
                    {
                        var type = Type.GetType(entry.type);
                        if (type == null)
                        {
                            Debug.LogWarning($"Type not found for saved entry {entry.type}");
                            continue;
                        }
                        var payload = JsonUtility.FromJson(entry.json, type);
                        s.RestoreState(payload);
                    }
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Load failed: {ex}");
            return false;
        }
    }

    public static void Delete(string slot)
    {
        var path = GetSavePath(slot);
        if (File.Exists(path)) File.Delete(path);
    }

    public static bool Exists(string slot) => File.Exists(GetSavePath(slot));
}
