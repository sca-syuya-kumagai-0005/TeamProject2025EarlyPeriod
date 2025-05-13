using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class ScoreRanking : MonoBehaviour
{
    [SerializeField] int score;//現在のプレイヤーのスコア
    [SerializeField] InputField nameInputField;//名前入力のUI
    [SerializeField] Button subitButton;//名前送信ボタン


    List<(string name, int score)> rankingList = new List<(string name, int score)>();
    string filePath;

    ScoreEvaluation scoreEvaluation;
    void Start()
    {
        scoreEvaluation = GetComponent<ScoreEvaluation>();
        score = scoreEvaluation.ResltScore;
        /// ランキングの手順
        //StreamingAssetsフォルダのCSVファイルのパスを取得
        filePath = Path.Combine(Application.streamingAssetsPath, "ScoreRanking.csv");
        /// データの読み込み 
        LoadRanking();
        /// 現在のデータと読み込んだデータを比較してtop10に入るかを確認する
        if (IsHighScore(score))
        {
            //名前入力を有効か
            nameInputField.gameObject.SetActive(true);
            subitButton.gameObject.SetActive(true);

            //ボタンにイベントの登録
            subitButton.onClick.AddListener(OnSubmitName);
        }

    }
    void Update()
    {

    }


    void OnSubmitName()
    {
        string playerName = nameInputField.text.Trim();
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogWarning("名前が入力されていません");
            return;
        }
        /// 名前の入力
        InsertScore(playerName, score);
        SaveRanking();
    } 

    /// <summary>
    /// ランキングの読み込み
    /// </summary>
    void LoadRanking()
    {

        //ファイルの読み込み    
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
            Debug.LogWarning("ランキングファイルが見つかりません" + filePath);
        }
    }
    //Top10に入るかどうかの判定
    bool IsHighScore(int newScore)
    {
        if(rankingList.Count<10)return true;
        return newScore > rankingList.Min(entry =>entry.score);
    }
    /// ランキングの変動
    //スコアをランキングに追加して並び替え
    void InsertScore(string name,int nreScore)
    {
        rankingList.Add((name,nreScore));
        //スコアを並び替えて上位を保持
        rankingList = rankingList
        .OrderByDescending(entry => entry.score)
        .Take(10)
        .ToList();

        Debug.Log("新しいスコアをランキングに追加");
        for(int i = 0;i < rankingList.Count; i++)
        {
            Debug.Log($"{i + 1}位:{rankingList[i].name},{rankingList[i].score}");
        }
    }
    /// 変動したランキングをCSVファイルに保存
    void SaveRanking()
    {
        List<string> lines = new List<string>();
        foreach(var entry in rankingList)
        {
            lines.Add($"{entry.name},{entry.score}");
        }
        File.WriteAllLines(filePath,lines);
        Debug.Log("ランキングの保存");
    }
}
