using System.Collections;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    Vector3 mousePos, pos;
    int score;
    Collider2D circleCollider;

    public Transform cameraCenter;

    bool canMove = true; //移動可能かどうか

    public ShutterEffect shutterEffect; //シャッターエフェクトへの参照

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
        int nAddPoints = 0;
        int tAddPoints = 0;
        int AddedScore = 0;

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

        if (nAddPoints == 1)
        {
            AddedScore += 1;
        }
        else if (nAddPoints == 2)
        {
            AddedScore += 2;
        }

        if (tAddPoints == 1)
        {
            AddedScore += 2;
        }
        else if (tAddPoints == 2)
        {
            AddedScore += 5;
        }

        if (AddedScore > 0)
        {
            score += AddedScore;
            Debug.Log("Score: " + score);
        }
    }

    bool IsFullyInside(Bounds outer, Bounds inner)
    {
        return outer.Contains(inner.min) && outer.Contains(inner.max);
    }
}