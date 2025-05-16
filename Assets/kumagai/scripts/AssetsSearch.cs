using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class AssetsSearch : MonoBehaviour
{
    const string frontPath = "Assets/kumagai/Sound/";
    const string bgmPath = "BGM/";
    const string soundExtension = ".mp3";
    const string bgmPlayerName = "BGMPlayer";
    [SerializeField]List<string> soundName=new List<string>();
    AudioClip clip;
    [SerializeField]AudioSource bgmPlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bgmPlayer = GameObject.Find(bgmPlayerName).gameObject.GetComponent<AudioSource>();
        string[] bgmNames=Directory.GetFiles(frontPath+bgmPath);
        foreach(string name in bgmNames)
        {
            if (name.Substring(name.Length - 4) == soundExtension)
            {
                soundName.Add(name);
            }
        }
        SetBGM("Test.mp3");
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    private void BGMPlayer(string name)
    {
        clip = AssetDatabase.LoadAssetAtPath<AudioClip>(frontPath + bgmPath + name + soundExtension);
        
    }
    /// <summary>
    /// BGMを設定する関数
    /// </summary>
    /// <param name="name">Assetの中から検索するファイルの名前</param>
    protected void SetBGM(string name)
    {
        
        if(name.Substring(name.Length-4)==soundExtension)
        {
            Debug.LogError("関数SetBGMの引数nameは.mp3を必要としません");
        }
        if (AssetDatabase.LoadAssetAtPath<AudioClip>(frontPath + bgmPath + name + soundExtension)==null) 
        {
            string str = CheckName(name);
            Debug.LogError($"指定されたAudioClip{name}が見つけられませんでした。名前の一致度が高い{str}を再生しています。引数nameを確認してください");
            name = str;       
        }
        BGMPlayer(name);
    }

    /// <summary>
    /// 引数で渡したnameと一致度が高いファイル名を返す関数
    /// </summary>
    /// <param name="name">検索したいファイル名</param>
    /// <returns></returns>
    private string CheckName(string name)
    {
        string str="";//最終的に返す文字列
        float maxRatio=0.0f;
        float clearLine = 0.6f;
        if(soundName.Count == 0) { return str; }
        for(int i=0;i<soundName.Count;i++)
        {
            Debug.Log(soundName[i].Length);
            string sound = soundName[i].Substring((frontPath + bgmPath).Length, soundName[i].Length-(frontPath + bgmPath).Length-soundExtension.Length);//soundNameに代入すると、soundNameの要素全てに代入されてしまうため回避用の変数を作成
            if (name.Substring(name.Length - soundExtension.Length) == soundExtension)
            {
                name = name.Substring(0, name.Length - soundExtension.Length);
                Debug.Log(name);
            }
            float ratio = 0;//一致度
            float hit = 0;//文字列の一致数 
            for (int j = 0; j < name.Length;j++)
            {
                if (sound.Length <= j)
                {
                    break;
                }
                if (sound.Contains(name[j]))//i番目のmp3ファイルの名前にnameのj番目の文字が含まれていればhitを加算
                {
                    hit++;
                    Debug.Log($"{j}番目の文字列が一致しました。ファイル番号は{i}です");
                    if (sound[j] == name[j])
                    {
                        hit++;//位置まで一致していたらhitを更に加算
                    }
                }
            }
            if(hit>0)
            {
                ratio = hit / name.Length;
            }

            if(ratio>clearLine)
            {
                if (ratio > maxRatio)
                {
                    str = sound;
                    maxRatio = ratio;
                }
            }
           
        }
        return str;
        
    }
}
