using System;
using System.IO;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    public static SaveLoad Instance;
    private string fileName = "GameSave.json";
    private string filePath;
    LeaderBoardData leaderBoardData;
    private void Awake()
    {
        Instance = this;
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        filePath = Application.persistentDataPath + "/" + fileName;
        //Directory.CreateDirectory(filePath);
        if (File.Exists(filePath))
        {
            //get data
            LoadFromFile();
        }
        else
        {
            Debug.Log("file not found , creating one");
            leaderBoardData = new LeaderBoardData();
            SaveGame();
        }
    }
    public bool CheckFileExist()
    {
        return File.Exists(filePath);
    }

    public LeaderBoardData GetData()
    {
        return leaderBoardData;
    }
    public void LoadFromFile()
    {
        string data = File.ReadAllText(filePath);
        leaderBoardData = JsonUtility.FromJson<LeaderBoardData>(data);

        
        Debug.Log("Game loaded from file");
    }
    public void SaveGame()
    {
        string data = JsonUtility.ToJson(leaderBoardData, true);
        File.WriteAllText(filePath, data);
        Debug.Log("Game saved");
    }
}
