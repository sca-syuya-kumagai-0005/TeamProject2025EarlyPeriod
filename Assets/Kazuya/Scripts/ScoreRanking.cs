using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Unity.Android.Gradle;

public class ScoreRanking : MonoBehaviour
{
    [SerializeField] int score;//���݂̃v���C���[�̃X�R�A
    [SerializeField] InputField nameInputField;//���O���͂�UI
    [SerializeField] Button subitButton;//���O���M�{�^��


    List<(string name, int score)> rankingList = new List<(string name, int score)>();
    string filePath;

    ScoreEvaluation scoreEvaluation;
    void Start()
    {
        scoreEvaluation = GetComponent<ScoreEvaluation>();
        score = scoreEvaluation.ResltScore;
        /// �����L���O�̎菇
        //StreamingAssets�t�H���_��CSV�t�@�C���̃p�X���擾
        filePath = Path.Combine(Application.streamingAssetsPath, "ScoreRanking.csv");
        /// �f�[�^�̓ǂݍ��� 
        LoadRanking();
        /// ���݂̃f�[�^�Ɠǂݍ��񂾃f�[�^���r����top10�ɓ��邩���m�F����
        if (IsHighScore(score))
        {
            //���O���͂�L����
            nameInputField.gameObject.SetActive(true);
            subitButton.gameObject.SetActive(true);

            //�{�^���ɃC�x���g�̓o�^
            subitButton.onClick.AddListener(OnSubmitName);
        }

    }


    void OnSubmitName()
    {
        string playerName = nameInputField.text.Trim();
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogWarning("���O�����͂���Ă��܂���");
            return;
        }
        /// ���O�̓���
        InsertScore(playerName, score);
        SaveRanking();
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
}
