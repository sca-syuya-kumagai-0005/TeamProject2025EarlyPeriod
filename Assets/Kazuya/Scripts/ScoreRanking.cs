using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class ScoreRanking : MonoBehaviour
{
    [SerializeField] int score;

    int[] Scores;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        //StreamingAssetsフォルダのCSVファイルのパスを取得
        string filePath = Path.Combine(Application.streamingAssetsPath, "ScoreRanking.csv");
        
        //ファイルの読み込み    
        if(File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach(string line in lines)
            {
                string[] values = line.Split(',');
                for (int i = 0; i < 10; i++)
                {
                    Scores[i] = int.Parse(values[1]);
                    Debug.Log(Scores[i]);
                }
            }



        }
        else
        {
            Debug.LogError("CSVファイルが見つかりません:"+filePath);
        }



        ///
        /// ランキングの手順
        /// データの読み込み 〇
        /// 現在のデータと読み込んだデータを比較してtop10に入るかを確認する
        /// 名前の入力
        /// ランキングの変動
        /// 変動したランキングをCSVファイルに保存
        ///


    }
    void Update()
    {
        
    }
}
