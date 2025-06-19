/*
    [Header("早送りするときの倍速度")]
    [SerializeField]float acceleration = 0.3f;

    [Header("UIの基準オブジェクト")]
    [SerializeField] GameObject photoDisplayReference;//写真を表示する位置と大きさの基準となるオブジェクト
    [SerializeField] GameObject displayArea;//複製したオバケを表示する範囲となるオブジェクト

    [Header("写真のオブジェクト類")]
    [SerializeField] GameObject cameraMask;//シーンをまたいできた写真の親オブジェクト
    Transform photoContainer;//写真のオブジェクト
    [SerializeField] List<GameObject> photoList = new List<GameObject>();//写真を格納するリスト
    [SerializeField] List<GameObject> clonedEnemies = new List<GameObject>();//複製するオバケのリスト

    private int cumulativeScore = 0;//累計スコア
                                    //早送り/スキップのフラグ
    private bool skipRequested = false;
    private bool fastForwardRequested = false;

    private void Awake()
    {
        //シーンをまたいだ親オブジェクトを取得
        cameraMask = GameObject.Find("PhotoStorage");
        if(cameraMask == null)
        {
            Debug.LogError("PhotoStorageが見つからないよ");
            return;
        }
        //基準となるオブジェクトが設定されているか
        if(photoDisplayReference == null)
        {
            Debug.LogError("基準となるオブジェクトが見つからないよ");
            return;
        }
        //写真オブジェクトを現在のシーンに移動
        SceneManager.MoveGameObjectToScene(cameraMask, SceneManager.GetActiveScene());
        photoContainer = cameraMask.transform;

        //表示先の基準オブジェクトを取得
        var targetzBounds = photoDisplayReference.GetComponent<Collider>().bounds;

        Renderer[] childRenderers = cameraMask.GetComponentsInChildren<Renderer>();
        if(childRenderers.Length == 0)
        {
            Debug.LogError("子オブジェクトに、Rendererがついてません");
            return;
        }

        //全ての子を囲うBoundsを計算
        Bounds totalBounds = childRenderers[0].bounds;
        for(int i = 1; i < childRenderers.Length; i++)
        {
            totalBounds.Encapsulate(childRenderers[i].bounds);
        }
        
        cameraMask.transform.position = targetzBounds.center;

        //基準となるオブジェクトのサイズと、計算した写真全体のサイズの比率比較
        float scaleX = targetzBounds.size.x / totalBounds.size.x;
        float scaleY = targetzBounds.size.y / totalBounds.size.y;

        float finalScaleRatio = Mathf.Min(scaleX, scaleY);

        //計算した比率を適用
        cameraMask.transform.localScale *= finalScaleRatio;
    }
    void Start()
    {
        if (photoContainer == null) 
        {
            Debug.LogError("写真オブジェクトが初期化されていません"); 
            return; 
        }
        //UIの表示リセット
        ResetScoreUI();
            //写真をリストを初期化し、PhotoStorageの子要素(撮影した写真)をリストに追加
            photoList.Clear();
            for (int i = 0; i < photoContainer.childCount; i++)
            {
                photoList.Add(photoContainer.GetChild(i).gameObject);
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
        for(int i = 0;i < photoList.Count;i++)
        {
            Debug.Log(photoList.Count);
            //リストの範囲外になら終了させる
            if (i >= pointlist.point.Count)
            {
                Debug.Log("リストの範囲外になりました。");
                break;
            }
            GameObject currentPhoto = photoList[i];
            var currentScoreData = pointlist.point[i];

            UpdateScores(currentScoreData);

            //写真の得点を累計に追加
            int photoScore = currentScoreData.eyes + currentScoreData.rarity + currentScoreData.bonus;
            cumulativeScore += photoScore;

            AddScore.text = $"{cumulativeScore}";
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
         SceneManager.LoadScene(nextSceneName);
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
            yield return new WaitForSeconds(GetInterval(FocusedPhoto));
            DuplicateAndMoveEnemies(photo);
        }

        if (skipRequested ) yield break;
        
        //得点詳細の表示
        if(skipRequested ) yield break;
        yield return new WaitForSeconds(GetInterval(Information));
        // 複製したEnemyをすべて削除
        foreach (GameObject enemy in clonedEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }
        clonedEnemies.Clear(); // リストもクリア
    }
    void DuplicateAndMoveEnemies(GameObject photo)
    {
        //表示先の範囲となるオブジェクトとそのColliderを取得
        if(displayArea == null)
        {
            Debug.LogError("対象となるオブジェクトが設定されていません");
            return;
        }
        var destCollider = displayArea.GetComponent<Collider>();
        if(destCollider ==  null )
        {
            Debug.LogError("コライダーが設定されていません");
            return;
        }

        //複製元の写真オブジェクトとそのコライダーの取得
        Renderer[] sourceCollider = photo.GetComponentsInChildren<Renderer>();
        if(sourceCollider == null)
        {
            Debug.LogError("写真にコライダーがついてないよ");
            return;
        }

        //それぞれのColliderから、ワールド空間での境界を取得
        Bounds sourceBounds = sourceCollider[0].bounds;
        for(int i = 1; i < sourceCollider.Length; i++)
        {
            sourceBounds.Encapsulate(sourceCollider[i].bounds);
        }

        var destBounds = destCollider.bounds;

        //写真内の"Enemy"タグを持つオブジェクトを全て探す
        foreach (Transform child in photo.transform)
        {
            if (child.CompareTag("Enemy"))
            {
                // オバケの複製
                GameObject clone = Instantiate(child.gameObject);

                //元の座標が、元の範囲のどのくらいの割合になるかの計算

                float relativeX = Mathf.InverseLerp(sourceBounds.min.x, sourceBounds.max.x, child.position.x);
                float relativeY = Mathf.InverseLerp(sourceBounds.min.y,sourceBounds.max.y, child.position.y);

                //新しい範囲内で座標を決定
                float newX = Mathf.Lerp(destBounds.min.x,destBounds.max.x, relativeX);
                float newY = Mathf.Lerp(destBounds.min.y,destBounds.max.y, relativeY);
                float newZ = destBounds.center.z;

                //計算した座標を複製したオバケの設定
                clone.transform.localPosition = new Vector3(newX, newY, newZ);
                //基準オブジェクトのサイズと現在のスケールの比率を計算
                float scaleX = destBounds.size.x / sourceBounds.size.x;
                float scaleY = destBounds.size.y / sourceBounds.size.y;

                // 縦横比を維持するため、比率の小さい方を採用
                float finalScaleRatio = Mathf.Min(scaleX, scaleY);
                //計算した比率を適用
            }
        }
    }




}*/

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

    [Header("シーン移行")]
    public string nextSceneName = "RankingScene";//←移動先のシーン名

    [Header("UIの基準オブジェクト")]
    [SerializeField]  GameObject photoDisplayReference; // 写真を表示する位置と大きさの基準となるオブジェクト
    [SerializeField]  GameObject displayArea; // 複製したオバケを表示する範囲となるオブジェクト

    [Header("早送りするときの倍速度")]
    [SerializeField] float acceleration = 0.3f;

     [SerializeField]GameObject cameraMask; // シーンをまたいで運ばれてきた写真の親オブジェクト("PhotoStorage")
     Transform photoContainer; // 写真オブジェクトのTransform
    [SerializeField] List<GameObject> photoList = new List<GameObject>(); // 写真を格納するリスト
    [SerializeField] List<GameObject> clonedEnemies = new List<GameObject>(); // 複製したオバケのリスト

     int cumulativeScore = 0; // 累計スコア
    //早送り/スキップのフラグ
     bool skipRequested = false;
     bool fastForwardRequested = false;

    private void Awake()
    {

    }

    void Start()
    {
        // シーンをまたいだ写真の親オブジェクトを取得
        cameraMask = GameObject.Find("PhotoStorage");
        if (cameraMask == null)
        {
            Debug.LogError("PhotoStorageオブジェクトが見つかりません。");
            return;
        }

        // 表示位置の基準となるオブジェクトが設定されているか確認
        if (photoDisplayReference == null)
        {
            Debug.LogError("基準オブジェクト(photoDisplayReference)が設定されていません。");
            return;
        }

        // 写真オブジェクトを現在のシーンに移動
        SceneManager.MoveGameObjectToScene(cameraMask, SceneManager.GetActiveScene());
        photoContainer = cameraMask.transform;

        // --- 写真全体の表示位置とスケールを自動調整 ---

        // 1. 表示先の基準オブジェクトのBoundsを取得
        var targetBounds = photoDisplayReference.GetComponent<Collider>().bounds;

        // 2. 写真の全子要素のRendererから、写真全体のBoundsを計算
        Renderer[] childRenderers = cameraMask.GetComponentsInChildren<Renderer>(true);
        Debug.Log(childRenderers.Length);
        if (childRenderers.Length == 0)
        {
            Debug.LogError("PhotoStorageの子オブジェクトに、表示するためのRendererコンポーネントが見つかりません。");
            return;
        }

        // 全ての子を囲うBoundsを計算（最初の子のBoundsで初期化し、残りを結合していく）
        Bounds totalSourceBounds = childRenderers[0].bounds;
        for (int i = 1; i < childRenderers.Length; i++)
        {
            totalSourceBounds.Encapsulate(childRenderers[i].bounds);
        }

        // 3. 写真の位置を基準オブジェクトの中央に設定
        cameraMask.transform.position = new Vector3(targetBounds.center.x, targetBounds.center.y, 70); //targetBounds.center;

        // 4. 基準オブジェクトのサイズと写真全体のサイズの比率を計算
        float scaleX = targetBounds.size.x / totalSourceBounds.size.x;
        float scaleY = targetBounds.size.y / totalSourceBounds.size.y;

        // 縦横比を維持するため、XとYの比率のうち小さい方を採用
        float finalScaleRatio = Mathf.Min(scaleX, scaleY);

        // 5. 計算した比率を現在のスケールに適用
        cameraMask.transform.localScale *= finalScaleRatio;
        if (photoContainer == null)
        {
            Debug.LogError("写真オブジェクトが初期化されていません。");
            return;
        }

        ResetScoreUI();

        // PhotoStorageの子要素（撮影した写真）をリストに追加
        photoList.Clear();
        for (int i = 0; i < photoContainer.childCount; i++)
        {
            photoList.Add(photoContainer.GetChild(i).gameObject);
        }

        StartCoroutine(ProcessPhotos());
    }

    void Update()
    {
        HandleUserInput();
    }

    IEnumerator ProcessPhotos()
    {
        for (int i = 0; i < photoList.Count; i++)
        {
            if (i >= pointlist.point.Count)
            {
                Debug.LogWarning("写真の数とスコアデータの数が一致しないため、処理を中断します。");
                break;
            }

            GameObject currentPhoto = photoList[i];
            var currentScoreData = pointlist.point[i];

            UpdateScores(currentScoreData);

            int photoScore = currentScoreData.eyes + currentScoreData.rarity + currentScoreData.bonus;
            cumulativeScore += photoScore;
            AddScore.text = $"{cumulativeScore}";

            skipRequested = false;

            yield return StartCoroutine(PhotoDisplaySequence(currentPhoto));

            if (currentPhoto != null)
            {
                Destroy(currentPhoto);
            }
        }

        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator PhotoDisplaySequence(GameObject photo)
    {
        photo.SetActive(true);

        if (skipRequested) yield break;
        yield return new WaitForSeconds(GetInterval(WholePhotoTime));

        if (skipRequested) yield break;
        DuplicateAndMoveEnemies(photo);
        yield return new WaitForSeconds(GetInterval(FocusedPhoto));

        if (skipRequested) yield break;
        yield return new WaitForSeconds(GetInterval(Information));

        // 複製したEnemyをすべて削除
        foreach (GameObject enemy in clonedEnemies)
        {
            if (enemy != null) Destroy(enemy);
        }
        clonedEnemies.Clear();
    }

    void DuplicateAndMoveEnemies(GameObject photo)
    {
        // 1. 表示先の範囲オブジェクトとそのColliderを取得
        if (displayArea == null)
        {
            Debug.LogError("オバケの表示範囲(displayArea)が設定されていません。");
            return;
        }
        var destCollider = displayArea.GetComponent<Collider>();
        if (destCollider == null)
        {
            Debug.LogError("displayAreaにColliderがアタッチされていません。");
            return;
        }

        // 2. 複製元の写真の全子要素から、写真全体のBoundsを計算
        Renderer[] sourceRenderers = photo.GetComponentsInChildren<Renderer>();
        if (sourceRenderers.Length == 0)
        {
            Debug.LogError("複製元の写真に表示可能な子要素(Renderer)がありません。");
            return;
        }
        Bounds sourceBounds = sourceRenderers[0].bounds;
        for (int i = 1; i < sourceRenderers.Length; i++)
        {
            sourceBounds.Encapsulate(sourceRenderers[i].bounds);
        }

        var destBounds = destCollider.bounds;

        Transform[] allDescendants = photo.GetComponentsInChildren<Transform>();

        // 全ての子孫の中から"Enemy"タグを持つオブジェクトを探す
        foreach (Transform descendant in allDescendants)
        {
            if (descendant.CompareTag("Enemy"))
            {
                GameObject clone = Instantiate(descendant.gameObject);

                Renderer cloneRenderer = clone.GetComponent<Renderer>();
                if(cloneRenderer != null)
                {
                    cloneRenderer.sortingOrder = 10;
                }

                // --- 座標とスケールのマッピング ---
                // 元の座標が、元の範囲(sourceBounds)のどのくらいの割合の位置にあるかを計算
                float relativeX = Mathf.InverseLerp(sourceBounds.min.x, sourceBounds.max.x, descendant.position.x);
                float relativeY = Mathf.InverseLerp(sourceBounds.min.y, sourceBounds.max.y, descendant.position.y);

                // 計算した割合を使って、新しい範囲(destBounds)内での座標を決定
                float newX = Mathf.Lerp(destBounds.min.x, destBounds.max.x, relativeX);
                float newY = Mathf.Lerp(destBounds.min.y, destBounds.max.y, relativeY);

                // 計算した新しいワールド座標を複製したオバケに設定
                clone.transform.position = new Vector3(newX, newY, destBounds.center.z);

                // スケールも写真全体の縮小率に合わせて調整
                float scaleRatioX = destBounds.size.x / sourceBounds.size.x;
                float scaleRatioY = destBounds.size.y / sourceBounds.size.y;
                float finalScaleRatio = Mathf.Min(scaleRatioX, scaleRatioY);
                
                Debug.Log(finalScaleRatio);
                //計算した比率を適用
                clone.transform.localScale *= finalScaleRatio;
                clone.transform.rotation = descendant.rotation;
                clone.name = descendant.name + "_Copy";
                // 後でまとめて削除するためにリストに追加
                clonedEnemies.Add(clone);
            }
        }
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
    void UpdateScores(EnemyData data)
    {
        if (pointlist.point == null)
        {
            Debug.LogError("PointListが設定されていません。");
            return;
        }
        // UIテキストに計算結果を反映
        NumberEyes.text = $"{data.eyes}つ";
        // ToDo: Coward, Furiousのカウント方法は元スクリプトで未定義のため、一旦0で表示
        GostType.text = $"0体 0体"; // ToDo
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
        }
        //早送り処理
        fastForwardRequested = Input.GetKey(KeyCode.F);
    }

    //早送り状態を考慮した、待機時間
    float GetInterval(float baseInterval)
    {
        //早送り要求があれば基本時間のn倍、なければ基本時間をそのまま返す
        return fastForwardRequested ? baseInterval * acceleration : baseInterval;
    }
}
