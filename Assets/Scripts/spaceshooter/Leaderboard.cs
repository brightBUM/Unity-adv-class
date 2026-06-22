using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] LeaderBoardData leaderBoardData;
    [SerializeField] GameObject leaderboardPrefab;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TMP_InputField nameInput;
    int score = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        leaderBoardData = SaveLoad.Instance.GetData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SetScore()
    {
        Debug.Log("submit ");
        var name = nameInput.text;
        PlayerBase playerBase = new PlayerBase(name,score);
        leaderBoardData.playerData.Add(playerBase);
        SaveLoad.Instance.SaveGame();
    }
    public int GetHighestScore()
    {
        //sort the list , find the highest score
        return 0;
    }
    public bool IsNewHighScore( int score)
    {
        leaderboardPrefab.SetActive(true);
        this.score = score;
        scoreText.text = this.score.ToString();
        //check if current score is higher than existing high score
        return false;
    }
    public void CloseHighScore()
    {
        leaderboardPrefab.SetActive(false);

    }
}
[System.Serializable]
public class LeaderBoardData
{
    public List<PlayerBase> playerData;
}
[System.Serializable]
public class PlayerBase
{
    public string name;
    public int score;

    public PlayerBase(string name, int score)
    {
        this.name = name;
        this.score = score;
    }
}
