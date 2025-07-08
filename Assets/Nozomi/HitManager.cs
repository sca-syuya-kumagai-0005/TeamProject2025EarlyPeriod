using UnityEngine;
using System.Collections.Generic;
using System.Collections;



public class HitManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> hitEnemies = new List<GameObject>();
    SpawnManager spawnManager;
    private bool shoot;
    private Collider2D collider;
    [SerializeField] bool click;
    public bool Click { get { return click; } }
    [SerializeField] bool coolTimeUp;
    [SerializeField] float coolTime;
    public enum modeChange
    {
        cameraMode,
        flashMode,
    };
    [SerializeField] modeChange mode;
    public modeChange Mode { get { return mode; } }
    public bool HitCoolUp { get { return coolTimeUp; } }
    [SerializeField] private GameObject imageObject; // 表示・非表示を切り替える対象
    [SerializeField] private GameObject blueRectangle; // Canvas配下の青い四角


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject obj = GameObject.Find("SpawnManager");//敵を生成するスポナーを検索して代入
        spawnManager = obj.GetComponent<SpawnManager>();//spawnManagerに、上で検索したオブジェクトのInspectorからSpawnManagerを取得
        shoot = false;
        click = false;
        //(modeChange) = 1;
        coolTimeUp = true;
        collider = GetComponent<Collider2D>();
    }


    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            mode = modeChange.cameraMode;
            imageObject.SetActive(false);
            blueRectangle.SetActive(true);

        }
        if (Input.GetKey(KeyCode.D))
        {
            mode = modeChange.flashMode;
            imageObject.SetActive(true);
            blueRectangle.SetActive(false);
        }
        if ((mode == modeChange.cameraMode) && coolTimeUp)
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
        /*// Dキーで画像表示
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
        }*/

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
        yield return new WaitForSeconds(coolTime);
        Debug.Log("クールタイム終了（撮影）");
        coolTimeUp = true;
    }
}
