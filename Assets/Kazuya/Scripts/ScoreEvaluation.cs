using UnityEngine;
using Unity.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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
    public InputField inputField;
    public Text nameText;

    [SerializeField] int ResltScore;
    [SerializeField] int evaluationA;
    [SerializeField] int evaluationB;
    [SerializeField] int evaluationC;





    void Start()
    {
        //InputField‚ÌŽæ“¾
        inputField = inputField.GetComponent<InputField>();
        //nameText = inputField.GetComponent<Text>();
        if (ResltScore <= evaluationC)
        {
            evaluation = Evaluation.D;
        }
        else if (ResltScore <= evaluationB)
        {
            evaluation = Evaluation.C;
        }
        else if(ResltScore <= evaluationA)
        {
            evaluation = Evaluation.B;
        }
        else
        {
            evaluation = Evaluation.A;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (evaluation)
        {
            case Evaluation.A:
                Debug.Log("•]‰¿A");
                break;
            case Evaluation.B:
                Debug.Log("•]‰¿B");
                break;
            case Evaluation.C:
                Debug.Log("•]‰¿C");
                break;
            default:
                Debug.Log("•]‰¿ŠO");
                break;
        }
    }

    public void InputText()
    {
        nameText.text = inputField.text;
    }
}
