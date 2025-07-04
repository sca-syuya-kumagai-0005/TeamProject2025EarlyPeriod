using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using System.Linq;

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
    [SerializeField] Transform cloneRoot;//表示エリアに配置する親オブジェクト

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

            UpdateScores();

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
    void UpdateScores()
    {
        if (clonedEnemies.Count > 0)
        {
            GameObject maskClone = clonedEnemies[0]; // 最初の要素はマスク
            int addedScore = CalculateScore(maskClone);
            cumulativeScore += addedScore;
            AddScore.text = $"{cumulativeScore}";
        }
    }


    int CalculateScore(GameObject maskClone)
    {
        int nEye = 0,tEye = 0,nRed = 0, tRed = 0,nBlue = 0,tBlue = 0;
        string[] validTags = {"nEye","tEye","nred","tred","nblue","tblue"};

        Collider maskCol = maskClone.GetComponent<Collider>();

        Bounds maskBounds = maskCol.bounds;
        foreach(GameObject ghot in clonedEnemies)
        {
            if(ghot == null) continue;

            Collider[] childCols = ghot.GetComponentsInChildren<Collider>();
            foreach(Collider col in childCols)
            {
                if (!validTags.Contains(col.tag)) continue;

                if (maskBounds.Contains(col.bounds.min) && maskBounds.Contains(col.bounds.max))
                {
                    switch (col.tag)
                    {
                        case "nEye": nEye++; break;
                        case "tEye": tEye++; break;
                        case "nred": nRed++; break;
                        case "tred": tRed++; break;
                        case "nblue": nBlue++; break;
                        case "tblue": tBlue++; break;
                    }
                }
            }
        }
        // スコア計算（Mouse.cs と同じルール）
        int score = 0;
        int normal = nEye + nRed + nBlue;
        score += (normal / 2) * 2;
        if (normal % 2 == 1) score += 1;

        int threaten = tEye + tRed + tBlue;
        score += (threaten / 2) * 5;
        if (threaten % 2 == 1) score += 2;

        score += GetRareBonus(nRed, 50);
        score += GetRareBonus(nBlue, 100);
        score += GetRareBonus(tRed, 70);
        score += GetRareBonus(tBlue, 120);

        score += GetBonusPoint(normal + threaten);
        return score;
    }

    int GetBonusPoint(int eyes)
    {
        switch (eyes)
        {
            case 3: return 5;
            case 4: return 10;
            case 5: return 20;
            case 6: return 50;
            case 7: return 100;
            case 8: return 250;
            case 9: return 300;
            case 10: return 500;
            default: return 0;
        }
    }

    int GetRareBonus(int count, int baseScore)
    {
        if (count == 0) return 0;
        if (count <= 2) return baseScore;
        if (count <= 4) return baseScore * 2;
        if (count <= 6) return baseScore * 3;
        if (count <= 8) return baseScore * 4;
        if (count <= 10) return baseScore * 5;
        return baseScore * 6;
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
