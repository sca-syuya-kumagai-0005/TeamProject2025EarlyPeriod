using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;
using TMPro;
using UnityEngine.Experimental.GlobalIllumination;


public class DisplayScores : MonoBehaviour
{
    [Header("UI表示テキスト")]
    [SerializeField] Text NumberEyes;//エネミーの目の数
    [SerializeField] Text GostType;//エネミーの種類
    [SerializeField] Text Rarity;//レア度
    [SerializeField] Text BonusPoints;//ボーナスポイント
    [SerializeField] Text AddScore;//累計Point
    [SerializeField]GameObject cameraMask;//マスク全体のオブジェクト
    Transform  PhotoObject;//写真のオブジェクト
    
    [Header("写真の表示の設定")]
    [SerializeField] float WholePhotoTime = 1.5f;//取った写真の表示時間
    [SerializeField] float FocusedPhoto = 1.0f;//ピント内写真の表示時間
    [SerializeField] float Information = 1.0f;//得点の詳細説明



    public List<GameObject> PhotoList = new List<GameObject>();
    [Header("シーン移行")]
    public string nextSceneName = "RankingScene";//←移動先のシーン名

    private bool skipRequested = false;
    private bool fastForwardRequested = false;

    [Header("早送りするときの倍速度")]
    [SerializeField]float acceleration = 0.3f;


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
        //シーンをまたいだオバケを取得
        cameraMask = GameObject.Find("PhotoStorage").gameObject;
        if(cameraMask != null )
        {
            PhotoObject = cameraMask.transform;
            SceneManager.MoveGameObjectToScene(cameraMask, SceneManager.GetActiveScene());
            cameraMask.transform.position = MaskPosition;
            cameraMask.transform.localScale = MaskScale;
        }
        else
        {
            Debug.Log("指定されたオブジェクトが見つかりません");
        }
    }
    void Start()
    {
        if (PhotoObject == null) return;
            //写真をリストを初期化し、PhotoStorageの子要素(撮影した写真)をリストに追加
            PhotoList.Clear();
            for (int i = 0; i < PhotoObject.childCount; i++)
            {
                Transform child = PhotoObject.GetChild(i);
                PhotoList.Add(child.gameObject);
            }

        //スコアを計算してUIを更新
        CalculateAndDisplayScores();
        //写真を順番に表示するコルーチンを開始
        StartCoroutine(ProcessPhotos());

        //ProcessObjects();
    }

    void Update()
    {
        //ユーザー入力の受付
         HandleUserInput();
    }

    /*後で消す
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

    }*/
    //コルーチン

    //写真を順番に処理するメインのコルーチン
    IEnumerator ProcessPhotos()
    {
        //写真リスト内の各オブジェクトに対して処理を繰り返す
        foreach(GameObject photo in PhotoList)
        {
            //次の写真のためのスキップ要求をリセット
            skipRequested = false;

            //写真を1枚表示するシーケンスコルーチン
            yield return StartCoroutine(PhotoDisplay(photo));

            //表示の終了orスキップされた 写真は破棄
            if(photo != null )Destroy(photo);
        }
        //すべての写真の処理が終わったら、次のシーンに遷移する
       // SceneManager.LoadScene(nextSceneName);
    }

    //写真1枚の表示部分
    IEnumerator PhotoDisplay(GameObject photo)
    {
        photo.SetActive(true);

        //写真全体の表示
        //スキップが適用されたら即終了へ
        if(skipRequested)yield break;
        //指定された時間だけ待機(早送りを考慮)
        yield return new WaitForSeconds(GetInterval(WholePhotoTime));

        //ピンとない写真表示
        if(skipRequested ) yield break;
        yield return new WaitForSeconds(GetInterval(FocusedPhoto));

        //得点詳細の表示
        if(skipRequested ) yield break;
        yield return new WaitForSeconds(GetInterval(Information));
    }

    // スコアを集計し、UIテキストを更新する
    void CalculateAndDisplayScores()
    {
        //if (PhotoList == null || PhotoList.point == null)
        //{
        //    Debug.LogError("PointListが設定されていません。");
        //    return;
        //}

        //var enemies = PhotoList.point;
        //int eyesTotal = enemies.Sum(e => e.eyes);
        //int rarityTotal = enemies.Sum(e => e.rarity);
        //int bonusTotal = enemies.Sum(e => e.bonus);
        //int scoreTotal = eyesTotal + rarityTotal + bonusTotal;

        //// UIテキストに計算結果を反映
        //NumberEyes.text = $"{eyesTotal}つ";
        //// ToDo: Coward, Furiousのカウント方法は元スクリプトで未定義のため、一旦0で表示
        //GostType.text = $"0体　0体";
        //Rarity.text = $"{rarityTotal}";
        //BonusPoints.text = $"{bonusTotal}";
        //AddScore.text = $"{scoreTotal}";
    }


    //ユーザー入力処理
    void HandleUserInput()
    {
        //スキップ処理
        if (Input.GetKeyDown(KeyCode.S))
        {
            skipRequested = true;
            Debug.Log("スキップ処理");
        }
        //早送り処理
        fastForwardRequested = Input.GetKey(KeyCode.F);
    }

    //早送り状態を考慮した、待機時間
    float GetInterval(float baseInterval)
    {
        //早送り要求があれば基本時間のn倍、なければ基本時間をそのまま返す
        return fastForwardRequested ? baseInterval* acceleration : baseInterval;
    }
}
