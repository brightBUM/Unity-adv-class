using NUnit.Framework;
using System;
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
    public static Action OnFileLoad;

    private void OnEnable()
    {
        OnFileLoad += GetDataOnFileLoad;
    }
    private void Start()
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
    private void GetDataOnFileLoad()
    {
        leaderBoardData = SaveLoad.Instance.GetData();
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
   
    public bool IsNewHighScore( int score)
    {
        this.score = score;
        scoreText.text = this.score.ToString();
        //check if current score is higher than existing high score

        if (score<=0)
        {
            CloseHighScore();
        }
        ShowHighScore();
        
        return false;
    }
    public void WatchReward()
    {
        LevelPlaySample.instance.rewardedVideoAd.ShowAd();
        LevelPlaySample.instance.rewardedVideoAd.OnAdRewarded += RewardedVideoAd_OnAdRewarded;
    }


    private void RewardedVideoAd_OnAdRewarded(Unity.Services.LevelPlay.LevelPlayAdInfo arg1, Unity.Services.LevelPlay.LevelPlayReward arg2)
    {
        DoubleCoins();
        scoreText.text = score.ToString();
        LevelPlaySample.instance.rewardedVideoAd.LoadAd();
        LevelPlaySample.instance.rewardedVideoAd.OnAdRewarded -= RewardedVideoAd_OnAdRewarded;
    }
    public void DoubleCoins()
    {
        score += score;
    }
    public void ShowHighScore()
    {
        LevelPlaySample.instance.bannerAd.LoadAd();
        highScorePanel.SetActive(true);

    }
    public void CloseHighScore()
    {
        highScorePanel.SetActive(false);
        leaderBoardPanel.SetActive(true);
        PopulateLeaderBoardUI();
        LevelPlaySample.instance.bannerAd.HideAd();
    }
    private void OnDisable()
    {
        OnFileLoad -= GetDataOnFileLoad;


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
