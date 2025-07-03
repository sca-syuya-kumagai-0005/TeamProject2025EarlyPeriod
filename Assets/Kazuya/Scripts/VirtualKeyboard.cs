using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class VirtualKeyboard : MonoBehaviour
{
    [Header("���z�L�[�{�[�h�ݒ�")]
    public GameObject buttonPrefab;     //�����{�^���̃v���n�u
    public Transform buttonContainer;   //�{�^���̕��ׂ�e�I�u�W�F�N�g
    public GameObject emptoPrefab;      //�󔒃{�^��

    [Header("UI�R���|�[�l���g")]
    public Text nameText;               //���͒��̖��O�̃e�L�X�g
    public Button dakutenButton;    //���_�{�^��
    public Button handakutenButton; //�����_�{�^��
    public Button deleteButton;          //1�����폜�{�^��
    public Button alldeteButton;        //�S�����폜�{�^��
    public Button submitButton;         //���O�̊m��{�^��

    public Action<string> OnNameSubmitted;  //���O�����͂��ꂽ���}

    private string currentName = "";        //���͒��̖��O
    [Header("����������")]
    [SerializeField]private int maxLength = 7;        //�ő�̓��͐�

    //���C�A�E�g
    string[] katakanaChars = new string[]
    {
        "�[","��","��","��","�}","�n","�i","�^","�T","�J","�A",
        "�b","","��","","�~","�q","�j","�`","�V","�L","�C",
        "��","��","��","��","��","�t","�k","�c","�X","�N","�E",
        "��","","��","","��","�w","�l","�e","�Z","�P","�G",
        "��","��","��","��","��","�z","�m","�g","�\","�R","�I",
    
    };

    Dictionary<string, string> dakutenMap = new Dictionary<string, string>() {
    {"�J", "�K"}, {"�L", "�M"}, {"�N", "�O"}, {"�P", "�Q"}, {"�R", "�S"},
    {"�T", "�U"}, {"�V", "�W"}, {"�X", "�Y"}, {"�Z", "�["}, {"�\", "�]"},
    {"�^", "�_"}, {"�`", "�a"}, {"�c", "�d"}, {"�e", "�f"}, {"�g", "�h"},
    {"�n", "�o"}, {"�q", "�r"}, {"�t", "�u"}, {"�w", "�x"}, {"�z", "�{"},
    {"�p", "�o"}, {"�s", "�r"}, {"�v", "�u"}, {"�y", "�x"}, {"�|", "�{"}
    };

    Dictionary<string, string> reverseDakutenMap = new()
{
    {"�K", "�J"}, {"�M", "�L"}, {"�O", "�N"}, {"�Q", "�P"}, {"�S", "�R"},
    {"�U", "�T"}, {"�W", "�V"}, {"�Y", "�X"}, {"�[", "�Z"}, {"�]", "�\"},
    {"�_", "�^"}, {"�a", "�`"}, {"�d", "�c"}, {"�f", "�e"}, {"�h", "�g"},
    {"�o", "�n"}, {"�r", "�q"}, {"�u", "�t"}, {"�x", "�w"}, {"�{", "�z"}
};

    Dictionary<string, string> handakutenMap = new Dictionary<string, string>() {
    {"�n", "�p"}, {"�q", "�s"}, {"�t", "�v"}, {"�w", "�y"}, {"�z", "�|"},
    {"�o", "�p"}, {"�r", "�s"}, {"�u", "�v"}, {"�x", "�y"}, {"�{", "�|"}
    };

    Dictionary<string, string> reverseHandakutenMap = new()
    {
    {"�p", "�n"}, {"�s", "�q"}, {"�v", "�t"}, {"�y", "�w"}, {"�|", "�z"}
    };

    void Start()
    {
        foreach(string Key in katakanaChars)
        {
            GameObject btn;

            if (string.IsNullOrWhiteSpace(Key))
            {
                btn = Instantiate(emptoPrefab,buttonContainer);
            }
            else
            {
                btn = Instantiate(buttonPrefab, buttonContainer);
                btn.GetComponentInChildren<Text>().text = Key;
               string capturedkey = Key;
                //�{�^���ɃN���b�N�C�x���g�̓o�^
                btn.GetComponent<Button>().onClick.AddListener(() => OnClickCharacter(capturedkey));
            }
        }
        // �폜�{�^���Ƒ��M�{�^���̐ݒ�
        deleteButton.onClick.AddListener(DeleteCharacter);
        submitButton.onClick.AddListener(SubmitName);
        dakutenButton.onClick.AddListener(AddDakuten);
        handakutenButton.onClick.AddListener(AddHandakuten);
        alldeteButton.onClick.AddListener(AllDelete);

        UpdateDisplay(); // �ŏ��ɕ\�����X�V
    }
    //�����̒ǉ�����
    void OnClickCharacter(string character)
    {
        if (currentName.Length < maxLength)
        {
            currentName+= character;
            UpdateDisplay();
        }
    }
    //1�����폜
    void DeleteCharacter()
    {
        if (currentName.Length > 0)
        {
            currentName = currentName.Substring(0, currentName.Length - 1);
            UpdateDisplay();
        }
    }

    void AllDelete()
    {
        if (currentName.Length > 0)
        {
            currentName = "";
            UpdateDisplay();
        }
    }
    /// <summary>
    /// �����̑��_��
    /// </summary>
    void AddDakuten()
    {
        if(currentName.Length == 0)  return;
        string last = currentName[^1].ToString();
        if(dakutenMap.TryGetValue(last, out string converted))
        {
            currentName = currentName.Substring(0,currentName.Length-1)+converted;
        }
        else if(reverseDakutenMap.TryGetValue(last, out string original))
        {
            currentName = currentName.Substring(0, currentName.Length - 1) + original;
        }
        UpdateDisplay();
    }
    /// <summary>
    /// �����̔����_��
    /// </summary>
    void AddHandakuten()
    {
        if (currentName.Length == 0) return;
        string last = currentName[^1].ToString();
        if (handakutenMap.TryGetValue(last, out string converted))
        {
            currentName = currentName.Substring(0, currentName.Length - 1) + converted;
        }
        else if (reverseHandakutenMap.TryGetValue(last, out string original))
        {
            currentName = currentName.Substring(0, currentName.Length - 1) + original;
        }
        UpdateDisplay();
    }

    //���O�̊m��
    void SubmitName()
    {
        if(!string.IsNullOrEmpty(currentName))
        {
            OnNameSubmitted?.Invoke(currentName);//�R�[���o�b�N��ScoreRanking�ɓn��
        }
    }
    /// <summary>
    /// ���͒��̖��O�̕\���̍X�V
    /// </summary>
    void UpdateDisplay()
    {
        nameText.text = currentName;
    }
}
