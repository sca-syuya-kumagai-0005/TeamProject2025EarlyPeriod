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
    /// BGM��ݒ肷��֐�
    /// </summary>
    /// <param name="name">Asset�̒����猟������t�@�C���̖��O</param>
    protected void SetBGM(string name)
    {
        
        if(name.Substring(name.Length-4)==soundExtension)
        {
            Debug.LogError("�֐�SetBGM�̈���name��.mp3��K�v�Ƃ��܂���");
        }
        if (AssetDatabase.LoadAssetAtPath<AudioClip>(frontPath + bgmPath + name + soundExtension)==null) 
        {
            string str = CheckName(name);
            Debug.LogError($"�w�肳�ꂽAudioClip{name}���������܂���ł����B���O�̈�v�x������{str}���Đ����Ă��܂��B����name���m�F���Ă�������");
            name = str;       
        }
        BGMPlayer(name);
    }

    /// <summary>
    /// �����œn����name�ƈ�v�x�������t�@�C������Ԃ��֐�
    /// </summary>
    /// <param name="name">�����������t�@�C����</param>
    /// <returns></returns>
    private string CheckName(string name)
    {
        string str="";//�ŏI�I�ɕԂ�������
        float maxRatio=0.0f;
        float clearLine = 0.6f;
        if(soundName.Count == 0) { return str; }
        for(int i=0;i<soundName.Count;i++)
        {
            Debug.Log(soundName[i].Length);
            string sound = soundName[i].Substring((frontPath + bgmPath).Length, soundName[i].Length-(frontPath + bgmPath).Length-soundExtension.Length);//soundName�ɑ������ƁAsoundName�̗v�f�S�Ăɑ������Ă��܂����߉��p�̕ϐ����쐬
            if (name.Substring(name.Length - soundExtension.Length) == soundExtension)
            {
                name = name.Substring(0, name.Length - soundExtension.Length);
                Debug.Log(name);
            }
            float ratio = 0;//��v�x
            float hit = 0;//������̈�v�� 
            for (int j = 0; j < name.Length;j++)
            {
                if (sound.Length <= j)
                {
                    break;
                }
                if (sound.Contains(name[j]))//i�Ԗڂ�mp3�t�@�C���̖��O��name��j�Ԗڂ̕������܂܂�Ă����hit�����Z
                {
                    hit++;
                    Debug.Log($"{j}�Ԗڂ̕����񂪈�v���܂����B�t�@�C���ԍ���{i}�ł�");
                    if (sound[j] == name[j])
                    {
                        hit++;//�ʒu�܂ň�v���Ă�����hit���X�ɉ��Z
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
