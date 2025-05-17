using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
public class ScoreEvaluation : MonoBehaviour
{
    /// <summary>
    /// �]���i�K
    /// </summary>
    enum Evaluation
    {
        A,
        B,
        C,
        D
    }
    Evaluation evaluation;

    List<int> ranc = new List<int>();

    [SerializeField] public int ResltScore;
    [SerializeField] int evaluationA;
    [SerializeField] int evaluationB;
    [SerializeField] int evaluationC;
    [SerializeField] Text scoretext;





    void Start()
    {

        //�X�R�A�ɂ�郉���N����
        if (ResltScore < evaluationC)
        {
            evaluation = Evaluation.D;
        }
        else if (ResltScore < evaluationB)
        {
            evaluation = Evaluation.C;
        }
        else if(ResltScore < evaluationA)
        {
            evaluation = Evaluation.B;
        }
        else
        {
            evaluation = Evaluation.A;
        }
    }

    void Update()
    {
        //�����N�̕\��
        switch (evaluation)
        {
            case Evaluation.A:
               // Debug.Log("�]��A");
                break;
            case Evaluation.B:
              //  Debug.Log("�]��B");
                break;
            case Evaluation.C:
              //  Debug.Log("�]��C");
                break;
            default:
              //  Debug.Log("�]���O");
                break;
        }

        scoretext.text = ResltScore.ToString();
    }
}
