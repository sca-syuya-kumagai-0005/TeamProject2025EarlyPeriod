using UnityEngine;
using System.Collections.Generic;


public class HitManager : MonoBehaviour
{
    [SerializeField]private List<GameObject> hitEnemies=new List<GameObject>();
    SpawnManager spawnManager;
    private bool shoot;
    private Collider2D collider;
    bool clickFarst;
    bool modeChange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject obj=GameObject.Find("SpawnManager");
        spawnManager = obj.GetComponent<SpawnManager>();
        shoot=false;
        clickFarst=true;
        modeChange = true;
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        { 
            modeChange = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            modeChange = false;
        }
        if (modeChange) 
        {
            collider.enabled = Input.GetMouseButton(0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool haveEnemy = false;
        for (int i = 0; i < hitEnemies.Count; ++i)
        {
            if (hitEnemies[i] == collision.gameObject)
            {
                haveEnemy = true;
            }
        }

        if (!haveEnemy)//‚·‚Å‚ÉŽæ“¾‚µ‚Ä‚¢‚éObject‚ðœŠO
        {
            hitEnemies.Add(collision.gameObject);
        }

        for (int i = 0; i < hitEnemies.Count; i++)
        {
            HitCheakBibiri clickTest = hitEnemies[i].GetComponent<HitCheakBibiri>();
            clickTest.AlphaStart = true;
        }
         hitEnemies=new List<GameObject>();
    }
}
