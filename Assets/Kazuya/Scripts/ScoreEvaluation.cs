using UnityEngine;
using System.Collections;
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
    }
    Evaluation evaluation;
    [SerializeField]public  int testScore = 10000;
    [Header("�]���p�̃I�u�W�F�N�g")]
    [SerializeField] GameObject EvaluationTextA;
    [SerializeField] GameObject EvaluationTextB;
    [SerializeField] GameObject EvaluationTextC;
    [Header("�]���̊�ƂȂ�_��")]
    [SerializeField] int evaluationA;
    [SerializeField] int evaluationB;
    [SerializeField] int evaluationC;

    [Header("�L�����o�X�̎��")]
    [SerializeField] Canvas EvaluationCanvas;
    [SerializeField] Canvas RankingCanvas;

    [Header("���v�X�R�A")]
    [SerializeField] Text scoretext;

    [SerializeField] float waittime = 3.0f;//�ҋ@����





    void Start()
    {
        /*
        //�X�R�A�ɂ�郉���N����
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
        //�X�R�A�ɂ�郉���N����
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
        //�����N�̕\��
        switch (evaluation)
        {
            case Evaluation.A:
                EvaluationTextA.SetActive(true);
               // Debug.Log("�]��A");
                break;
            case Evaluation.B:
                EvaluationTextB.SetActive(true);
                //  Debug.Log("�]��B");
                break;
            case Evaluation.C:
                EvaluationTextC.SetActive(true);
                //  Debug.Log("�]��C");
                break;
            default:
              //  Debug.Log("�]���O");
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


