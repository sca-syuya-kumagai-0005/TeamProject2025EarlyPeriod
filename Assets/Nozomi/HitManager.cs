using UnityEngine;
using System.Collections.Generic;

//二回目以降の消去でエラーが発生する模様。（仮修正済み）
/*エラー原因とソースの問題、解決案（堀越先生から）
 原因
    hitEnemies(List)の要素数がEnemyを消した時に減らない。
    53行目のfor文を回した時に想定よりも多く回る→nullでエラー
ソースの問題
    上を解決するにはEnemyを消す時に要素数も消す必要がある
    ただ、現時点のソースだと要素数の消す場所を指定できない
    理由：Enemyを消しているのがHitManagerではなくClickTest2Dだから
    これだとEnemyの種類を増やした時やListの端以外を消した時に困る
解決案
    Enemy消去をClickTest2DではなくHitManagerに任せる
    ClickTest2Dでフラグを立て、HitManagerで消去とListのremoveをする
 */
public class HitManager : MonoBehaviour
{
    [SerializeField]private List<GameObject> hitEnemies=new List<GameObject>();
    SpawnManager spawnManager;
    private bool shoot;
    private Collider2D collider;
    bool clickFarst;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject obj=GameObject.Find("SpawnManager");
        spawnManager = obj.GetComponent<SpawnManager>();
        shoot=false;
        clickFarst=true;
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        collider.enabled=Input.GetMouseButton(0);
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

        if (!haveEnemy)//すでに取得しているObjectを除外
        {
            hitEnemies.Add(collision.gameObject);
        }

        for (int i = 0; i < hitEnemies.Count; i++)
        {
            HitCheak clickTest = hitEnemies[i].GetComponent<HitCheak>();
            clickTest.AlphaStart = true;
        }
         hitEnemies=new List<GameObject>();
    }
}
