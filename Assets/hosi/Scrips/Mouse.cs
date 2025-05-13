using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    Vector3 mousePos, pos;
    int score;
    Collider2D circleCollider;

    public Transform cameraCenter;

    bool canMove = true; // ← 追加：移動できるかどうか
    void Start()
    {
        score = 0;
        circleCollider = GetComponent<Collider2D>();

        if (cameraCenter == null)
        {
            cameraCenter = transform.Find("CameraCenter");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canMove)
        {
            AddScore();
            StartCoroutine(DisableMovementForSeconds(3f));

        }
        if (canMove)
        {
            // マウス座標からワールド座標へ変換
            mousePos = Input.mousePosition;
            Vector3 desiredWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));

            Vector3 offset = cameraCenter.position - transform.position;
            Vector3 desiredCameraCenterPos = desiredWorldPos + offset;
            Vector3 clampedCameraCenterPos = ClampToScreenBounds(desiredCameraCenterPos);

            transform.position = clampedCameraCenterPos - offset;
        }
    }

    IEnumerator DisableMovementForSeconds(float seconds)
    {
        canMove = false;
        yield return new WaitForSeconds(seconds);
        canMove = true;
    }

    Vector3 ClampToScreenBounds(Vector3 worldPos)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        float marginX = 0f;
        float marginY = 0f;

        if (cameraCenter.TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
        {
            Vector3 extents = sr.bounds.extents;
            Vector3 screenExtents = Camera.main.WorldToScreenPoint(extents + cameraCenter.position) - Camera.main.WorldToScreenPoint(cameraCenter.position);

            marginX = screenExtents.x;
            marginY = screenExtents.y;
        }

        screenPos.x = Mathf.Clamp(screenPos.x, marginX, Screen.width - marginX);
        screenPos.y = Mathf.Clamp(screenPos.y, marginY, Screen.height - marginY);

        return Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, screenPos.z));
    }

    void AddScore()
    {
        Collider2D[] colliders = GameObject.FindObjectsOfType<Collider2D>();
        int nAddPoints = 0;
        int tAddPoints = 0;
        int AddedScore = 0;

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