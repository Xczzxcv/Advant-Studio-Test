using System.IO;
using System.Text;
using UnityEngine;

namespace Core.GameData
{
internal class LocalJsonSaveDataHandler : ISaveDataHandler
{
    private const string SaveFileName = "save.json";

    private readonly string _saveFilePath;

    public LocalJsonSaveDataHandler()
    {
        _saveFilePath = Path.Join(
            Application.persistentDataPath,
            SaveFileName);
    }

    public bool IsSaveDataExists()
    {
        return IsFileExists(_saveFilePath);
    }

    public bool TryGetSaveData(out GameData gameData)
    {
        if (!TryReadFile(_saveFilePath, out var gameDataBytes))
        {
            gameData = default;
            return false;
        }

        gameData = SerializationHelper.DeserializeFromJson<GameData>(gameDataBytes);
        return true;
    }

    public void SaveData<T>(T data)
    {
        var gameDataJsonString = SerializationHelper.SerializeToJson(data);
        WriteToFile(gameDataJsonString, _saveFilePath);
    }

    private static void WriteToFile(string gameDataJsonString, string filePath)
    {
        using var gameDataFile = new FileStream(filePath, FileMode.Create);
        var gameDataBytes = Encoding.ASCII.GetBytes(gameDataJsonString);
        gameDataFile.Write(gameDataBytes);
    }

    private static bool IsFileExists(string filePath)
    {
        var gameDataSaveFileInfo = new FileInfo(filePath);
        var exists = gameDataSaveFileInfo.Exists;
        return exists;
    }

    private static bool TryReadFile(string filePath, out byte[] gameDataFileBytes)
    {
        gameDataFileBytes = default;
        
        int fileLength;
        int readBytes;
        using (var gameDataFile = new FileStream(filePath, FileMode.Open))
        {
            fileLength = (int) gameDataFile.Length;
            if (fileLength != gameDataFile.Length)
            {
                Debug.LogError($"Length of the file {gameDataFile.Length}, but we can read only {fileLength}");
                return false;
            }

            gameDataFileBytes = new byte[fileLength];
            readBytes = gameDataFile.Read(gameDataFileBytes);
        }

        if (readBytes != gameDataFileBytes.Length)
        {
            Debug.LogError($"We read only {readBytes}, but {fileLength} bytes is available");
            return false;
        }

        return true;
    }
}
}