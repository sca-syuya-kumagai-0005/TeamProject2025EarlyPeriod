using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]private GameObject[] enemies;
    public int EnemyCount { get { return enemies.Length; } }   
    [SerializeField]private GameObject[] hitEnemy;
    [SerializeField]private GameObject enemyParent;
    [SerializeField]private GameObject enemy;
    const float time=3.0f;
    Vector2 position;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EnemySearch();
    }

    // Update is called once per frame
    void Update()
    {
        EnemySpawner();
        EnemySearch();
    }

    private void EnemySpawner()
    {
        int rand=Random.Range(0,1000);//毎フレームrandを取得
        //if(enemies.Length>=5||rand<999)//敵が5体以上もしくは99.9%の確率でreturn
        if(enemies.Length>=5)//テスト用　敵が５体以上ならreturn
        {
            return;
        }
        Instantiate(enemy,transform.position,Quaternion.identity,enemyParent.transform);//ここのポジションをランダムに変更してね
    }

    private void EnemySearch()
    {
        enemies=GameObject.FindGameObjectsWithTag("Enemy");
    }
}
