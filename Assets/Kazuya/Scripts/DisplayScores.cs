using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;


public class DisplayScores : MonoBehaviour
{
    [Header("スコアデータ")]
    [SerializeField] PointList pointlist;

    [Header("UI表示テキスト")]
    [SerializeField] Text NumberEyes;//エネミーの目の数
    [SerializeField] Text GostType;//エネミーの種類
    [SerializeField] Text Rarity;//レア度
    [SerializeField] Text BonusPoints;//ボーナスポイント
    [SerializeField] Text AddScore;//累計Point
    
    [Header("写真の表示の設定")]
    [SerializeField] float WholePhotoTime = 1.5f;//取った写真の表示時間
    [SerializeField] float FocusedPhoto = 1.0f;//ピント内写真の表示時間
    [SerializeField] float Information = 1.0f;//得点の詳細説明

    [SerializeField] float cropAreaWidth = 0.2f;//切り取る領域の幅
    [SerializeField] float cropAreaHeight = 0.2f;//切り取る高さ
    [Header("シーン移行")]
    public string nextSceneName = "RankingScene";//←移動先のシーン名

    public List<GameObject> PhotoList = new List<GameObject>();//写真を格納するリスト
    public List<GameObject> croppedPhoto = new List<GameObject>();
    [SerializeField] GameObject cameraMask;//マスク全体のオブジェクト
    Transform PhotoObject;//写真のオブジェクト

    //早送り/スキップのフラグ
    private bool skipRequested = false;
    private bool fastForwardRequested = false;

    [Header("早送りするときの倍速度")]
    [SerializeField]float acceleration = 0.3f;

    [Header("写真を移動するときの座標")]
    [SerializeField]
    Vector3 MaskPosition = new Vector3((float)-4.5,1,1);
    [SerializeField]
    Vector3 MaskScale = new Vector3((float)0.2,(float)0.2,1);


    private int CumulativeScore = 0;//累計スコア

    private void Awake()
    {
        //シーンをまたいだオバケを取得
        cameraMask = GameObject.Find("PhotoStorage");
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
        //UIの表示リセット
        ResetScoreUI();
            //写真をリストを初期化し、PhotoStorageの子要素(撮影した写真)をリストに追加
            PhotoList.Clear();
            for (int i = 0; i < PhotoObject.childCount; i++)
            {
                PhotoList.Add(PhotoObject.GetChild(i).gameObject);
            }
        //写真を順番に表示するコルーチンを開始
        StartCoroutine(ProcessPhotos());
    }

    void Update()
    {
        //ユーザー入力の受付
         HandleUserInput();
    }
    //コルーチン

    //写真を順番に処理するメインのコルーチン
    IEnumerator ProcessPhotos()
    {
        //写真リスト内の各オブジェクトに対して処理を繰り返す
        for(int i = 0;i < PhotoList.Count;i++)
        {
            Debug.Log(PhotoList.Count);
            //リストの範囲外になら終了させる
            if (i >= pointlist.point.Count)
            {
                break;
            }
            GameObject currentPhoto = PhotoList[i];
            var currentScoreData = pointlist.point[i];

            UpdataScores(currentScoreData);

            //写真の得点を累計に追加
            int photoScore = currentScoreData.eyes + currentScoreData.rarity + currentScoreData.bonus;
            CumulativeScore += photoScore;

            AddScore.text = $"{CumulativeScore}";
            //次の写真のためのスキップ要求をリセット
            skipRequested = false;

            //写真を1枚表示するシーケンスコルーチン
            yield return StartCoroutine(PhotoDisplay(currentPhoto));

            if (currentPhoto != null)
            {
                Destroy(currentPhoto);
            }
            Debug.Log(i+1 + "枚目終了");
        }
        //表示の終了orスキップされた 写真は破棄
        yield return new WaitForSeconds(1.0f);


        //すべての写真の処理が終わったら、次のシーンに遷移する
         //SceneManager.LoadScene(nextSceneName);
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

        //--切り取り機能追加
        if (!skipRequested)
        {
            
            List<GameObject> enemiesInPhoto =  FindDescendantWithTag(photo.transform, "Enemy");
            foreach(var enemy in enemiesInPhoto)
            {
                if(enemy != null)
                {
                    croppedPhoto.Add(enemy);
                }
            }
            yield return new WaitForSeconds(GetInterval(FocusedPhoto));
            foreach (var cropped in croppedPhoto)
            {
                if (cropped != null)
                {
                    StartCoroutine(CropAndDisplay(cropped));
                }
            }
        }

        if (skipRequested ) yield break;
        
        //得点詳細の表示
        if(skipRequested ) yield break;
        yield return new WaitForSeconds(GetInterval(Information));
        croppedPhoto.Clear();
    }

    //---切り取り機能
    public Transform croppedPhotoparent;
    public Vector3 croppedPhotoPosition = new Vector3(1.0f, 0.0f, 0.0f);//Inspectorから切り取った部分の表示位置

    IEnumerator CropAndDisplay(GameObject originalPhoto)
    {

        Texture originalTexture = null;
        Rect originalRect = new Rect(0, 0, 1, 1);
        Vector2 TextureSize = Vector2.zero;

        Renderer renderer = originalPhoto.GetComponent<Renderer>();
        RawImage rawImage = originalPhoto.GetComponent<RawImage>();

        if (renderer != null && renderer.material != null && renderer.material.mainTexture != null)
        {
            originalTexture = renderer.material.mainTexture;
            if (renderer is SpriteRenderer spriteRenderer && spriteRenderer.sprite != null)
            {
                originalRect = spriteRenderer.sprite.textureRect;
                TextureSize = new Vector2(spriteRenderer.sprite.texture.width, spriteRenderer.sprite.texture.height);
            }
        }
        else if (rawImage != null && rawImage.texture != null)
        {
            originalTexture = rawImage.texture;
            originalRect = rawImage.uvRect;
            TextureSize = new Vector2(rawImage.texture.width, rawImage.texture.height);
        }
        if (originalTexture != null)
        {
            Debug.Log("テクスチャの取得に成功。");
            GameObject croppedObject = new GameObject("CroppedPhoto_" + originalPhoto.name);
            RawImage croppedRawImage = croppedObject.AddComponent<RawImage>();
            croppedRawImage.texture = originalTexture;

            // 切り取る領域の中心位置 (正規化)
            float cropCenterX = 0.5f;
            float cropCenterY = 0.5f;

            // 切り取る領域のUV座標
            Rect cropRect = new Rect(
                originalRect.x + cropCenterX * originalRect.width - (cropAreaWidth * 0.5f * originalRect.width),
                originalRect.y + cropCenterY * originalRect.height - (cropAreaHeight * 0.5f * originalRect.height),
                cropAreaWidth * originalRect.width,
                cropAreaHeight * originalRect.height
            );

            croppedRawImage.uvRect = cropRect;

            // 表示位置と親を設定
            if (croppedPhoto != null)
            {
                croppedObject.transform.SetParent(croppedPhotoparent, false);
                croppedObject.transform.localPosition = croppedPhotoPosition;
            }
            else
            {
                croppedObject.transform.position = originalPhoto.transform.position + croppedPhotoPosition;
            }

            croppedObject.transform.localScale = originalPhoto.transform.localScale; // スケールを合わせる

            croppedPhoto.Add(croppedObject); // リストに追加
        }
        else
        {
            Debug.LogError("エラー:写真オブジェクトから切り抜き可能なテクスチャが見つかりませんでした。");
        }
        yield return null;
    }

    //---孫オブジェクトの参照

    public List<GameObject> FindDescendantWithTag(Transform parent, string tag)
    {
        List<GameObject> foundObjects = new List<GameObject>();
        //paren以下のすべての子・孫以下のTransformを配列で取得
        Transform[] allChildren = parent.GetComponentsInChildren<Transform>(true);

        foreach(Transform child in allChildren)
        {
            //取得物の中の親は除外する
            if(child == parent)
            {
                continue;
            }
            if (child.CompareTag(tag))
            {
                foundObjects.Add(child.gameObject);
            }
        }
        //ループ終了時に見つからなければnull
        return null;
    }

    //スコアの初期化
    void ResetScoreUI()
    {
        NumberEyes.text = "_";
        GostType.text = "_";
        Rarity.text = "_";
        BonusPoints.text = "_";
        AddScore.text = "0";
    }



    // スコアを集計し、UIテキストを更新する
    void UpdataScores(EnemyData data)
    {
        if (PhotoList == null || pointlist.point == null)
        {
            Debug.LogError("PointListが設定されていません。");
            return;
        }

        // UIテキストに計算結果を反映
        NumberEyes.text = $"{data.eyes}つ";
        // ToDo: Coward, Furiousのカウント方法は元スクリプトで未定義のため、一旦0で表示
        GostType.text = $"0体　0体";
        Rarity.text = $"{data.rarity}";
        BonusPoints.text = $"{data.bonus}";
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
