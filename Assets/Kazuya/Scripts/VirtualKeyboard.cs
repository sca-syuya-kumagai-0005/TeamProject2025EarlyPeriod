using UnityEngine;
using UnityEngine.UI;
using System;

public class VirtualKeyboard : MonoBehaviour
{
    [Header("���z�L�[�{�[�h�ݒ�")]
    public GameObject buttonPrefab;     //�����{�^���̃v���n�u
    public Transform buttonContainer;   //�{�^���̕��ׂ�e�I�u�W�F�N�g

    [Header("UI�R���|�[�l���g")]
    public Text nameText;               //���͒��̖��O�̃e�L�X�g
    public Button deleteButton;          //1�����폜�{�^��
    public Button submitButton;         //���O�̊m��{�^��

    public Action<string> OnNameSubmitted;  //���O�����͂��ꂽ���}

    private string currentName = "";        //���͒��̖��O
    private const int maxLength = 7;        //�ő�̓��͐�

    string[] katakanaChars = new string[]
    {
        "�A","�C","�E","�G","�I",
        "�J","�L","�N","�P","�R",
        "�^","�`","�c","�e","�g",
        "�i","�j","�k","�l","�m",
        "�n","�q","�t","�w","�z",
        "�}","�~","��","��","��",
        "��",�@�@�@"��",�@�@"��",
        "��","��","��","��","��",
        "��",�@�@�@"��",�@�@"��",
        "�[","�b","��","��","��",
  
    };


    void Start()
    {
        foreach(string kana in katakanaChars)
        {
            GameObject btn = Instantiate(buttonPrefab, buttonContainer);
            btn.GetComponentInChildren<Text>().text = kana;

            //�{�^���ɃN���b�N�C�x���g�̓o�^
            btn.GetComponent<Button>().onClick.AddListener(() => OnClickCharacter(kana));
        }
        // �폜�{�^���Ƒ��M�{�^���̐ݒ�
        deleteButton.onClick.AddListener(DeleteCharacter);
        submitButton.onClick.AddListener(SubmitName);

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
