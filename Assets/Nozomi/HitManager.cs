using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using Unity.VisualScripting;

public class HitManager : MonoBehaviour
{
    [SerializeField]private List<GameObject> hitEnemies=new List<GameObject>();
    SpawnManager spawnManager;
    private bool shoot;
    private Collider2D collider;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject obj=GameObject.Find("SpawnManager");
        spawnManager = obj.GetComponent<SpawnManager>();
        shoot=false;
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            collider.enabled = true;//コライダーを有効化
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        bool haveEnemy = false;
        for (int i = 0; i < hitEnemies.Count; ++i)
        {
            if (hitEnemies[i] == collision.gameObject)
            {
                haveEnemy = true;
            }
        }

        if (!haveEnemy)//すでに取得しているObjectを除外
        {
            hitEnemies.Add(collision.gameObject);
        }

        for(int i = 0;i < hitEnemies.Count; i++)
        {
            ClickTest2D clickTest = hitEnemies[i].GetComponent<ClickTest2D>();
            clickTest.AlphaStart = true;
        }
        collider.enabled = false;
        
    }
}
