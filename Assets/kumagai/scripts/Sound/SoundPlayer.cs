using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class SoundPlayer : SoundManager
{
    protected void SEPlayer(AudioClip clip, bool loop)
    {
        if (clip == null)
        {
            string[] color = new string[4] { "cyan", "yellow", "lime", "fuchsia" };
            string name = this.gameObject.name + "�ŌĂ΂�Ă���SEPlayer�ɑΉ�����SE���������Ă��܂���B";
            string output = null;
            for (int i = 0; i < name.Length; i++)
            {
                output += $"<color={color[i % color.Length]}>{name[i]}</color>";
            }
            Debug.LogError(output);
            return;
        }
        //GameObject obj = Instantiate(seAudioSource);
        //AudioSource se = obj.GetComponent<AudioSource>();

        //se.clip = clip;
        //se.loop = loop;

        //se.Stop();
        //se.Play();
        //if (!loop)
        //{
        //    StartCoroutine(DestroySE(obj, clip.length));
        //}

    }
}
