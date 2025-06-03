using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

//����
//��x��v����������͎��񂩂�hit�����Z���Ȃ��悤�ɂ���
//�A�����ĕ�������v���Ă���hit�����Z

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
    /// BGM��ݒ肷��֐�
    /// </summary>
    /// <param name="name">Asset�̒����猟������t�@�C���̖��O</param>
    protected AudioClip SetSound(string name, string fileName)
    {
        string[] soundNames = Directory.GetFiles(frontPath + "/" + fileName, "*" + soundExtension);
        foreach (string soundName in soundNames)
        {
            this.soundName.Add(soundName);
        }

        if (Resources.Load<AudioClip>(fileName + name) == null)//�t�@�C�����Ɏw�肵��mp3�t�@�C�����Ȃ����
        {
            string str = CheckName(name, fileName);//��v�x�̍���mp3������
            if (str == "")//���ȏ�̈�v����mp3�t�@�C�����������Ȃ����
            {
                Debug.LogError($"�w�肳�ꂽmp3�t�@�C��{name}���������܂���ł����B�t�@�C�����A�ۑ��ꏊ���m�F���Ă�������");
                return null;
            }
            Debug.LogError($"�w�肳�ꂽmp3�t�@�C��{name}���������܂���ł����B���O�̈�v�x������{str}���Đ����܂��B�t�@�C�����A�ۑ��ꏊ���m�F���Ă�������");
            name = str;
        }
        clip = Resources.Load<AudioClip>(fileName + name);
        return clip;
    }
    /// <summary>
    /// �����œn����name�ƈ�v�x�������t�@�C������Ԃ��֐�
    /// </summary>
    /// <param name="name">�����������t�@�C����</param>
    /// <returns></returns>
    private string CheckName(string name, string fileName)
    {
        string str = "";//�ŏI�I�ɕԂ�������
        float maxRatio = 0.0f;
        float clearLine = 0.8f;
        float continuous = 0.0f;
        int charCount = 0;
        int maxCharCount = 0;
        if (soundName.Count == 0)
        {
            return str;
        }
        for (int i = 0; i < soundName.Count; i++)
        {
            string sound = soundName[i].Substring((frontPath + bgmPath).Length, soundName[i].Length - (frontPath + fileName).Length - soundExtension.Length - 1);//soundName�ɑ������ƁAsoundName�̗v�f�S�Ăɑ������Ă��܂����߉��p�̕ϐ����쐬
            if (name.Length < soundExtension.Length) { }
            else if (name.Substring(name.Length - soundExtension.Length) == soundExtension)
            {
                name = name.Substring(0, name.Length - soundExtension.Length);
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
