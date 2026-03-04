using System;
using System.IO;
using System.Text;

using UnityEngine;

public struct GameSaveData
{
    // Progression
    public int currentWorld;
    public int currentLevel;
    public int[] unlockedWorlds;
    public int[] defeatedBosses;

    // Player
    public int currentHearts;
    public int maxHearts;
    public string[] unlockedAbilities;

    // Checkpoint
    public Vector3 lastCheckpointPosition;

    // Meta
    public string saveTimestamp;
}


public class SaveGameManager : MonoBehaviour
{
    public static SaveGameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SaveGame(int slotNumber, ref GameSaveData data)
    {
        data.saveTimestamp = DateTime.Now.ToString("yyyy-MM-dd");

        var jsonData = JsonUtility.ToJson(data);
        var savePath = GetSaveFilePath(slotNumber);

        File.WriteAllText(savePath, jsonData, Encoding.UTF8);
        Debug.Log("Game saved!");
    }

    public GameSaveData? LoadGame(int slotNumber)
    {
        if (!ExitSave(slotNumber))
        {
            Debug.LogError("No save file found for this slot!");
            return null;
        }

        var jsonData = File.ReadAllText($"save_{slotNumber}.json");

        return JsonUtility.FromJson<GameSaveData>(jsonData); ;
    }

    public void DeleteSave(int slotNumber)
    {
        var savePath = GetSaveFilePath(slotNumber);

        if (!File.Exists(savePath))
        {
            Debug.LogError("No save file found to delete!");
            return;
        }

        File.Delete(savePath);
        Debug.Log("Save file deleted.");
    }

    public bool ExitSave(int slotNumber)
    {
        var savePath = GetSaveFilePath(slotNumber);

        return File.Exists(savePath);
    }

    public string GetSaveSlotText(int slotNumber)
    {
        if (!ExitSave(slotNumber)) return "New Game";

        var jsonData = File.ReadAllText(GetSaveFilePath(slotNumber));
        var savedData = JsonUtility.FromJson<GameSaveData>(jsonData);

        return $"World {savedData.currentWorld} - Level {savedData.currentLevel}\nSaved on: {savedData.saveTimestamp}";
    }

    string GetSaveFilePath(int slotNumber)
    {
        var directory = Path.Combine(Application.persistentDataPath, ".runeblade");

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        return Path.Combine(directory, $"save_{slotNumber}.json");
    }
}
