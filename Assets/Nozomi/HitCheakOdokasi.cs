using UnityEngine;
using System.Collections;

//�G���[�o���Ă�̂ŁA�g�p���Ȃ�
//�G���[�����F�T�T�s�ڂ�(hitManager.Mode == HitManager.modeChange.cameraMode)��null
//hitmanager���C���X�y�N�^�[���瓱���ł��Ȃ��@Enemy���v���n�u������H�i�v�m�F�j

public class HitCheakOdokasi : MonoBehaviour
{
    const float timer = 3.0f;
    float alphaTimer;
    bool alphaStart;//�������̊J�n����t���O�@true�ŊJ�n
    public bool AlphaStart { set { alphaStart = value; } }//alphaStart�𑼂ł������悤�ɂ���Z�b�^�[�B����܂�g��Ȃ���������
    Collider2D[] colliders;
    SpriteRenderer spriteRenderer;
    HitManager hitManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hitManager = GameObject.Find("Hit").gameObject.GetComponent<HitManager>();
        alphaStart = false;
        alphaTimer = 1.0f;
        colliders = GetComponents<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        //GameObject clickedGameObject;//�N���b�N���ꂽ�Q�[���I�u�W�F�N�g��������ϐ�

        if (alphaStart)
        {
            alphaTimer -= Time.deltaTime / timer;//������
            StartCoroutine(DestroyTimer(this.gameObject));//��莞�Ԍ�ɔj��
        }
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = !alphaStart;//collider�̃I���I�t��alphaStart�̔��΂ɐݒ�
        }
        spriteRenderer.color = new Color(1, 1, 1, alphaTimer);//�������𔽉f
                                                              //spriteRenderer.color = new Color(1, 1, 1, 0);


    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemySpriteAnimator esa = GetComponent<EnemySpriteAnimator>();
        if (!esa.IsScalingPaused) { return; }
        if (collision.CompareTag("PlayerCamera"))
        {
            if (hitManager.Mode == HitManager.modeChange.cameraMode)
            {
                alphaStart = true;
            }
        }
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = false;
        }
    }

    IEnumerator DestroyTimer(GameObject obj)
    {
        yield return new WaitForSeconds(timer);
        Destroy(obj);
    }
}