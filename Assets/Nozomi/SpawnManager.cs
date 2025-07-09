using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;
    public int EnemyCount { get { return enemies.Length; } }
    public static int stageLevel=0;
    [SerializeField,ReadOnly(false)]private bool clearLine;
    public bool ClearLine { get {  return clearLine; } }
    [SerializeField] private GameObject[] hitEnemy;
    [SerializeField] private GameObject enemyParent;
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject flashImage;
    [SerializeField] private Vector3[] spawnPosition;
    [SerializeField] private GameObject stayEnemy;
    [SerializeField] private GameObject flash;
    [SerializeField,ReadOnly(true)]private string[] spawnWaitEnemies = new string[25];//25は仮
    const string stayEnemyTag = "StayEnemy";
    const string moveEnemyTag = "MoveEnemy";
    int nextMakeEnemy = 0;
    public GameObject FlashImage { get { return flashImage; } }
    public GameObject Flash {  get { return flash; } }  
    const float time = 3.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        clearLine = false;
        if(SceneManager.GetActiveScene().name=="MainGame0"|| SceneManager.GetActiveScene().name == "MainGame1"|| SceneManager.GetActiveScene().name == "MainGame2")
        {
            stageLevel++;
        }
        else if(SceneManager.GetActiveScene().name == "Result")
        {
            stageLevel = 0;
        }
        EnemySearch();

        int makeMove;
        switch (stageLevel / 5)
        {
            case 0:
                {
                    makeMove = Random.Range(1, 2 + 1);
                }
                break;
            case 1:
                {
                    makeMove = Random.Range(2, 5 + 1);
                }
                break;
            case 2:
                {
                    makeMove = Random.Range(4, 7 + 1);
                }
                break;
            case 3:
                {
                    makeMove = Random.Range(7, 10 + 1);
                }
                break;
            default:
                {
                    makeMove = Random.Range(15, 20);
                }
                break;
        }

        int[] makeMovePoint = new int[makeMove];
        for(int i=0;i<makeMovePoint.Length;i++)
        {
            while (true)
            {
                int r = Random.Range(0, spawnWaitEnemies.Length);
                bool duplicate = false;
                for (int j = 0; j < makeMovePoint.Length; j++)
                {
                    if (r == makeMovePoint[j])
                    {
                        duplicate = true;
                    }
                }
                if(duplicate)
                {
                    continue;
                }
                makeMovePoint[i] = r;
                break;
            }
        }
        for(int i=0;i<spawnWaitEnemies.Length;i++)
        {
            for(int j=0;j<makeMovePoint.Length;j++)
            {
                spawnWaitEnemies[i] = stayEnemyTag;
                if (makeMovePoint[j]==i)
                {
                    spawnWaitEnemies[i]=moveEnemyTag;
                    break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        EnemySpawner();
        EnemySearch();
    }

    private void EnemySpawner()
    {
        
        int rand = Random.Range(0, 100);//毎フレームrandを取得
        Vector3 spawnPos = new Vector3(Random.Range(-5, 5), -3, 0);
        if (enemies.Length < 5)//テスト用　敵が５体以上ならreturn
        {
            switch(spawnWaitEnemies[nextMakeEnemy])
            {
                case (moveEnemyTag):
                    {
                        Instantiate(enemy, PositionRand(), Quaternion.identity, enemyParent.transform);
                    }
                    break;
                case (stayEnemyTag):
                    {
                        Instantiate(stayEnemy, spawnPos, Quaternion.identity, enemyParent.transform);
                    }
                    break;
            }
            nextMakeEnemy++;
            Debug.Log("エネミーは" + nextMakeEnemy);
            if(nextMakeEnemy == spawnWaitEnemies.Length)
            {
                clearLine = true;
            }
            nextMakeEnemy%=spawnWaitEnemies.Length;
            return;
        }
        //int makeRand=Random.Range(0,100);
       
        //if(rand!=0)
        //{
        //    return;
        //}
        //if(makeRand<20) Instantiate(enemy, PositionRand(), Quaternion.identity, enemyParent.transform);//ここのポジションをランダムに変更してね
        //else
        //{
        //    int posRand =  Random.Range(0,spawnPosition.Length);
        //    Instantiate(stayEnemy, spawnPos, Quaternion.identity, enemyParent.transform);
        //}
    }

    private void EnemySearch()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    private Vector2 PositionRand()
    {
        Vector3 rand = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 10f);
        Vector3 pos = Camera.main.ViewportToWorldPoint(rand);
        return pos;
    }
}
