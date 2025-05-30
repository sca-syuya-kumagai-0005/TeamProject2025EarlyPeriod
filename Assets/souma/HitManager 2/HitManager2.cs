using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;

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

    [SerializeField] private GameObject imageObject; // �\���E��\����؂�ւ���Ώ�
    [SerializeField] private GameObject blueRectangle; // Canvas�z���̐��l�p



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject obj = GameObject.Find("SpawnManager");//�G�𐶐�����X�|�i�[���������đ��
        spawnManager = obj.GetComponent<SpawnManager>();//spawnManager�ɁA��Ō��������I�u�W�F�N�g��Inspector����SpawnManager���擾
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
            // D�L�[�ŉ摜�\��
            if (Input.GetKeyDown(KeyCode.D))
            {
                imageObject.SetActive(true);
            }

            // A�L�[�ŉ摜��\���i���ɖ߂��j
            if (Input.GetKeyDown(KeyCode.A))
            {
                imageObject.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                // A�L�[�ŕ\��
                blueRectangle.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                // D�L�[�Ŕ�\��
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
            HitCheakBibiri clickTest = hitEnemies[i].GetComponent<HitCheakBibiri>();//�e�G�ɂ��Ă���HitCheckScript���擾
                                                                                    // HitCheakOdokasi clickTest2 = hitEnemies[i].GetComponent<HitCheakOdokasi>();
            if (clickTest != null) { clickTest.AlphaStart = true; }
            //clickTest2.AlphaStart = true;
        }
        //collider.enabled = false;
        hitEnemies = new List<GameObject>();
    }

    //Unity�ŗp�ӂ���Ă���֐� �����蔻�� �p�r�ɉ����Ďg���֐����Ⴄ���璍�� �ڂ����͎����Œ��ׂ�
    private void OnTriggerEnter2D(Collider2D collision)
    {
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
            HitCheakBibiri clickTest = hitEnemies[i].GetComponent<HitCheakBibiri>();//�e�G�ɂ��Ă���HitCheckScript���擾
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

