using System.Collections;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    Vector3 mousePos, pos;
    public static int score;
    Collider2D circleCollider;

    public Transform cameraCenter;

    bool canMove = true; //移動可能かどうか

    public ShutterEffect shutterEffect; //シャッターエフェクトへの参照

    public bool Touched_nred = false;
    public bool Touched_nblue = false;
    
    public bool Touched_tred = false;
    public bool Touched_tblue = false;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "nred")
        {
            Touched_nred = true;
            Debug.Log("n赤さわる");
        }
        
        if(collision.gameObject.tag == "nblue")
        {
            Touched_nblue = true;
            Debug.Log("n青さわる");
        }
        
        if(collision.gameObject.tag == "tred")
        {
            Touched_tred = true;
            Debug.Log("t赤さわる");
        }
        
        if(collision.gameObject.tag == "tblue")
        {
            Touched_tblue = true;
            Debug.Log("t青さわる");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "nred")
        {
            Touched_nred = false;
            Debug.Log("n赤さわってない");
        }
        
        if (collision.gameObject.tag == "nblue")
        {
            Touched_nblue = false;
            Debug.Log("n青さわってない");
        }
        
        if (collision.gameObject.tag == "tred")
        {
            Touched_tred = false;
            Debug.Log("t赤さわってない");
        }
        
        if (collision.gameObject.tag == "tblue")
        {
            Touched_tblue = false;
            Debug.Log("t青さわってない");
        }
    }

    //スコアの加算
    void AddScore()
    {
        Collider2D[] colliders = GameObject.FindObjectsOfType<Collider2D>();
        int nAddPoints = 0; //ノーマルの目
        int tAddPoints = 0; //脅かしの目
        int totalEyesScore = 0; //ノーマル +　脅かしのスコア

        //各コライダーが判定枠内に入っているかのチェック
        foreach (var col in colliders)
        {
            if (col.CompareTag("nEye") && IsFullyInside(circleCollider.bounds, col.bounds))
            {
                nAddPoints++;

            }
            else if (col.CompareTag("tEye") && IsFullyInside(circleCollider.bounds, col.bounds))
            {
                tAddPoints++;
            }
        }

        //ノーマルの目のポイントの加算
        if (nAddPoints == 1)
        {
            totalEyesScore += 1;
        }
        else if (nAddPoints == 2)
        {
            totalEyesScore += 2;
        }

        //脅かしの目のポイントの加算
        if (tAddPoints == 1)
        {
            totalEyesScore += 2;
        }
        else if (tAddPoints == 2)
        {
            totalEyesScore += 5;
        }

        int nRarebonus = 0;
        int tRarebonus = 0;

        if (nAddPoints == 1 || nAddPoints == 2)
        {
            if (Touched_nred)
            {
                nRarebonus += 50;
            }
            if (Touched_nblue)
            {
                nRarebonus += 100;
            }
        }
        
        if (tAddPoints == 1 || tAddPoints == 2)
        {
            if (Touched_tred)
            {
                tRarebonus += 70;
            }
            if (Touched_tblue)
            {
                tRarebonus += 120;
            }
        }
        int totalEyes = nAddPoints + tAddPoints; //最終的に判定される目の数
        int bonus = GetBonusPoint(totalEyes); //判定された目の数によるボーナス

         int AddedScore = totalEyesScore + bonus + nRarebonus + tRarebonus; //最終スコア

        if (AddedScore > 0)
        {
            score += AddedScore;

            Debug.Log("TotalEyesScore:" + totalEyesScore);
            Debug.Log("BonusScore:" + bonus);
            Debug.Log("nRarebouns" + nRarebonus);
            Debug.Log("tRarebouns" + tRarebonus);
            Debug.Log("Score: " + score);
        }
    }

    //ボーナススコアの配点 case:目の数 return:取得スコア
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

    bool IsFullyInside(Bounds outer, Bounds inner)
    {
        return outer.Contains(inner.min) && outer.Contains(inner.max);
    }
}