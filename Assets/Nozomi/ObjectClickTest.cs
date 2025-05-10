using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectClickTest : MonoBehaviour
{
    //クリックされたゲームオブジェクトを代入する変数
    GameObject clickedGameObject;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit))
            {
                clickedGameObject = hit.collider.gameObject;
                Debug.Log(clickedGameObject.name);//ゲームオブジェクトの名前を出力
                Destroy(clickedGameObject);//ゲームオブジェクトを破壊
            }
        }
    }
}
