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
    int Type = 4;
    int Raritys;
    int BonusPointss;
    int Scores;
    void Start()
    {
    }

    void Update()
    {
        NumberEyes.text     =$"枠内のお化けの目の数:       {Eyse}";
        GostType.text       =$"お化けの種類:               {Type}";
        Rarity.text         =$"レア度:{Raritys}" ;
        BonusPoints.text    =$"ボーナス得点:{BonusPointss}";
        AddScore.text       =$"合計得点:{Scores}";
    }
}
