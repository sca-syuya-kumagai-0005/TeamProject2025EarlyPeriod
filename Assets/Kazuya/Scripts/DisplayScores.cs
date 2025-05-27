using UnityEngine;
using UnityEngine.UI;


public class DisplayScores : MonoBehaviour
{
    [SerializeField] Text NumberEyes;//エネミーの目の数
    [SerializeField] Text GostType;//エネミーの種類
    [SerializeField] Text Rarity;//レア度
    [SerializeField] Text BonusPoints;//ボーナスポイント
    [SerializeField] Text AddScore;//累計Point

    int Eyse = 100;
    int Coward = 50;
    int Furious = 4;
    int Raritys = 8;
    int BonusPointss = 30;
    int Scores;
    void Start()
    {
        Scores = Eyse + Coward + Furious + Raritys + BonusPointss;
    }

    void Update()
    {
        NumberEyes.text     =$"{Eyse}つ";
        GostType.text       =$"{Coward}体　{Furious}体";
        Rarity.text         =$"{Raritys}" ;
        BonusPoints.text    =$"{BonusPointss}";
        AddScore.text       =$"{Scores}";
    }
}
