using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class ScoreRanking : MonoBehaviour
{
    [SerializeField] 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        //StreamingAssets�t�H���_��CSV�t�@�C���̃p�X���擾
        string filePath = Path.Combine(Application.streamingAssetsPath, "ScoreRanking.csv");
        
        //�t�@�C���̓ǂݍ���    
        if(File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach(string line in lines)
            {
                string[] values = line.Split(',');
            }
        }
        else
        {
            Debug.LogError("CSV�t�@�C����������܂���:"+filePath);
        }



        ///
        /// �����L���O�̎菇
        /// �f�[�^�̓ǂݍ���
        /// ���݂̃f�[�^�Ɠǂݍ��񂾃f�[�^���r����top10�ɓ��邩���m�F����
        /// ���O�̓���
        /// �����L���O�̕ϓ�
        /// �ϓ����������L���O��CSV�t�@�C���ɕۑ�
        ///


    }
    void Update()
    {
        
    }
}
