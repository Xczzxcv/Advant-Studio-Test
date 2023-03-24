namespace Core.GameData
{
internal interface ISaveDataHandler
{
    bool IsSaveDataExists();
    bool TryGetSaveData(out GameData gameData);
    void SaveData<T>(T data);
}
}