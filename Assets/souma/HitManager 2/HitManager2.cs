using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;

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
public class HitManager2 : MonoBehaviour
{
    [SerializeField] private List<GameObject> hitEnemies = new List<GameObject>();
    SpawnManager spawnManager;
    private bool shoot;
    private Collider2D collider;
    [SerializeField] bool click;
    public bool Click { get { return click; } }
    [SerializeField] bool modeChange;
    [SerializeField] bool coolTimeUp;
    [SerializeField] float coolTime;

    [SerializeField] private GameObject imageObject; // 表示・非表示を切り替える対象
    [SerializeField] private GameObject blueRectangle; // Canvas配下の青い四角



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject obj = GameObject.Find("SpawnManager");//敵を生成するスポナーを検索して代入
        spawnManager = obj.GetComponent<SpawnManager>();//spawnManagerに、上で検索したオブジェクトのInspectorからSpawnManagerを取得
        shoot = false;
        click = false;
        modeChange = true;
        coolTimeUp = true;
        collider = GetComponent<Collider2D>();
    }


    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            modeChange = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            modeChange = false;
        }
        if (modeChange && coolTimeUp)
        {
            if (Input.GetMouseButton(0))
            {
                coolTimeUp = false;
                collider.enabled = true;
            }

        }
        hitEnemies = new List<GameObject>();
    }
    // Update is called once per frame
    void Update()
    {

        {
            // Dキーで画像表示
            if (Input.GetKeyDown(KeyCode.D))
            {
                imageObject.SetActive(true);
            }

            // Aキーで画像非表示（元に戻す）
            if (Input.GetKeyDown(KeyCode.A))
            {
                imageObject.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                // Aキーで表示
                blueRectangle.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                // Dキーで非表示
                blueRectangle.SetActive(false);
            }

        }



        if (collider.enabled)
        {
            StartCoroutine(CoolTimeCoroutine());
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("通過");
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
            hitEnemies.Add(collision.gameObject); //まだ追加していないオブジェクトならListに追加する
        }

        for (int i = 0; i < hitEnemies.Count; i++)
        {
            HitCheakBibiri clickTest = hitEnemies[i].GetComponent<HitCheakBibiri>();//各敵についているHitCheckScriptを取得
                                                                                    // HitCheakOdokasi clickTest2 = hitEnemies[i].GetComponent<HitCheakOdokasi>();
            if (clickTest != null) { clickTest.AlphaStart = true; }
            //clickTest2.AlphaStart = true;
        }
        //collider.enabled = false;
        hitEnemies = new List<GameObject>();
    }

    //Unityで用意されている関数 当たり判定 用途に応じて使う関数が違うから注意 詳しくは自分で調べて
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("通過");
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
            hitEnemies.Add(collision.gameObject); //まだ追加していないオブジェクトならListに追加する
        }

        for (int i = 0; i < hitEnemies.Count; i++)
        {
            HitCheakBibiri clickTest = hitEnemies[i].GetComponent<HitCheakBibiri>();//各敵についているHitCheckScriptを取得
                                                                                    // HitCheakOdokasi clickTest2 = hitEnemies[i].GetComponent<HitCheakOdokasi>();
            if (clickTest != null) { clickTest.AlphaStart = true; }
            //clickTest2.AlphaStart = true;
        }
        // collider.enabled = false;
        hitEnemies = new List<GameObject>();
    }

    IEnumerator CoolTimeCoroutine()
    {
        collider.enabled = false;
        Debug.Log("a");
        yield return new WaitForSeconds(coolTime);
        coolTimeUp = true;
    }
}

