using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]private GameObject[] enemies;
    public int EnemyCount { get { return enemies.Length; } }   
    [SerializeField]private GameObject[] hitEnemy;
    [SerializeField]private GameObject enemyParent;
    [SerializeField]private GameObject enemy;
    const float time=3.0f;
    //Vector2 position;
    //Vector3 camLeftDown;
    //Vector3 camRightUp;
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
        EnemySpawner();
        EnemySearch();
    }

    private void EnemySpawner()
    {
        int rand=Random.Range(0,1000);//���t���[��rand���擾
        //if(enemies.Length>=5||rand<999)//�G��5�̈ȏ��������99.9%�̊m����return
        if(enemies.Length>=5)//�e�X�g�p�@�G���T�̈ȏ�Ȃ�return
        {
            return;
        }
        Instantiate(enemy, PositionRand(), Quaternion.identity,enemyParent.transform);//�����̃|�W�V�����������_���ɕύX���Ă�
    }

    private void EnemySearch()
    {
        enemies=GameObject.FindGameObjectsWithTag("Enemy");
    }

    private Vector2 PositionRand()
    {
        Vector3 rand = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 10f);
        Vector3 pos = Camera.main.ViewportToWorldPoint(rand);
        return pos;
    }
}
