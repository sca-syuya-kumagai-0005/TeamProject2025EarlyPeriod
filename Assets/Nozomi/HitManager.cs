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
    [SerializeField] private GameObject imageObject; // �\���E��\����؂�ւ���Ώ�
    [SerializeField] private GameObject blueRectangle; // Canvas�z���̐��l�p


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject obj = GameObject.Find("SpawnManager");//�G�𐶐�����X�|�i�[���������đ��
        spawnManager = obj.GetComponent<SpawnManager>();//spawnManager�ɁA��Ō��������I�u�W�F�N�g��Inspector����SpawnManager���擾
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
        /*// D�L�[�ŉ摜�\��
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
        }*/

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
        yield return new WaitForSeconds(coolTime);
        Debug.Log("�N�[���^�C���I���i�B�e�j");
        coolTimeUp = true;
    }
}
