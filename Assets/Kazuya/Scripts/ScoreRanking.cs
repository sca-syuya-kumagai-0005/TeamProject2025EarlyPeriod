using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public class ScoreRanking : MonoBehaviour
{
    [SerializeField] int score;//現在のプレイヤーのスコア
    [SerializeField] InputField nameInputField;//名前入力のUI
    [SerializeField] Button subitButton;//名前送信ボタン
    [SerializeField] Text[] rankingText = new Text[10];
    [SerializeField] GameObject TextObject;

    [Header("各種ボタン")]
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

        /// ランキングの手順
        //StreamingAssetsフォルダのCSVファイルのパスを取得
        filePath = Path.Combine(Application.streamingAssetsPath, "ScoreRanking.csv");
        /// データの読み込み 
        LoadRanking();
        /// 現在のデータと読み込んだデータを比較してtop10に入るかを確認する
        if (IsHighScore(score))
        {
            /*
            //名前入力を有効か
            nameInputField.gameObject.SetActive(true);
            subitButton.gameObject.SetActive(true);

            //ボタンにイベントの登録
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

    //csvファイルで持ってきたデータをテキストに置換
    void UpdateRankingDisplay()
    {
        TextObject.SetActive(true);
        for (int i = 0;i < rankingList.Count; i++)
        {
            if (i < rankingList.Count)
            {
                rankingText[i].text = $"{i + 1}位    :   {rankingList[i].name} :{rankingList[i].score}";

                // 1位から3位の色を変更
                if (i == 0)
                {
                    rankingText[i].color = Color.yellow; // 1位はゴールド
                }
                else if (i == 1)
                {
                    rankingText[i].color = Color.gray;   // 2位はシルバー
                }
                else if (i == 2)
                {
                    rankingText[i].color = new Color(0.8f, 0.5f, 0.2f); // 3位はブロンズっぽい色
                }
                else
                {
                    rankingText[i].color = Color.black; // その他の順位は通常の白
                }
            }
            else
            {
                rankingText[i].text = $"{i + 1}位; :";
                rankingText[i].color = Color.black;
            }
        }

    }

    //Top10に入るかどうかの判定
    bool IsHighScore(int newScore)
    {
        //ランキングが10未満ならスコアを追加する
        if(rankingList.Count<10)return true;
        //最低スコア(10位)より新しいスコアが高ければ追加する
        return newScore > rankingList.Last().score;
    }
    /// ランキングの変動
    //スコアをランキングに追加して並び替え
    void InsertScore(string name,int newScore)
    {
        rankingList.Add((name,newScore));
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
       lines.Add("名前,スコア");
        foreach (var entry in rankingList)
        {
            lines.Add($"{entry.name},{entry.score}");
        }
        File.WriteAllLines(filePath,lines,Encoding.UTF8);
        Debug.Log("ランキングの保存");
    }

    IEnumerator ButtonSetUp()
    {
        yield return new WaitForSeconds(4.0f);
        TextObject.SetActive(true);
        TitkeButton.SetActive(true);
        MainGameButton.SetActive(true);
    }
}
