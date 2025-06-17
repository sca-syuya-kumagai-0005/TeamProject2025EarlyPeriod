using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
public class ScoreEvaluation : MonoBehaviour
{
    /// <summary>
    /// 評価段階
    /// </summary>
    enum Evaluation
    {
        A,
        B,
        C,
    }
    Evaluation evaluation;
    [SerializeField]public  int testScore = 10000;
    [Header("評価用のオブジェクト")]
    [SerializeField] GameObject EvaluationTextA;
    [SerializeField] GameObject EvaluationTextB;
    [SerializeField] GameObject EvaluationTextC;
    [Header("評価の基準となる点数")]
    [SerializeField] int evaluationA;
    [SerializeField] int evaluationB;
    [SerializeField] int evaluationC;

    [Header("キャンバスの種類")]
    [SerializeField] Canvas EvaluationCanvas;
    [SerializeField] Canvas RankingCanvas;

    [Header("合計スコア")]
    [SerializeField] Text scoretext;

    [SerializeField] float waittime = 3.0f;//待機時間





    void Start()
    {
        /*
        //スコアによるランク判定
        if (Mouse.score < evaluationC)
        {
            evaluation = Evaluation.C;
        }
        else if (Mouse.score < evaluationB)
        {
            evaluation = Evaluation.C;
        }
        else if (Mouse.score < evaluationA)
        {
            evaluation = Evaluation.B;
        }
        else
        {
            evaluation = Evaluation.A;
        }*/
        //スコアによるランク判定
        if (testScore < evaluationC)
        {
            evaluation = Evaluation.C;
        }
        else if (testScore < evaluationB)
        {
            evaluation = Evaluation.C;
        }
        else if (testScore < evaluationA)
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
        //ランクの表示
        switch (evaluation)
        {
            case Evaluation.A:
                EvaluationTextA.SetActive(true);
               // Debug.Log("評価A");
                break;
            case Evaluation.B:
                EvaluationTextB.SetActive(true);
                //  Debug.Log("評価B");
                break;
            case Evaluation.C:
                EvaluationTextC.SetActive(true);
                //  Debug.Log("評価C");
                break;
            default:
              //  Debug.Log("評価外");
                break;
        }

        scoretext.text = Mouse.score.ToString();
        StartCoroutine(WaitTime());
    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(3.0f);
        EvaluationCanvas.enabled = false;
        RankingCanvas.enabled = true;
    }

}


