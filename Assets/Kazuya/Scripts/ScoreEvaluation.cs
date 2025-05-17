using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
public class ScoreEvaluation : MonoBehaviour
{
    /// <summary>
    /// •]‰¿’iŠK
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

        //ƒXƒRƒA‚É‚æ‚éƒ‰ƒ“ƒN”»’è
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
        //ƒ‰ƒ“ƒN‚Ì•\Ž¦
        switch (evaluation)
        {
            case Evaluation.A:
               // Debug.Log("•]‰¿A");
                break;
            case Evaluation.B:
              //  Debug.Log("•]‰¿B");
                break;
            case Evaluation.C:
              //  Debug.Log("•]‰¿C");
                break;
            default:
              //  Debug.Log("•]‰¿ŠO");
                break;
        }

        scoretext.text = ResltScore.ToString();
    }
}
