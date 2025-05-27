using UnityEngine;
using System.Collections;

public class HitCheakBibiri : MonoBehaviour
{
    const float timer = 3.0f;
    [SerializeField] float alphaTimer;
    [SerializeField] bool alphaStart;//�������̊J�n����t���O�@true�ŊJ�n
    public bool AlphaStart { set { alphaStart = value; } }//alphaStart�𑼂ł������悤�ɂ���Z�b�^�[�B����܂�g��Ȃ���������
    [SerializeField] Collider2D[] colliders;
    [SerializeField] SpriteRenderer spriteRenderer;
    bool flashHit;

    void Start()
    {
        alphaStart = false;
        alphaTimer = 1.0f;
        colliders = GetComponents<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        flashHit = false;
    }

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


    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(!hitManager.CoolTimeUp)
    //    {
    //        return;
    //    }
    //    if (collision.CompareTag("PlayerCamera"))
    //    {
    //        Debug.Log("ENTER");
    //        alphaStart = true;
    //    }
    //    for (int i = 0; i < colliders.Length; i++)
    //    {
    //        colliders[i].enabled = false;
    //    }
    //}

    //private void OnTriggerStay2D(Collider2D collision)
    //{
       
    //    if (collision.CompareTag("PlayerCamera"))
    //    {
    //        Debug.Log("STAY");
    //        alphaStart = true;
    //    }
    //    for (int i = 0; i < colliders.Length; i++)
    //    {
    //        colliders[i].enabled = false;
    //    }
    //}
    IEnumerator DestroyTimer(GameObject obj)
    {
        yield return new WaitForSeconds(timer);
        Destroy(obj);
    }
}
