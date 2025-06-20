using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;
    public int EnemyCount { get { return enemies.Length; } }
    [SerializeField] private GameObject[] hitEnemy;
    [SerializeField] private GameObject enemyParent;
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject flashImage;
    [SerializeField] private Vector3[] spawnPosition;
    [SerializeField] private GameObject stayEnemy;
    public GameObject FlashImage { get { return flashImage; } }
    const float time = 3.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //camLeftDown = Camera.main.ViewportToWorldPoint(Vector2.zero);
        //Debug.Log(camLeftDown);
        //camRightUp = Camera.main.ViewportToWorldPoint(Vector2.one);
        //Debug.Log(camRightUp);
        EnemySearch();
    }

    // Update is called once per frame
    void Update()
    {
        //if(flashImage.activeSelf)
        //{
        //    return;
        //}
      
        EnemySpawner();
        EnemySearch();
    }

    private void EnemySpawner()
    {
        int rand = Random.Range(0, 1000);//毎フレームrandを取得
        //if(enemies.Length>=5||rand<999)//敵が5体以上もしくは99.9%の確率でreturn
        if (enemies.Length >= 5)//テスト用　敵が５体以上ならreturn
        {
            return;
        }
        int makeRand=Random.Range(0,100);

        if(rand==0&&makeRand<30) Instantiate(enemy, PositionRand(), Quaternion.identity, enemyParent.transform);//ここのポジションをランダムに変更してね
        else if(rand==0)
        {
            int posRand =  Random.Range(0,spawnPosition.Length);
            Instantiate(stayEnemy, spawnPosition[posRand], Quaternion.identity, enemyParent.transform);
        }
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
