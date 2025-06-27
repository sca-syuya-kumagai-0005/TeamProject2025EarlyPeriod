using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

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
    [SerializeField] GameObject photoDisplayReference; // 写真を表示する位置と大きさの基準となるオブジェクト
    [SerializeField] GameObject displayArea; // 複製したオバケを表示する範囲となるオブジェクト
    [SerializeField] string maskName;

    [Header("早送りするときの倍速度")]
    [SerializeField] float acceleration = 0.3f;

    [SerializeField] GameObject cameraMask; // シーンをまたいで運ばれてきた写真の親オブジェクト("PhotoStorage")
    Transform photoContainer; // 写真オブジェクトのTransform
    [SerializeField] List<GameObject> photoList = new List<GameObject>(); // 写真を格納するリスト
    [SerializeField] List<GameObject> clonedEnemies = new List<GameObject>(); // 複製したオバケのリスト
    [Header("写真の拡大倍率")]
    [SerializeField] float Magnification =3.0f;

    int cumulativeScore = 0; // 累計スコア
                             //早送り/スキップのフラグ
    bool skipRequested = false;
    bool fastForwardRequested = false;

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
            SceneManager.LoadScene(nextSceneName);
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
    /// <summary>
    /// 写真をリストにまとめる
    /// </summary>
    /// <returns></returns>
    IEnumerator ProcessPhotos()
    {
        for (int i = 0; i < photoList.Count; i++)
        {
            //if (i >= pointlist.point.Count)
            //{
            //    Debug.LogWarning("写真の数とスコアデータの数が一致しないため、処理を中断します。");
            //    break;
            //}

            GameObject currentPhoto = photoList[i];
            //var currentScoreData = pointlist.point[i];

            //UpdateScores(currentScoreData);

            //int photoScore = currentScoreData.eyes + currentScoreData.rarity + currentScoreData.bonus;
            //cumulativeScore += photoScore;
            //AddScore.text = $"{cumulativeScore}";

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
    /// <summary>
    /// 写真を表示するシークエンス
    /// </summary>
    /// <param name="photo">写真の名前</param>
    /// <returns></returns>
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
    /// <summary>
    /// オバケを写真から切り取って複製する
    /// </summary>
    /// <param name="photo">写真オブジェクト名前</param>
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
        Transform maskTransform = photo.transform.Find(maskName);
        if (maskTransform == null) { Debug.LogError("Maskオブジェクトが選択されていません:" + maskName); return; }

        GameObject maskObject = maskTransform.gameObject;
        if (maskObject.GetComponent<Collider>() == null)
        {
            maskObject.AddComponent<BoxCollider>();
        }

        Transform[] allDescendants = photo.GetComponentsInChildren<Transform>();

        bool maskDuplicated = false;

        // 全ての子孫の中から"Enemy"タグを持つオブジェクトを探す
        foreach (Transform descendant in allDescendants)
        {

            if (!descendant.CompareTag("Enemy"))  continue; 
            {
                //レイキャストによる判定
                Vector3 rayorigin = descendant.position;
                Vector3 rayDirection = (maskObject.transform.position - rayorigin).normalized;
                float rayDistance = Vector3.Distance(rayorigin, maskObject.transform.position);
                Ray ray = new Ray(rayorigin, rayDirection);
                RaycastHit hit;
                Debug.DrawRay(rayorigin, rayDirection * rayDistance, Color.red, 1.0f);
                Debug.Log("レイの設定終了");
                //if (Physics.Raycast(rayorigin, rayDirection, out hit, rayDistance + 0.1f))
                {
                    //if (hit.transform == maskTransform)
                    {
                        if (!maskDuplicated)
                        {
                            Vector3 originalPos = maskObject.transform.position;

                            float relatveX = Mathf.InverseLerp(sourceBounds.min.x, sourceBounds.max.x, originalPos.x);
                            float relatveY = Mathf.InverseLerp(sourceBounds.min.y, sourceBounds.max.y, originalPos.y);
                            // 1. スケール調整（写真と表示エリアの比率に合わせて）
                            float scaleRatiomaskX = destBounds.size.x / sourceBounds.size.x;
                            float scaleRatiomaskY = destBounds.size.y / sourceBounds.size.y;
                            float finalScaleRatiomask = Mathf.Min(scaleRatiomaskX, scaleRatiomaskY);

                            // 2. 複製して center に表示
                            GameObject maskClone = Instantiate(maskObject.gameObject);

                            //  displayArea の中心に配置
                            maskClone.transform.position = new Vector3(destBounds.center.x, destBounds.center.y, destBounds.center.z + 0.01f);

                            // スケール・回転の調整
                            maskClone.transform.localScale *= finalScaleRatiomask * Magnification;
                            maskClone.transform.rotation = maskObject.transform.rotation;
                            maskClone.name = maskObject.name + "_Copy";

                            clonedEnemies.Add(maskClone);
                            maskDuplicated = true;
                        }
                        Debug.Log("レイを感知　オブジェクトの複製を開始");
                        // スケール比率
                        float scaleX = destBounds.size.x / sourceBounds.size.x;
                        float scaleY = destBounds.size.y / sourceBounds.size.y;
                        float finalScaleRatio = Mathf.Min(scaleX, scaleY);
                        //マスクの元座標と複製後の中心座標
                        Vector3 originalMaskPos = maskObject.transform.position;
                        Vector3 maskCenterPos = destBounds.center;

                        //オバケの複製
                        GameObject clone = Instantiate(descendant.gameObject);

                        Renderer cloneRenderer = clone.GetComponent<Renderer>();
                        if (cloneRenderer == null) { Debug.LogError("Rendererの参照不可"); return; }

                        if (cloneRenderer != null)
                        {
                            cloneRenderer.sortingOrder = 10;
                        }

                        Vector3 offset = descendant.position - originalMaskPos;
                        // 計算した新しいワールド座標を複製したオバケに設定
                        clone.transform.position =maskCenterPos+offset * finalScaleRatio;



                        //計算した比率を適用
                        clone.transform.localScale *= finalScaleRatio* Magnification;
                        clone.transform.rotation = descendant.rotation;
                        clone.name = descendant.name + "_Copy";
                        // 後でまとめて削除するためにリストに追加
                        clonedEnemies.Add(clone);

                    }
                }
            }
        }
        maskTransform = null;
    }
    /// <summary>
    /// スコアの初期化
    /// </summary>
    void ResetScoreUI()
    {
        NumberEyes.text = "_";
        GostType.text = "_";
        Rarity.text = "_";
        BonusPoints.text = "_";
        AddScore.text = "0";
    }
    /// <summary>
    /// スコアを集計し、UIテキストを更新する
    /// </summary>
    /// <param name="data"></param>
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

    /// <summary>
    /// ユーザー入力処理
    /// </summary>
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

    /// <summary>
    /// 早送り状態を考慮した、待機時間
    /// </summary>
    /// <param name="baseInterval">標準の待機時間</param>
    /// <returns></returns>
    float GetInterval(float baseInterval)
    {
        //早送り要求があれば基本時間のn倍、なければ基本時間をそのまま返す
        return fastForwardRequested ? baseInterval * acceleration : baseInterval;
    }
}
