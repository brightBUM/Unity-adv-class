using TMPro;
using UnityEngine;

public class LeaderBoardItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI rankText;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI scoreText;
    
    public void SetUIData(int rank,string name,int score)
    {
        this.rankText.text = rank.ToString();
        this.nameText.text = name;
        this.scoreText.text = score.ToString();
    }
}
