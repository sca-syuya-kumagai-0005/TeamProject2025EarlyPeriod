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
    int Type = 4;
    int Raritys;
    int BonusPointss;
    int Scores;
    void Start()
    {
    }

    void Update()
    {
        NumberEyes.text     =$"�g���̂������̖ڂ̐�:       {Eyse}";
        GostType.text       =$"�������̎��:               {Type}";
        Rarity.text         =$"���A�x:{Raritys}" ;
        BonusPoints.text    =$"�{�[�i�X���_:{BonusPointss}";
        AddScore.text       =$"���v���_:{Scores}";
    }
}
