using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class SoundPlayer : SoundManager
{
    protected void SEPlayer(string name, bool loop)
    {
       

        AudioClip clip = SetSound(name,sePath);
        Debug.Log(clip);


        if (clip == null)
        {
            string[] color = new string[4] { "cyan", "yellow", "lime", "fuchsia" };
            string check = this.gameObject.name + "‚ÅŒÄ‚Î‚ê‚Ä‚¢‚éSEPlayer‚É‘Î‰‚·‚éSE‚ª‘ã“ü‚³‚ê‚Ä‚¢‚Ü‚¹‚ñB";
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
}
