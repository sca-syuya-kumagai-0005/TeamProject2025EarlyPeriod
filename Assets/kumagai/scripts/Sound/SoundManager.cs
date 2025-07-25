using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

//メモ
//一度一致した文字列は次回からhitを加算しないようにする
//連続して文字が一致してたらhitを加算

public class SoundManager : MonoBehaviour
{
    const string frontPath = "Assets/Resources";
    protected const string bgmPath = "Sound/BGM/";
    protected const string sePath = "Sound/SE/";
    protected const string soundExtension = ".mp3";
    protected const string bgmAudioSource = "Sound/BGMPlayer";
    protected const string seAudioSource = "Sound/SEPlayer";
    List<int> isChecked = new List<int>();

    List<string> soundName = new List<string>();
    AudioClip clip;
    AudioSource bgmPlayer;

    protected IEnumerator DestroySE(GameObject se, float time)
    {
        yield return new WaitForSecondsRealtime(time + 1);
        Destroy(se);
    }

    /// <summary>
    /// BGMを設定する関数
    /// </summary>
    /// <param name="name">Assetの中から検索するファイルの名前</param>
    protected AudioClip SetSound(string name, string fileName)
    {
        //string[] soundNames = Directory.GetFiles(frontPath + "/" + fileName, "*" + soundExtension);
        //foreach (string soundName in soundNames)
        //{
        //    this.soundName.Add(soundName);
        //}

        //if (Resources.Load<AudioClip>(fileName + name) == null)//ファイル内に指定したmp3ファイルがなければ
        //{
        //    string str = CheckName(name, fileName);//一致度の高いmp3を検索
        //    if (str == "")//一定以上の一致するmp3ファイルを見つけられなければ
        //    {
        //        Debug.LogError($"指定されたmp3ファイル{name}を見つけられませんでした。ファイル名、保存場所を確認してください");
        //        return null;
        //    }
        //    Debug.LogError($"指定されたmp3ファイル{name}を見つけられませんでした。名前の一致度が高い{str}を再生します。ファイル名、保存場所を確認してください");
        //    name = str;
        //}
        clip = Resources.Load<AudioClip>(fileName + name);
        return clip;
    }
    /// <summary>
    /// 引数で渡したnameと一致度が高いファイル名を返す関数
    /// </summary>
    /// <param name="name">検索したいファイル名</param>
    /// <returns></returns>
    private string CheckName(string name, string fileName)
    {
        string str = "";//最終的に返す文字列
        float maxRatio = 0.0f;
        float clearLine = 0.0f;
        float continuous = 0.0f;
        int charCount = 0;
        int maxCharCount = 0;
        if (soundName.Count == 0)
        {
            return str;
        }
        for (int i = 0; i < soundName.Count; i++)
        {
            string sound = soundName[i].Substring((frontPath + bgmPath).Length, soundName[i].Length - (frontPath + fileName).Length - soundExtension.Length - 1);//soundNameに代入すると、soundNameの要素全てに代入されてしまうため回避用の変数を作成
            if (name.Length < soundExtension.Length) { }
            else if (name.Substring(name.Length - soundExtension.Length) == soundExtension)
            {
                name = name.Substring(0, name.Length - soundExtension.Length);
            }
            float ratio = 0;//一致度
            float hit = 0;//文字列の一致数 
            for (int j = 0; j < name.Length; j++)
            {
                if (sound.Length <= j)
                {
                    break;
                }
                if (sound.Contains(name[j]))//i番目のmp3ファイルの名前にnameのj番目の文字が含まれていればhitを加算
                {
                    hit++;
                    //Debug.Log($"{j}番目の文字列が一致しました。ファイル番号は{i}です");
                    if (sound[j] == name[j])
                    {
                        hit++;//位置まで一致していたらhitを更に加算
                        hit += continuous;
                        continuous += 0.5f;
                    }
                    else
                    {
                        continuous = 0.0f;
                    }
                }
            }
            if (hit > 0)
            {
                ratio = hit / name.Length;
            }

            if (ratio >= clearLine)
            {
                if (ratio > maxRatio)
                {
                    charCount = Mathf.Abs(name.Length - sound.Length);
                    str = sound;
                    maxRatio = ratio;
                    maxCharCount = charCount;
                }
                else if (ratio == maxRatio)
                {
                    charCount = Mathf.Abs(name.Length - sound.Length);
                    if (charCount < maxCharCount)
                    {
                        str = sound;
                        maxRatio = ratio;
                        maxCharCount = charCount;
                    }
                }
            }

        }
        soundName = new List<string>();
        return str;

    }
}
