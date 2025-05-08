using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    Vector3 mousePos, pos;
    int score;
    Collider2D circleCollider;

    void Start()
    {
        score = 0;
        circleCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        // マウス座標からワールド座標へ変換し位置を更新
        mousePos = Input.mousePosition;
        pos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
        transform.position = pos;

        // マウス左クリックでスコア計算
        if (Input.GetMouseButtonDown(0))
        {
            AddScore();
        }
    }

    void AddScore()
    {
        Collider2D[] eyes = GameObject.FindObjectsOfType<Collider2D>();
        int addPoints = 0;

        foreach (var eye in eyes)
        {
            if (eye.CompareTag("Eye"))
            {
                // 各目の全Boundsがサークル内に含まれているかチェック
                if (IsFullyInside(circleCollider.bounds, eye.bounds))
                {
                    addPoints++;
                }
            }
        }

        if (addPoints > 0)
        {
            score += addPoints;
            Debug.Log("Score: " + score);
        }
    }

    bool IsFullyInside(Bounds outer, Bounds inner)
    {
        return outer.Contains(inner.min) && outer.Contains(inner.max);
    }
}