using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SoundManager : MonoBehaviour
{
    const string frontPath = "Assets/Resources";
    protected const string bgmPath = "Sound/BGM/";
    protected const string sePath="Sound/SE/";
    const string soundExtension = ".mp3";
    const string bgmPlayerName = "BGMPlayer";
    protected const string seAudioSource = "Sound/SEPlayer";

    List<string> soundName = new List<string>();
    AudioClip clip;
    AudioSource bgmPlayer;

    protected IEnumerator DestroySE(GameObject se, float time)
    {
        yield return new WaitForSecondsRealtime(time + 1);
        Destroy(se);
    }

    /// <summary>
    /// BGM��ݒ肷��֐�
    /// </summary>
    /// <param name="name">Asset�̒����猟������t�@�C���̖��O</param>
    protected AudioClip SetSound(string name,string fileName)
    {
        string[] soundNames = Directory.GetFiles(frontPath + "/" + fileName,"*"+soundExtension);
        Debug.Log(soundNames.Length);
        foreach (string soundName in soundNames)
        {
                this.soundName.Add(soundName);
        }
        if (Resources.Load<AudioClip>(fileName) == null)//�t�@�C�����Ɏw�肵��mp3�t�@�C�����Ȃ����
        {
            Debug.Log("null�������̂�");
            string str = CheckName(name,fileName);//��v�x�̍���mp3������
            if(str=="")//���ȏ�̈�v����mp3�t�@�C�����������Ȃ����
            {
                Debug.LogError($"�w�肳�ꂽmp3�t�@�C��{name}���������܂���ł����B�t�@�C�����A�ۑ��ꏊ���m�F���Ă�������");
                return null;
            }
            Debug.LogError($"�w�肳�ꂽmp3�t�@�C��{name}���������܂���ł����B���O�̈�v�x������{str}���Đ����܂��B�t�@�C�����A�ۑ��ꏊ���m�F���Ă�������");
            name = str;
        }
        clip= Resources.Load<AudioClip>(fileName + name);
        return clip;
    }

    protected void SetSE(string name)
    {
        
    }

    /// <summary>
    /// �����œn����name�ƈ�v�x�������t�@�C������Ԃ��֐�
    /// </summary>
    /// <param name="name">�����������t�@�C����</param>
    /// <returns></returns>
    private string CheckName(string name,string fileName)
    {
        string str = "";//�ŏI�I�ɕԂ�������
        float maxRatio = 0.0f;
        float clearLine = 0.0f;
        if (soundName.Count == 0)
            {
                return str; 
            }
        for (int i = 0; i < soundName.Count; i++)
        {
            string sound = soundName[i].Substring((frontPath + bgmPath).Length, soundName[i].Length - (frontPath + fileName).Length - soundExtension.Length-1);//soundName�ɑ������ƁAsoundName�̗v�f�S�Ăɑ������Ă��܂����߉��p�̕ϐ����쐬
            if (name.Length < soundExtension.Length) {  }
            else if (name.Substring(name.Length - soundExtension.Length) == soundExtension)
            {
                Debug.Log(name);
                name = name.Substring(0, name.Length - soundExtension.Length);
                Debug.Log(name);
            }
            float ratio = 0;//��v�x
            float hit = 0;//������̈�v�� 
            for (int j = 0; j < name.Length; j++)
            {
                if (sound.Length <= j)
                {
                    break;
                }
                if (sound.Contains(name[j]))//i�Ԗڂ�mp3�t�@�C���̖��O��name��j�Ԗڂ̕������܂܂�Ă����hit�����Z
                {
                    hit++;
                    //Debug.Log($"{j}�Ԗڂ̕����񂪈�v���܂����B�t�@�C���ԍ���{i}�ł�");
                    if (sound[j] == name[j])
                    {
                        hit++;//�ʒu�܂ň�v���Ă�����hit���X�ɉ��Z
                    }
                }
            }
            if (hit > 0)
            {
                ratio = hit / name.Length;
            }

            if (ratio > clearLine)
            {
                if (ratio > maxRatio)
                {
                    str = sound;
                    maxRatio = ratio;
                }
            }

        }
        Debug.Log(str);
        soundName=new List<string>();
        return str;

    }
}
