using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public class ScoreRanking : MonoBehaviour
{
    [SerializeField] int score;//���݂̃v���C���[�̃X�R�A
    [SerializeField] InputField nameInputField;//���O���͂�UI
    [SerializeField] Button subitButton;//���O���M�{�^��
    [SerializeField] Text[] rankingText = new Text[10];
    [SerializeField] GameObject TextObject;

    [Header("�e��{�^��")]
    [SerializeField] GameObject TitkeButton;
    [SerializeField] GameObject MainGameButton;

    List<(string name, int score)> rankingList = new List<(string name, int score)>();
    string filePath;

    bool Rankingfinish = false;

    ScoreEvaluation scoreEvaluation;
    [SerializeField] VirtualKeyboard virtualKeyboard;
    [SerializeField] GameObject virtualObject;
    [SerializeField] Canvas VirtualCanvas;
    [SerializeField] Canvas RankingCanvas;
    void Start()
    {
        scoreEvaluation = GetComponent<ScoreEvaluation>();
        score = Mouse.score;
        //score = scoreEvaluation.testScore;

        /// �����L���O�̎菇
        //StreamingAssets�t�H���_��CSV�t�@�C���̃p�X���擾
        filePath = Path.Combine(Application.streamingAssetsPath, "ScoreRanking.csv");
        /// �f�[�^�̓ǂݍ��� 
        LoadRanking();
        /// ���݂̃f�[�^�Ɠǂݍ��񂾃f�[�^���r����top10�ɓ��邩���m�F����
        if (IsHighScore(score))
        {
            /*
            //���O���͂�L����
            nameInputField.gameObject.SetActive(true);
            subitButton.gameObject.SetActive(true);

            //�{�^���ɃC�x���g�̓o�^
            subitButton.onClick.AddListener(OnSubmitName);*/
            TextObject.SetActive(false);
            virtualObject.SetActive(true);
            virtualKeyboard.OnNameSubmitted = OnSubmitNameWithKeyboard;
        }
        else
        {
            UpdateRankingDisplay();
            StartCoroutine(ButtonSetUp());
        }
    }


    //void OnSubmitName(string playerName)
    //{
    //    if (!string.IsNullOrEmpty(playerName))
    //    {
    //        InsertScore(playerName, score);
    //        SaveRanking();
    //        UpdateRankingDisplay();
    //        virtualKeyboard.gameObject.SetActive(false);
    //        StartCoroutine(ButtonSetUp());
    //    }
    //}


    void OnSubmitNameWithKeyboard(string playerName)
    {
        if (!string.IsNullOrEmpty(playerName))
        {
            InsertScore(playerName, score);
            SaveRanking();
            TextObject.SetActive(true);
            UpdateRankingDisplay();
            StartCoroutine(ButtonSetUp());
            virtualObject.SetActive(false);
            VirtualCanvas.enabled = false;

        }
    }

    /// <summary>
    /// �����L���O�̓ǂݍ���
    /// </summary>
    void LoadRanking()
    {

        //�t�@�C���̓ǂݍ���    
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] values = line.Split(',');

                if (values.Length >= 2 && int.TryParse(values[1], out int parsedScore))
                    {
                    rankingList.Add((values[0], parsedScore));
                }
            }
        }
        else
        {
            Debug.LogWarning("�����L���O�t�@�C����������܂���" + filePath);
        }
    }

    //csv�t�@�C���Ŏ����Ă����f�[�^���e�L�X�g�ɒu��
    void UpdateRankingDisplay()
    {
        TextObject.SetActive(true);
        for (int i = 0;i < rankingList.Count; i++)
        {
            if (i < rankingList.Count)
            {
                rankingText[i].text = $"{i + 1}��    :   {rankingList[i].name} :{rankingList[i].score}";

                // 1�ʂ���3�ʂ̐F��ύX
                if (i == 0)
                {
                    rankingText[i].color = Color.yellow; // 1�ʂ̓S�[���h
                }
                else if (i == 1)
                {
                    rankingText[i].color = Color.gray;   // 2�ʂ̓V���o�[
                }
                else if (i == 2)
                {
                    rankingText[i].color = new Color(0.8f, 0.5f, 0.2f); // 3�ʂ̓u�����Y���ۂ��F
                }
                else
                {
                    rankingText[i].color = Color.black; // ���̑��̏��ʂ͒ʏ�̔�
                }
            }
            else
            {
                rankingText[i].text = $"{i + 1}��; :";
                rankingText[i].color = Color.black;
            }
        }

    }

    //Top10�ɓ��邩�ǂ����̔���
    bool IsHighScore(int newScore)
    {
        //�����L���O��10�����Ȃ�X�R�A��ǉ�����
        if(rankingList.Count<10)return true;
        //�Œ�X�R�A(10��)���V�����X�R�A��������Βǉ�����
        return newScore > rankingList.Last().score;
    }
    /// �����L���O�̕ϓ�
    //�X�R�A�������L���O�ɒǉ����ĕ��ёւ�
    void InsertScore(string name,int newScore)
    {
        rankingList.Add((name,newScore));
        //�X�R�A����ёւ��ď�ʂ�ێ�
        rankingList = rankingList
        .OrderByDescending(entry => entry.score)
        .Take(10)
        .ToList();

        Debug.Log("�V�����X�R�A�������L���O�ɒǉ�");
        for(int i = 0;i < rankingList.Count; i++)
        {
            Debug.Log($"{i + 1}��:{rankingList[i].name},{rankingList[i].score}");
        }
    }
    /// �ϓ����������L���O��CSV�t�@�C���ɕۑ�
    void SaveRanking()
    {
        List<string> lines = new List<string>();
       lines.Add("���O,�X�R�A");
        foreach (var entry in rankingList)
        {
            lines.Add($"{entry.name},{entry.score}");
        }
        File.WriteAllLines(filePath,lines,Encoding.UTF8);
        Debug.Log("�����L���O�̕ۑ�");
    }

    IEnumerator ButtonSetUp()
    {
        yield return new WaitForSeconds(4.0f);
        TextObject.SetActive(true);
        TitkeButton.SetActive(true);
        MainGameButton.SetActive(true);
    }
}
