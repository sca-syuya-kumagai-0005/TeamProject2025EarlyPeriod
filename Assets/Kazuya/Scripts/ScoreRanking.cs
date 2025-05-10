using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class ScoreRanking : MonoBehaviour
{
    [SerializeField] 

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
                Debug.Log("Name:" + values[0] + "Score:" + values[1]);
            }
        }
        else
        {
            Debug.LogError("CSVファイルが見つかりません:"+filePath);
        }



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
