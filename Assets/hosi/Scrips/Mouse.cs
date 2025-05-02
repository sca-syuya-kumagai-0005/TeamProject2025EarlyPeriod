using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    Vector3 mousePos, pos; //マウスとゲームオブジェクトの座標データを格納

    public bool AddPoint = false;
    int score;

    private void Start()
    {
        AddPoint = false;
        score = 0;
    }

    void Update()
    {
        //マウスの座標を取得する
        mousePos = Input.mousePosition;
        //マウス位置を確認
        //Debug.Log(mousePos);
        //スクリーン座標をワールド座標に変換する
        pos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
        //ワールド座標をゲームオブジェクトの座標に設定する
        transform.position = pos;

        if(AddPoint == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                score++;
                Debug.Log(score);
            }
        }
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        AddPoint = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        AddPoint = false;
    }

}