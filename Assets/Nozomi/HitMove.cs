using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitMove : MonoBehaviour
{
    Vector3 mousePos, objPos; //マウスとゲームオブジェクトの座標データを格納
    Vector3 rightTopPos;
    Vector3 leftBottomPos;
    public float depth = 10.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //マウスの座標を取得する
        mousePos = Input.mousePosition;
        //スクリーン座標をワールド座標に変換する
        objPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, depth));
        //ワールド座標をゲームオブジェクトの座標に設定する
        transform.position = objPos;

        rightTopPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, depth));
        leftBottomPos = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, depth));

        //オブジェクトがスクリーンからはみ出さないようにするやつ
        if (objPos.x >= rightTopPos.x) 
        {
            objPos.x = rightTopPos.x;
        }
    }
}
