using NUnit.Framework;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;


public class DisplayScores : MonoBehaviour
{
    [SerializeField] PointList pointlist;
    [SerializeField] Text NumberEyes;//�G�l�~�[�̖ڂ̐�
    [SerializeField] Text GostType;//�G�l�~�[�̎��
    [SerializeField] Text Rarity;//���A�x
    [SerializeField] Text BonusPoints;//�{�[�i�X�|�C���g
    [SerializeField] Text AddScore;//�݌vPoint
    [SerializeField]GameObject cameraMask;//�}�X�N�S�̂̃I�u�W�F�N�g
    Transform  PhotoObject;//�ʐ^�̃I�u�W�F�N�g

    [SerializeField] float WholePhotoTime = 1.5f;//������ʐ^�̕\������
    [SerializeField] float FocusedPhoto = 1.0f;//�s���g���ʐ^�̕\������
    [SerializeField] float Information = 1.0f;//���_�̏ڍא���



    public List<GameObject> PhotoList = new List<GameObject>();
    
    public string nextSceneName = "RankingScene";//���ړ���̃V�[����
    
    
    Vector3 MaskPosition = new Vector3((float)-4.5,1,1);
    Vector3 MaskScale = new Vector3((float)0.2,(float)0.2,1);
    int Eyse;
    int Coward;
    int Furious;
    int Raritys;
    int BonusPointss;
    int Scores;
    private void Awake()
    {
        cameraMask = GameObject.Find("PhotoStorage").gameObject;
        PhotoObject = cameraMask.transform;
        SceneManager.MoveGameObjectToScene(cameraMask, SceneManager.GetActiveScene());
        cameraMask.transform.position = MaskPosition;
        cameraMask.transform.localScale = MaskScale;
    }
    void Start()
    {
        PhotoList.Clear();
        for(int i = 0;i < PhotoObject.childCount;i++)
        {
            Transform child = PhotoObject.GetChild(i);
            PhotoList.Add(child.gameObject);
        }
    }

    void Update()
    {
        ProcessObjects();
        var enemies = pointlist.point;
        Eyse = enemies.Sum(e => e.eyes);
        Raritys = enemies.Sum(e => e.rarity);
        BonusPointss = enemies.Sum(e => e.bonus);
        Scores = Eyse+Raritys+BonusPointss;
        EnemyDataUpdate();
    }




    void EnemyDataUpdate()
    {
        NumberEyes.text = $"{Eyse}��";
        GostType.text = $"{Coward}�́@{Furious}��";
        Rarity.text = $"{Raritys}";
        BonusPoints.text = $"{BonusPointss}";
        AddScore.text = $"{Scores}";
    }

    IEnumerator ProcessObjects()
    {
        foreach (GameObject obj in PhotoList)
        {
            Debug.Log("�\���J�n");
            yield return StartCoroutine(FlashObject(obj,1));
            Destroy(obj);
        }
        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator FlashObject(GameObject obj, int flashCount)
    {
        for (int i = 0; i < flashCount; i++)
        {
            yield return new WaitForSeconds(WholePhotoTime);
            obj.SetActive(true);
            yield return new WaitForSeconds(FocusedPhoto);
            yield return new WaitForSeconds(Information);
        }

    }
}
