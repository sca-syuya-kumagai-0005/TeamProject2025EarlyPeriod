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
        Collider2D[] colliders = GameObject.FindObjectsOfType<Collider2D>();
        int nAddPoints = 0;
        int tAddPoints = 0;

        int AddedScore = 0;

        foreach (var col in colliders)
        {
            if (col.CompareTag("nEye") && IsFullyInside(circleCollider.bounds, col.bounds))
            {
                nAddPoints++; //ノーマルの目、1もしくは2追加される
            }
            else if (col.CompareTag("tEye") && IsFullyInside(circleCollider.bounds, col.bounds))
            {
                tAddPoints++; //脅かしの目、2もしくは5追加される
            }
        }

        // ノーマル目のスコア処理
        if (nAddPoints == 1) //ノーマルの目:1つの時
        {
            AddedScore += 1;
        }
        else if(nAddPoints == 2) //ノーマルの目:2つの時
        {
            AddedScore += 2;
        }
            
        // 脅かし目のスコア処理
        if (tAddPoints == 1) //脅かしの目:1つの時
        {
            AddedScore += 2;
        }
        else if (tAddPoints == 2) //脅かしの目:1つの時
        {
            AddedScore += 5;
        }

        // スコア加算と表示
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