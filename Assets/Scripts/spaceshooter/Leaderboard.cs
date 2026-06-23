using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] LeaderBoardData leaderBoardData;
    [SerializeField] GameObject highScorePanel;
    [SerializeField] GameObject leaderBoardPanel;
    [SerializeField] GameObject leaderBoardItemPrefab;
    [SerializeField] Transform contentParent;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TMP_InputField nameInput;
    int score = 0;
    
    
    public void SetScore()
    {
        Debug.Log("submit ");
        var name = nameInput.text;
        PlayerBase playerBase = new PlayerBase(name,score);
        leaderBoardData.playerData.Add(playerBase);
        SaveLoad.Instance.SaveGame();
    }

    public void PopulateLeaderBoardUI()
    {
        var size = leaderBoardData.playerData.Count;
        var playerData = leaderBoardData.playerData;
        playerData.Sort((a, b) => b.score.CompareTo(a.score));

        for (int i = 0; i < size; i++)
        {
            var leaderboardItemObject = Instantiate(leaderBoardItemPrefab,contentParent);
            var leaderBoardItem = leaderboardItemObject.GetComponent<LeaderBoardItem>();
            leaderBoardItem.SetUIData(i + 1, playerData[i].name, playerData[i].score);
        }
    }
    public int GetHighestScore()
    {
        //sort the list , find the highest score
        return 0;
    }
    public bool IsNewHighScore( int score)
    {
        leaderBoardData = SaveLoad.Instance.GetData();
        if(score<=0)
        {
            CloseHighScore();
        }


        highScorePanel.SetActive(true);
        this.score = score;
        scoreText.text = this.score.ToString();
        //check if current score is higher than existing high score
        return false;
    }
    public void CloseHighScore()
    {
        highScorePanel.SetActive(false);
        leaderBoardPanel.SetActive(true);
        PopulateLeaderBoardUI();
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
