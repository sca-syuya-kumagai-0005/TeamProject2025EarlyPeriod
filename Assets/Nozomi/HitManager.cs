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
        GameObject obj=GameObject.Find("SpawnManager");//敵を生成するスポナーを検索して代入
        spawnManager = obj.GetComponent<SpawnManager>();//spawnManagerに、上で検索したオブジェクトのInspectorからSpawnManagerを取得
        shoot=false;
        clickFarst=true;
        collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        collider.enabled=Input.GetMouseButton(0);//マウスを押したらコライダーを有効化
    }

    ///Unityで用意されている関数　当たり判定　用途に応じて使う関数が違うから注意　詳しくは自分で調べて
    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool haveEnemy = false;
        for (int i = 0; i < hitEnemies.Count; ++i)
        {
            if (hitEnemies[i] == collision.gameObject)
            {
                haveEnemy = true;//すでに同じオブジェクトを取得しているかどうか
            }
        }

        if (!haveEnemy)//すでに取得しているオブジェクトを除外
        {
            hitEnemies.Add(collision.gameObject);//まだ追加していないオブジェクトならListに追加する
        }

        for (int i = 0; i < hitEnemies.Count; i++)
        {
            HitCheak clickTest = hitEnemies[i].GetComponent<HitCheak>();//各敵についているHitCheckScriptを取得
            clickTest.AlphaStart = true;//HitCheckScriptのAlphaStartをTrueに変更
        }
         hitEnemies=new List<GameObject>();//hitEnemiesを初期化
    }
}
