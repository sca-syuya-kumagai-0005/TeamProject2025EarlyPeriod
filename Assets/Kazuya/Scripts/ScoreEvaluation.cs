using UnityEngine;
using Unity.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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
        //InputField�̎擾
        inputField = inputField.GetComponent<InputField>();
        inputField.Select();//���͂̃^�C�~���O�ŌĂяo��

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
                Debug.Log("�]��A");
                break;
            case Evaluation.B:
                Debug.Log("�]��B");
                break;
            case Evaluation.C:
                Debug.Log("�]��C");
                break;
            default:
                Debug.Log("�]���O");
                break;
        }
    }

    public void InputText()
    {
        nameText.text = inputField.text;
        PlayerName = inputField.text.ToString();
    }
}
