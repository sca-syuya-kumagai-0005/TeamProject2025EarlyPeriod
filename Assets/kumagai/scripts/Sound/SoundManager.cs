using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

public class SoundManager : MonoBehaviour
{
    const string frontPath = "Assets/Resources";
    const string bgmPath = "/BGM/";
    const string soundExtension = ".mp3";
    const string bgmPlayerName = "BGMPlayer";
    string[] bgmNames = Directory.GetFiles(frontPath + bgmPath);
    [SerializeField] List<string> soundName = new List<string>();
    AudioClip clip;
    [SerializeField] AudioSource bgmPlayer;

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
        foreach (string bgmName in bgmNames)
        {
            if (bgmName.Substring(name.Length - 4) == soundExtension)
            {
                soundName.Add(name);
            }
        }

        if (name.Substring(name.Length - 4) == soundExtension)
        {
            Debug.LogError("�֐�SetBGM�̈���name��.mp3��K�v�Ƃ��܂���");
        }
        if (Resources.Load<AudioClip>(fileName + name) == null)//�t�@�C�����Ɏw�肵��mp3�t�@�C�����Ȃ����
        {
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
        float clearLine = 0.6f;
        if (soundName.Count == 0) { return str; }
        for (int i = 0; i < soundName.Count; i++)
        {
            string sound = soundName[i].Substring((frontPath + bgmPath).Length, soundName[i].Length - (frontPath + fileName).Length - soundExtension.Length);//soundName�ɑ������ƁAsoundName�̗v�f�S�Ăɑ������Ă��܂����߉��p�̕ϐ����쐬
            if (name.Substring(name.Length - soundExtension.Length) == soundExtension)
            {
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
        soundName=new List<string>();
        return str;

    }
}
