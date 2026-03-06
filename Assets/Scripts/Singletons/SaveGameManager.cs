using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

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

    public async Task<bool> SaveGame(int slotNumber, GameSaveData data)
    {
        try
        {
            var jsonData = JsonUtility.ToJson(data);
            var savePath = await GetSaveFilePath(slotNumber);

            await File.WriteAllTextAsync(savePath, jsonData, Encoding.UTF8);

            Debug.Log("Game saved!");

            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save game: {e.Message}");

            return false;
        }
    }

    public async Task<GameSaveData?> LoadGame(int slotNumber)
    {
        try
        {
            if (!await SaveExists(slotNumber))
            {
                Debug.LogError("No save file found for this slot!");

                return null;
            }

            var savePath = await GetSaveFilePath(slotNumber);
            var jsonData = await File.ReadAllTextAsync(savePath, Encoding.UTF8);

            return JsonUtility.FromJson<GameSaveData>(jsonData);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load game: {e.Message}");

            return null;
        }
    }

    public async Task<bool> DeleteSave(int slotNumber)
    {
        try
        {
            var savePath = await GetSaveFilePath(slotNumber);

            if (!await SaveExists(slotNumber))
            {
                Debug.LogError("No save file found to delete!");

                return false;
            }

            await Task.Run(() => File.Delete(savePath));

            Debug.Log("Save file deleted.");

            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to delete save: {e.Message}");
            return false;
        }
    }

    public async Task<bool> SaveExists(int slotNumber)
    {
        var savePath = await GetSaveFilePath(slotNumber);

        return await Task.FromResult(File.Exists(savePath));
    }

    public async Task<string> GetSaveSlotText(int slotNumber)
    {
        if (!await SaveExists(slotNumber)) return "New Game";

        var savePath = await GetSaveFilePath(slotNumber);
        var jsonData = await File.ReadAllTextAsync(savePath);
        var savedData = JsonUtility.FromJson<GameSaveData>(jsonData);

        return $"World {savedData.currentWorld} - Level {savedData.currentLevel}\nSaved on: {savedData.saveTimestamp}";
    }

    async Task<string> GetSaveFilePath(int slotNumber)
    {
        var directory = Path.Combine(Application.persistentDataPath, ".runeblade");
        var existDirectory = await Task.FromResult(Directory.Exists(directory));

        Debug.Log($"Game saved at {directory}");

        if (!existDirectory)
        {
            await Task.Run(() => Directory.CreateDirectory(directory));
        }

        return Path.Combine(directory, $"save_{slotNumber}.json");
    }
}
