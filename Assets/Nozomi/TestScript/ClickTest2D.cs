using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClickTest2D : MonoBehaviour
{
    const float timer=3.0f;
    float flamecount = 0.0f;
    [SerializeField] float alphaTimer;
    bool alphaStart;
    public bool AlphaStart { set { alphaStart = value; } }
    [SerializeField]Collider2D[] colliders;
    [SerializeField]SpriteRenderer spriteRenderer;
    ObjectSpawn objectSpawn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        alphaStart=false;
        alphaTimer = 1.0f;
        colliders = GetComponents<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //objectSpawn = GameObject.Find("GameObject").GetComponent<ObjectSpawn>();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject clickedGameObject;//クリックされたゲームオブジェクトを代入する変数

        // Update is called once per frame
        
        if(alphaStart)
        {
            alphaTimer -= Time.deltaTime/timer;
            StartCoroutine(DestroyTimer(this.gameObject));
        }
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = !alphaStart;
        }
        spriteRenderer.color = new Color(1,1,1, alphaTimer);
        
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

        //    if (hit2d)
        //    {
        //        alphaStart = true;
        //        clickedGameObject = hit2d.transform.gameObject;
        //        Debug.Log(clickedGameObject.name);//ゲームオブジェクトの名前を出力
        //     
        //       // Collider.Destroy(clickedGameObject);//ゲームオブジェクトの判定を破壊
        //      
        //    }

        //}
    }
    
    IEnumerator DestroyTimer(GameObject obj)
    {
        yield return new WaitForSeconds(timer);
        Destroy(obj);
    }
}
