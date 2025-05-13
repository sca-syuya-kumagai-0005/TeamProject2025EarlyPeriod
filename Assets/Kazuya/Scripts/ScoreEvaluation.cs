using UnityEngine;
using Unity.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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
        D
    }
    Evaluation evaluation;
    public InputField inputField;
    public Text nameText;
    public string PlayerName;

    List<int> ranc = new List<int>();

    [SerializeField] public int ResltScore;
    [SerializeField] int evaluationA;
    [SerializeField] int evaluationB;
    [SerializeField] int evaluationC;





    void Start()
    {
        //InputFieldの取得
        inputField = inputField.GetComponent<InputField>();
        inputField.Select();//入力のタイミングで呼び出す

        //スコアによるランク判定
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
        //ランクの表示
        switch (evaluation)
        {
            case Evaluation.A:
                Debug.Log("評価A");
                break;
            case Evaluation.B:
                Debug.Log("評価B");
                break;
            case Evaluation.C:
                Debug.Log("評価C");
                break;
            default:
                Debug.Log("評価外");
                break;
        }
    }

    public void InputText()
    {
        nameText.text = inputField.text;
        PlayerName = inputField.text.ToString();
    }
}
