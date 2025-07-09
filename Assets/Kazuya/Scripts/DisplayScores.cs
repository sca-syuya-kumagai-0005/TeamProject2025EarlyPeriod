using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using System.Xml.Serialization;

public class DisplayScores : MonoBehaviour
{
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
    [SerializeField] Transform cloneRoot;//表示エリアに配置する親オブジェクト
    [SerializeField]GameObject cloneRootObject;   
    [SerializeField] GameObject maskScore;
    [SerializeField] Image ScoreTextImage;

    [Header("早送りするときの倍速度")]
    [SerializeField] float acceleration = 0.3f;

    [SerializeField] GameObject cameraMask; // シーンをまたいで運ばれてきた写真の親オブジェクト("PhotoStorage")
    Transform photoContainer; // 写真オブジェクトのTransform
    [SerializeField] List<GameObject> photoList = new List<GameObject>(); // 写真を格納するリスト
    GameObject currentPhoto;
    [SerializeField] List<GameObject> clonedEnemies = new List<GameObject>(); // 複製したオバケのリスト
    [Header("写真の拡大倍率")]
    [SerializeField] float Magnification =3.0f;
    [Header("ページ演出用")]
    [SerializeField] Image pageTurnImage;//空白のページのイラスト
    [SerializeField] float pageFadeTime = 0.5f;
    [Header("本が閉じる演出画像")]
    [SerializeField] Image closingBookImage;//本が閉じる画像
    [SerializeField] float closingtime = 1.0f;

                             //早送り/スキップのフラグ
    bool skipRequested = false;
    bool fastForwardRequested = false;

    [SerializeField] ScoreCalculator scoreZone;

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
        displayArea.gameObject.SetActive(false);
        ScoreTextImage.gameObject.SetActive(false);
    }
    /// <summary>
    /// 写真をリストにまとめる
    /// </summary>
    /// <returns></returns>
    IEnumerator ProcessPhotos()
    {
        for (int i = 0; i < photoList.Count; i++)
        {
            if(skipRequested) break;
            currentPhoto = photoList[i];

            yield return StartCoroutine(PhotoDisplaySequence(currentPhoto));

            if (currentPhoto != null)
            {
                Destroy(currentPhoto);
            }
        }
        if (skipRequested)
        {
            currentPhoto.SetActive(false);
            cloneRootObject.SetActive(false);
            displayArea.gameObject.SetActive(false);
            ScoreTextImage.gameObject.SetActive(false);
        }

        yield return StartCoroutine(PlayClosingBookEffect());
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
        displayArea.gameObject.SetActive(true);
        DuplicateAndMoveEnemies(photo);
        yield return new WaitForSeconds(GetInterval(FocusedPhoto));

        if (skipRequested) yield break;
        ScoreTextImage.gameObject.SetActive(true);
        Collider2D scoringArea = maskScore.GetComponent<Collider2D>();
        scoreZone.CalculateScoreLikeMouse(scoringArea);

        UpdateScores(scoreZone);
        yield return new WaitForSeconds(GetInterval(Information));

        // 複製したEnemyをすべて削除
        foreach (GameObject enemy in clonedEnemies)
        {
            if (enemy != null) Destroy(enemy);
        }
        clonedEnemies.Clear();  //中身の初期化
        scoreZone.ResetScore(); //スコアをリセット
        ResetScoreUI();         //同上
        photo.SetActive(false);
        displayArea.gameObject.SetActive(false);
        ScoreTextImage.gameObject.SetActive(false);
        yield return StartCoroutine(PageTurnEffect());//ページをめくる演出 
    }
    /// <summary>
    /// オバケを写真から切り取って複製する
    /// </summary>
    /// <param name="photo">写真オブジェクト名前</param>
    void DuplicateAndMoveEnemies(GameObject photo)
    {
        // 1. 表示先の範囲オブジェクトとそのCollider2Dを取得
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

        Transform maskTransform = photo.transform.Find(maskName);
        if (maskTransform == null) { Debug.LogError("Maskオブジェクトが選択されていません:" + maskName); return; }

        GameObject maskObject = maskTransform.gameObject;
        var maskCollider = maskObject.GetComponent<Collider>();
        if (maskCollider == null)
        {
            var tempCollider = maskObject.AddComponent<BoxCollider>();
            maskCollider = tempCollider;
        }

        var maskBounds = maskCollider.bounds;

        //. 複製元の写真の全子要素から、写真全体のBoundsを計算
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
        // 1. 全体的な縮小率を計算（写真→表示エリア）
        float baseScaleRatio = Mathf.Min(destBounds.size.x / sourceBounds.size.x, destBounds.size.y / sourceBounds.size.y);
        GameObject maskClone = Instantiate(maskObject);
        maskClone.transform.position = destBounds.center;
        maskClone.transform.localScale = maskObject.transform.lossyScale * baseScaleRatio * Magnification;
        clonedEnemies.Add(maskClone);
        
        Transform[] allDescendants = photo.GetComponentsInChildren<Transform>();
        int count = 1;

        // 全ての子孫の中から"Enemy"タグを持つオブジェクトを探す
        foreach (Transform descendant in allDescendants)
        {
        if (!descendant.CompareTag("Enemy")) continue;
            // --- ステップ1: まず全てのオバケを複製する ---
            // これで元のdescendantオブジェクトは一切変更されません。
           GameObject clone = Instantiate(descendant.gameObject);

            // すべての Collider2D を isTrigger に変更して物理干渉を防ぐ
            Collider2D[] colliders = clone.GetComponentsInChildren<Collider2D>();
            foreach (var col in colliders)
            {
                col.isTrigger = true;
            }
            // --- ステップ2: 複製したクローンの位置とスケールを計算する ---
            Vector3 originalCenterPos = maskObject.transform.position;
            Vector3 newCenterPos = destBounds.center;
            Vector3 relativePos = descendant.position - originalCenterPos;
            Vector3 newPosition = newCenterPos + (relativePos * baseScaleRatio * Magnification);

            clone.transform.position = newPosition;
            clone.transform.localScale = descendant.lossyScale * baseScaleRatio * Magnification;
            clone.transform.localScale = descendant.lossyScale * baseScaleRatio * Magnification;

            if (maskBounds.Contains(descendant.position))
            {
                Renderer cloneRenderer = clone.GetComponent<Renderer>();
                if (cloneRenderer != null) cloneRenderer.sortingOrder = 10;

                if (cloneRoot != null) clone.transform.SetParent(cloneRoot);

                clone.name = descendant.name + "_Copy" + $"{count}";
                clonedEnemies.Add(clone); // 表示リストに追加（後でまとめて消す）
                count++;
            }
            else
            {
                // 【範囲外の場合】 -> 元のオブジェクトをすぐに破棄する
                Destroy(clone);
            }
        }
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
    void UpdateScores(ScoreCalculator data)
    {
        AddScore.text = $"{data.totalScore}";
        NumberEyes.text = $"{data.totalEyes}つの目";
        Rarity.text = $"{data.rareEnemyCount}";
        BonusPoints.text = $"{data.BonusPoint}点";
        GostType.text = $"オドカシ:{data.odokashiCount}体 / ビビリ:{data.bibiriCount}体";

    }
    /// <summary>
    /// スキップ処理
    /// </summary>
    public void SkipButton()
    {
        skipRequested = true;
    }
    /// <summary>
    /// 早送り処理
    /// </summary>
    public void fastForwardButton()
    {
        if (!fastForwardRequested)
        {
            fastForwardRequested = true;
        }
        else
        {
            fastForwardRequested = false;
        }

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
    /// <summary>
    /// ページを閉じる演出
    /// </summary>
    /// <returns></returns>
    IEnumerator PageTurnEffect()
    {
        float timer = 0f;
        while (timer < pageFadeTime)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, timer / pageFadeTime);
            pageTurnImage.color = new Color(1f,1f,1f,alpha);
            yield return null;
        }
        //1瞬止める
        yield return new WaitForSeconds(0.2f);

        // フェードアウト
        timer = 0f;
        while (timer < pageFadeTime)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, timer / pageFadeTime);
            pageTurnImage.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }

        pageTurnImage.color = new Color(1f, 1f, 1f, 0f);
    }

    IEnumerator PlayClosingBookEffect()
    {
        // 初期状態（透明）
        Color imageColor = closingBookImage.color;
        imageColor.a = 0f;
        closingBookImage.color = imageColor;
        closingBookImage.gameObject.SetActive(true); // 念のため表示ON

        float timer = 0f;
        while (timer < closingtime)
        {
            timer += Time.deltaTime;
            float alphaImage = Mathf.Clamp01(timer / closingtime); // 0〜1の範囲
            closingBookImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, alphaImage);
            yield return null;
        }

        // しっかり表示されている状態で1秒待つ
        closingBookImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, 1f);
        yield return new WaitForSeconds(1.0f);
    }
}

