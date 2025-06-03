using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class DisplayScores : MonoBehaviour
{
    [SerializeField] PointList pointlist;
    [SerializeField] Text NumberEyes;//�G�l�~�[�̖ڂ̐�
    [SerializeField] Text GostType;//�G�l�~�[�̎��
    [SerializeField] Text Rarity;//���A�x
    [SerializeField] Text BonusPoints;//�{�[�i�X�|�C���g
    [SerializeField] Text AddScore;//�݌vPoint
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
        NumberEyes.text = $"{Eyse}��";
        GostType.text = $"{Coward}�́@{Furious}��";
        Rarity.text = $"{Raritys}";
        BonusPoints.text = $"{BonusPointss}";
        AddScore.text = $"{Scores}";
    }
}
