using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class DisplayScores : MonoBehaviour
{
    [SerializeField] PointList pointlist;
    [SerializeField] Text NumberEyes;//エネミーの目の数
    [SerializeField] Text GostType;//エネミーの種類
    [SerializeField] Text Rarity;//レア度
    [SerializeField] Text BonusPoints;//ボーナスポイント
    [SerializeField] Text AddScore;//累計Point
    [SerializeField]GameObject cameraMask;

    int Eyse;
    int Coward;
    int Furious;
    int Raritys;
    int BonusPointss;
    int Scores;
    void Start()
    {
        cameraMask = GameObject.Find("photo").gameObject;
    }

    void Update()
    {
        var enemies = pointlist.point;
        Eyse = enemies.Sum(e => e.eyes);
        Raritys = enemies.Sum(e => e.rarity);
        BonusPointss = enemies.Sum(e => e.bonus);
        Scores = Eyse+Raritys+BonusPointss;
        EnemyDataUpdate();
    }




    void EnemyDataUpdate()
    {
        NumberEyes.text = $"{Eyse}つ";
        GostType.text = $"{Coward}体　{Furious}体";
        Rarity.text = $"{Raritys}";
        BonusPoints.text = $"{BonusPointss}";
        AddScore.text = $"{Scores}";
    }
}
