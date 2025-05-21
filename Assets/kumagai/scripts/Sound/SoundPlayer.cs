using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using JetBrains.Annotations;
public class SoundPlayer : SoundManager
{
    /// <summary>
    /// SE���Đ�����֐�
    /// </summary>
    /// <param name="name">�Đ��������t�@�C���̖��O</param>
    /// <param name="loop">�Đ��ニ�[�v���邩�ǂ���</param>
    protected void SEPlayer(string name, bool loop)
    {
        AudioClip clip = SetSound(name,sePath);
        Debug.Log(clip);
        if (clip == null)
        {
            string[] color = new string[4] { "cyan", "yellow", "lime", "fuchsia" };
            string check = this.gameObject.name + "�ŌĂ΂�Ă���SEPlayer�ɑΉ�����SE���������Ă��܂���B";
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
    /// SE���Đ�����֐�
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
            string check = this.gameObject.name + "�ŌĂ΂�Ă���SEPlayer�ɑΉ�����SE���������Ă��܂���B";
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
