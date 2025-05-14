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
        int rand=Random.Range(0,1000);//���t���[��rand���擾
        //if(enemies.Length>=5||rand<999)//�G��5�̈ȏ��������99.9%�̊m����return
        if(enemies.Length>=5)//�e�X�g�p�@�G���T�̈ȏ�Ȃ�return
        {
            return;
        }
        Instantiate(enemy,transform.position,Quaternion.identity,enemyParent.transform);//�����̃|�W�V�����������_���ɕύX���Ă�
    }

    private void EnemySearch()
    {
        enemies=GameObject.FindGameObjectsWithTag("Enemy");
    }
}
