using System.Collections;
using TMPro;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    Vector3 mousePos, pos;
    public static int score;
    Collider2D circleCollider;

    public Transform cameraCenter;

    bool canMove = true; //移動可能かどうか

    public ShutterEffect shutterEffect; //シャッターエフェクトへの参照

    public GameObject ScoreTextPrefab;

    void Start()
    {
        score = 0;
        circleCollider = GetComponent<Collider2D>();

        if (cameraCenter == null)
        {
            cameraCenter = transform.Find("CameraCenter"); //子オブジェクト"CameraCenter"を探す

        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canMove) //移動可能でクリックされたとき
        {
            AddScore();                                     //スコア追加
            StartCoroutine(DisableMovementForSeconds(3f));  //移動の無効化
            shutterEffect.TriggerEffect();                  //シャッターエフェクトの発動

        }
        if (canMove)
        {
            //マウスのスクリーン座標をワールド座標に変換
            mousePos = Input.mousePosition;
            Vector3 desiredWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));

            //カメラ中心のオフセットを保持しながら移動先を計算
            Vector3 offset = cameraCenter.position - transform.position;
            Vector3 desiredCameraCenterPos = desiredWorldPos + offset;
            Vector3 clampedCameraCenterPos = ClampToScreenBounds(desiredCameraCenterPos); //移動先を画面範囲内に制限

            transform.position = clampedCameraCenterPos - offset;
        }

    }

    //一定時間移動を無効
    IEnumerator DisableMovementForSeconds(float seconds)
    {
        canMove = false;
        yield return new WaitForSeconds(seconds);
        canMove = true;
    }

    //指定のワールド座標を画面内に収める
    Vector3 ClampToScreenBounds(Vector3 worldPos)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        float marginX = 0f;
        float marginY = 0f;

        //SpriteRendererのサイズを取得してマージンを設定
        if (cameraCenter.TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
        {
            Vector3 extents = sr.bounds.extents;
            Vector3 screenExtents = Camera.main.WorldToScreenPoint(extents + cameraCenter.position) - Camera.main.WorldToScreenPoint(cameraCenter.position);

            marginX = screenExtents.x;
            marginY = screenExtents.y;
        }
        //画面内に収まるようにスクリーン座標を制限
        screenPos.x = Mathf.Clamp(screenPos.x, marginX, Screen.width - marginX);
        screenPos.y = Mathf.Clamp(screenPos.y, marginY, Screen.height - marginY);
        //スクリーン座標を再びワールド座標に変換
        return Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, screenPos.z));
    }

    //スコアの加算
    void AddScore()
    {
        Collider2D[] colliders = GameObject.FindObjectsOfType<Collider2D>();
        int nEye = 0; //ノーマルの目
        int tEye = 0; //脅かしの目
        int nRed = 0; //ノーマル赤の目
        int tRed = 0; //脅かし赤の目
        int nBlue = 0; //ノーマル青の目
        int tBlue = 0; //脅かし青の目

        int TotalEyesScore = 0; //ノーマル +　脅かしのスコア

        int nRarebonus = 0;
        int tRarebonus = 0;

        //各コライダーが判定枠内に入っているかのチェック
        foreach (var col in colliders)
        {
            // マウス範囲に完全に入っているか確認
            if (!IsFullyInside(circleCollider.bounds, col.bounds))
            {
                continue;//完全に入っていたら次へ
            }

            Vector3 offset = new Vector3(0, 0.5f, 0); // 高さのオフセット
            Vector3 spawnPos = col.bounds.center + offset;
            ScoreText(spawnPos);


            //タグで目の種類を確認
            switch (col.tag)
            {
                case "nEye":
                    nEye++;
                    break;
                case "tEye":
                    tEye++;
                    break;
                case "nred":
                    nRed++;
                    break;
                case "nblue":
                    nBlue++;
                    break;
                case "tred":
                    tRed++;
                    break;
                case "tblue":
                    tBlue++;
                    break;
            }
        }

        int nTotalEye = nEye + nRed + nBlue;
        int nPieces = nTotalEye / 2;
        int nSurplus = nTotalEye % 2;
        //ノーマルの目のポイントの加算
        for(int i = 0; i < nPieces; i++)
        {
            TotalEyesScore += 2;
        }
        if(nSurplus == 1)
        {
            TotalEyesScore += 1;
        }

        int tTotalEye = tEye + tRed + tBlue;
        int tPieces = tTotalEye / 2;
        int tSurplus = tTotalEye % 2;

        //脅かしの目のポイントの加算
        for (int i = 0; i < tPieces; i++)
        {
            TotalEyesScore += 5;
        }
        if (tSurplus == 1)
        {
            TotalEyesScore += 2;
        }


        int TotalEyes = nEye + tEye + nRed + tRed + nBlue + tBlue; //最終的に判定される目の数

        int bonus = GetBonusPoint(TotalEyes); //判定された目の数によるボーナス

        //色によるレアボーナス
        nRarebonus += GetRerebounus(nRed, 50);
        nRarebonus += GetRerebounus(nBlue, 100);
        tRarebonus += GetRerebounus(tRed, 70);
        tRarebonus += GetRerebounus(tBlue, 120);

        int AddedScore = TotalEyesScore + bonus + nRarebonus + tRarebonus; //最終スコア

        if (AddedScore > 0)
        {
            score += AddedScore;

            Debug.Log("TotalEyes:" + TotalEyes);
            Debug.Log("TotalEyesScore:" + TotalEyesScore);
            Debug.Log("BonusScore:" + bonus);
            Debug.Log("nRarebouns" + nRarebonus);
            Debug.Log("tRarebouns" + tRarebonus);
            Debug.Log("Score: " + score);
        }
    }

    /// <summary>
    /// ボーナススコアの配点
    /// </summary>
    /// <param name="eyes">目の数</param>
    /// <returns>取得スコア</returns>
    int GetBonusPoint(int eyes)
    {
        switch (eyes)
        {
            case 1:
            case 2:
                return 0;
            case 3:
                return 5;
            case 4:
                return 10;
            case 5:
                return 20;
            case 6:
                return 50;
            case 7:
                return 100;
            case 8:
                return 250;
            case 9:
                return 300;
            case 10:
                return 500;
            default:
                return 0;
        }
    }


    /// <summary>
    /// 色によるレアボーナスの目の数(現在５体まで)
    /// </summary>
    /// <param name="rEye">レアの目の数</param>
    /// <param name="RereScore">数によるスコア</param>
    /// <returns>一定以上の時 +0 スコア</returns>
    int GetRerebounus(int rEye, int RereScore)
    {
        if (rEye == 0)              //0体
        {
            return 0;
        }
        else if (rEye <= 2)         //1体
        {
            return RereScore;
        }
        else if (rEye <= 4)         //２体
        {
            return RereScore * 2;
        }
        else if (rEye <= 6)         //3体
        {
            return RereScore * 3;
        }
        else if (rEye <= 8)         //４体
        {
            return RereScore * 4;
        }
        else if (rEye <= 10)        //5体
        {
            return RereScore * 5;
        }
        else return 0;
    }

    void ScoreText(Vector3 worldPosition)
    {
        if (ScoreTextPrefab != null)
        {
            GameObject text = Instantiate(ScoreTextPrefab, worldPosition, Quaternion.identity);
            Destroy(text, 1.0f); // 1秒で消える
        }
    }

    bool IsFullyInside(Bounds outer, Bounds inner)
    {
        return outer.Contains(inner.min) && outer.Contains(inner.max);
    }
}