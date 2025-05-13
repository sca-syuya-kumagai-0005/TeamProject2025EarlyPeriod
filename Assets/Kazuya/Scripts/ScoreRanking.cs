using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class ScoreRanking : MonoBehaviour
{
    [SerializeField] int score;//���݂̃v���C���[�̃X�R�A

    List<(string name,int score)> rankingList = new List<(string name,int score)> ();
    string filePath;
    int[] Scores;
    [SerializeField]string Playername;//���O�̓���

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
        ///


    }
    void Update()
    {
        if (IsHighScore(score))
        {
            /// ���O�̓���
            InsertScore(name, score);
            Playername = scoreEvaluation.PlayerName;
            if(Playername != null)
            {
                SaveRanking();
            }

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
    //Top10�ɓ��邩�ǂ����̔���
    bool IsHighScore(int newScore)
    {
        if(rankingList.Count<10)return true;
        return newScore > rankingList.Min(entry =>entry.score);
    }
    /// �����L���O�̕ϓ�
    //�X�R�A�������L���O�ɒǉ����ĕ��ёւ�
    void InsertScore(string name,int nreScore)
    {
        rankingList.Add((name,nreScore));
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
        foreach(var entry in rankingList)
        {
            lines.Add($"{entry.name},{entry.score}");
        }
        File.WriteAllLines(filePath,lines);
        Debug.Log("�����L���O�̕ۑ�");
    }
}
