
using UnityEngine;
public class SoundPlayer : SoundManager
{
    /// <summary>
    /// SEを再生する関数
    /// </summary>
    /// <param name="name">再生したいファイルの名前</param>
    /// <param name="loop">再生後ループするかどうか</param>
    protected void SEPlayer(string name, bool loop)
    {
     
        AudioClip clip = SetSound(name,sePath);


        //if (clip != null)//遊んだ痕跡　エラーログをカラフルに表示する
        //{
        //    string[] color = new string[4] { "cyan", "yellow", "lime", "fuchsia" };
        //    string check = this.gameObject.name + "で呼ばれているSEPlayerに対応するSEが代入されていません。";
        //    string output = null;
        //    for (int i = 0; i < check.Length; i++)
        //    {
        //        output += $"<color={color[i % color.Length]}>{check[i]}</color>";
        //    }
        //    Debug.LogError(output);
        //}
        GameObject seObj = Resources.Load<GameObject>(seAudioSource);//Resourcesフォルダーに入っているSE用のオーディオソースを取得
        GameObject obj = Instantiate(seObj);//取得したものを生成して、生成したオブジェクトを取得
        
        AudioSource se = obj.GetComponent<AudioSource>();//生成したオブジェクトからAudioSourceコンポーネントを取得

        se.clip = clip;//取得したAudioSourceに引数で渡したオーディオファイルを代入
        se.loop = loop;//ループするかどうかを設定

        se.Stop();
        se.Play();
        float length=se.clip.length;
        if (!loop)
        {
            StartCoroutine(DestroySE(obj, clip.length));
        }
    }

    protected void BGMPlayer(string name)
    {
        GameObject obj;
       
        if (GameObject.Find("BGMPlayer") == null)
        {
            obj=(GameObject)Instantiate(Resources.Load(bgmAudioSource));
        }
        else
        {
            obj = GameObject.Find("BGMPlayer").gameObject;
        }
        AudioClip clip = SetSound(name,bgmPath);
        AudioSource bgm=obj.GetComponent<AudioSource>();
        bgm.clip = clip;
        bgm.Stop();
        bgm.Play();
    }
}
