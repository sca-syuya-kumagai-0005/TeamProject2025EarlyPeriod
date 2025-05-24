using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using JetBrains.Annotations;
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
        Debug.Log(clip);
        if (clip == null)
        {
            string[] color = new string[4] { "cyan", "yellow", "lime", "fuchsia" };
            string check = this.gameObject.name + "で呼ばれているSEPlayerに対応するSEが代入されていません。";
            string output = null;
            for (int i = 0; i < name.Length; i++)
            {
                output += $"<color={color[i % color.Length]}>{name[i]}</color>";
            }
            Debug.LogError(output);
            return;
        }
        GameObject seObj = Resources.Load<GameObject>(seAudioSource);
        GameObject obj = Instantiate(seObj);
        
        AudioSource se = obj.GetComponent<AudioSource>();
        se.clip = clip;
        se.loop = loop;

        se.Stop();
        se.Play();
        if (!loop)
        {
            StartCoroutine(DestroySE(obj, clip.length));
        }
    }

    /// <summary>
    /// SEを再生する関数
    /// </summary>
    /// <param name="name"></param>
    /// <param name="loop"></param>
    /// <param name="length"></param>
    protected void SEPlayer(string name, bool loop,float length)
    {
        AudioClip clip = SetSound(name, sePath);
        Debug.Log(clip);


        if (clip == null)
        {
            string[] color = new string[4] { "cyan", "yellow", "lime", "fuchsia" };
            string check = this.gameObject.name + "で呼ばれているSEPlayerに対応するSEが代入されていません。";
            string output = null;
            for (int i = 0; i < name.Length; i++)
            {
                output += $"<color={color[i % color.Length]}>{name[i]}</color>";
            }
            Debug.LogError(output);
            return;
        }
        GameObject seObj = Resources.Load<GameObject>(seAudioSource);
        GameObject obj = Instantiate(seObj);

        AudioSource se = obj.GetComponent<AudioSource>();
        se.clip = clip;
        se.loop = loop;

        se.Stop();
        se.Play();
        StartCoroutine(DestroySE(obj, clip.length));
    }


}
