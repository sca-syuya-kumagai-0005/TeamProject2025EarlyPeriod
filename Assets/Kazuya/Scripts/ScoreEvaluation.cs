using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
public class ScoreEvaluation : MonoBehaviour
{
    /// <summary>
    /// ]żiK
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

        //XRAÉĉéNğè
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
        //NÌ\Ĥ
        switch (evaluation)
        {
            case Evaluation.A:
               // Debug.Log("]żA");
                break;
            case Evaluation.B:
              //  Debug.Log("]żB");
                break;
            case Evaluation.C:
              //  Debug.Log("]żC");
                break;
            default:
              //  Debug.Log("]żO");
                break;
        }

        scoretext.text = ResltScore.ToString();
    }
}
