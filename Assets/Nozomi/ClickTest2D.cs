using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClickTest2D : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject clickedGameObject;//クリックされたゲームオブジェクトを代入する変数

        // Update is called once per frame
        
           

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

                if (hit2d)
                {
                    clickedGameObject = hit2d.transform.gameObject;
                    Debug.Log(clickedGameObject.name);//ゲームオブジェクトの名前を出力
                    Destroy(clickedGameObject);//ゲームオブジェクトを破壊
                }

            }
        }
    
}
