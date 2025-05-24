using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

//���ڈȍ~�̏����ŃG���[����������͗l�B�i���C���ς݁j
/*�G���[�����ƃ\�[�X�̖��A�����āi�x�z�搶����j
 ����
    hitEnemies(List)�̗v�f����Enemy�����������Ɍ���Ȃ��B
    53�s�ڂ�for�����񂵂����ɑz�����������遨null�ŃG���[
�\�[�X�̖��
    �����������ɂ�Enemy���������ɗv�f���������K�v������
    �����A�����_�̃\�[�X���Ɨv�f���̏����ꏊ���w��ł��Ȃ�
    ���R�FEnemy�������Ă���̂�HitManager�ł͂Ȃ�ClickTest2D������
    ���ꂾ��Enemy�̎�ނ𑝂₵������List�̒[�ȊO�����������ɍ���
������
    Enemy������ClickTest2D�ł͂Ȃ�HitManager�ɔC����
    ClickTest2D�Ńt���O�𗧂āAHitManager�ŏ�����List��remove������
 */
public class HitManager : MonoBehaviour
{
    [SerializeField]private List<GameObject> hitEnemies=new List<GameObject>();
    SpawnManager spawnManager;
    private bool shoot;
    private Collider2D collider;
    [SerializeField]bool click;
    public bool Click {  get { return click; }  }
    bool modeChange;
    bool coolTimeUp;
    public bool CoolTimeUp {get{return coolTimeUp;} }
    [SerializeField] float coolTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject obj=GameObject.Find("SpawnManager");//�G�𐶐�����X�|�i�[���������đ��
        spawnManager = obj.GetComponent<SpawnManager>();//spawnManager�ɁA��Ō��������I�u�W�F�N�g��Inspector����SpawnManager���擾
        shoot=false;
        click=false;
        modeChange = true;
        coolTimeUp = true;
       coolTime = 3.0f;
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
        if (modeChange&& coolTimeUp)
        {
               
            if (Input.GetMouseButtonDown(0))
            {
                click = true;
                StartCoroutine(CoolTimeCoroutine());

            }
            
        }
        Debug.Log(collider.enabled);
    }
    private void LateUpdate()
    {
        //if(coolTimeUp) collider.enabled = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!click||!collision.CompareTag("Enemy"))
        {
            return;
        }
        Debug.Log("Stay�ʉ�");
        bool haveEnemy = false;
        for (int i = 0; i < hitEnemies.Count; ++i)
        {
            
            if (hitEnemies[i] == collision.gameObject)
            {
                haveEnemy = true;//���łɓ����I�u�W�F�N�g���擾���Ă��邩�ǂ���
            }
        }

        Debug.Log(collision);
        if (!haveEnemy)//���łɎ擾���Ă���I�u�W�F�N�g�����O
        {
                hitEnemies.Add(collision.gameObject); 
        }

        for (int i = 0; i < hitEnemies.Count; i++)
        {
            HitCheakBIbiri clickTest = hitEnemies[i].GetComponent<HitCheakBIbiri>();//�e�G�ɂ��Ă���HitCheckScript���擾
            //HitCheakOdokasi clickTest2 = hitEnemies[i].GetComponent<HitCheakOdokasi>();
            clickTest.AlphaStart = true;//HitCheckScript��AlphaStart��True�ɕύX
           // clickTest2.AlphaStart = true;
        }
    }
    /////Unity�ŗp�ӂ���Ă���֐� �����蔻�� �p�r�ɉ����Ďg���֐����Ⴄ���璍�� �ڂ����͎����Œ��ׂ�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!click || !collision.CompareTag("Enemy"))
        {
            return;
        }
        Debug.Log("�ʉ�");
        bool haveEnemy = false;
        for (int i = 0; i < hitEnemies.Count; ++i)
        {
            if (hitEnemies[i] == collision.gameObject)
            {
                haveEnemy = true;//���łɓ����I�u�W�F�N�g���擾���Ă��邩�ǂ���
            }
        }

        if (!haveEnemy)//���łɎ擾���Ă���I�u�W�F�N�g�����O
        {
            hitEnemies.Add(collision.gameObject); //�܂��ǉ����Ă��Ȃ��I�u�W�F�N�g�Ȃ�List�ɒǉ�����
        }

        for (int i = 0; i < hitEnemies.Count; i++)
        {
            HitCheakBIbiri clickTest = hitEnemies[i].GetComponent<HitCheakBIbiri>();//�e�G�ɂ��Ă���HitCheckScript���擾
            HitCheakOdokasi clickTest2 = hitEnemies[i].GetComponent<HitCheakOdokasi>();
            clickTest.AlphaStart = true;//HitCheckScript��AlphaStart��True�ɕύX
            clickTest2.AlphaStart = true;
        }
        // hitEnemies = new List<GameObject>();//hitEnemies��������
    }

    IEnumerator CoolTimeCoroutine()
    {
        yield return 3;
        coolTimeUp = false;
        yield return new WaitForSeconds(coolTime);
        hitEnemies = new List<GameObject>();
        Debug.Log("�N�[���^�C���I���i�J�����j");
        coolTimeUp = true;
    }
}
