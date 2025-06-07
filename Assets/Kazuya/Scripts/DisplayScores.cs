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
    [SerializeField] Text NumberEyes;//エネミーの目の数
    [SerializeField] Text GostType;//エネミーの種類
    [SerializeField] Text Rarity;//レア度
    [SerializeField] Text BonusPoints;//ボーナスポイント
    [SerializeField] Text AddScore;//累計Point
    [SerializeField]GameObject cameraMask;//マスク全体のオブジェクト
    Transform  PhotoObject;//写真のオブジェクト

    [SerializeField] float WholePhotoTime = 1.5f;//取った写真の表示時間
    [SerializeField] float FocusedPhoto = 1.0f;//ピント内写真の表示時間
    [SerializeField] float Information = 1.0f;//得点の詳細説明



    public List<GameObject> PhotoList = new List<GameObject>();
    
    public string nextSceneName = "RankingScene";//←移動先のシーン名
    
    
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
        NumberEyes.text = $"{Eyse}つ";
        GostType.text = $"{Coward}体　{Furious}体";
        Rarity.text = $"{Raritys}";
        BonusPoints.text = $"{BonusPointss}";
        AddScore.text = $"{Scores}";
    }

    IEnumerator ProcessObjects()
    {
        foreach (GameObject obj in PhotoList)
        {
            Debug.Log("表示開始");
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
