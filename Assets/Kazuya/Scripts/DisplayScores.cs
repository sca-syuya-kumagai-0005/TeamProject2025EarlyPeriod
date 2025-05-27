using UnityEngine;
using UnityEngine.UI;


public class DisplayScores : MonoBehaviour
{
    [SerializeField] Text NumberEyes;//�G�l�~�[�̖ڂ̐�
    [SerializeField] Text GostType;//�G�l�~�[�̎��
    [SerializeField] Text Rarity;//���A�x
    [SerializeField] Text BonusPoints;//�{�[�i�X�|�C���g
    [SerializeField] Text AddScore;//�݌vPoint

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
        NumberEyes.text     =$"{Eyse}��";
        GostType.text       =$"{Coward}�́@{Furious}��";
        Rarity.text         =$"{Raritys}" ;
        BonusPoints.text    =$"{BonusPointss}";
        AddScore.text       =$"{Scores}";
    }
}
