using UnityEngine;
using System.Collections;

public class HitCheakOdokasi : MonoBehaviour
{
    const float timer = 3.0f;
    [SerializeField] float alphaTimer;
    bool alphaStart;//�������̊J�n����t���O�@true�ŊJ�n
    public bool AlphaStart { set { alphaStart = value; } }//alphaStart�𑼂ł������悤�ɂ���Z�b�^�[�B����܂�g��Ȃ���������
    [SerializeField] Collider2D[] colliders;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField]HitManager hitManager;
    bool flashHit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log(GameObject.Find("Hit").gameObject.GetComponent<HitManager>());
        hitManager = GameObject.Find("Hit").gameObject.GetComponent<HitManager>();  
        alphaStart = false;
        alphaTimer = 1.0f;
        colliders = GetComponents<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        flashHit = false;

    }

    // Update is called once per frame
    void Update()
    {
        //GameObject clickedGameObject;//�N���b�N���ꂽ�Q�[���I�u�W�F�N�g��������ϐ�
        if (!flashHit) 
        {
            
        }
        else
        {
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

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("ENTER");
        if (collision.CompareTag("PlayerCamera"))
        {
            if(hitManager.Mode == HitManager.modeChange.cameraMode)
            {
                alphaStart = true;
            }
            if (hitManager.Mode == HitManager.modeChange.flashMode)
            {
                flashHit = true;
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
