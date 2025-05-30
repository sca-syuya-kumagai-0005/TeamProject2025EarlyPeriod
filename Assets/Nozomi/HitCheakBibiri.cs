using UnityEngine;
using System.Collections;

public class HitCheakBibiri : MonoBehaviour
{
    const float timer = 3.0f;
    [SerializeField] float alphaTimer;
    [SerializeField] bool alphaStart;//透明化の開始判定フラグ　trueで開始
    public bool AlphaStart { set { alphaStart = value; } }//alphaStartを他でいじれるようにするセッター。あんまり使わない方がいい
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
        //GameObject clickedGameObject;//クリックされたゲームオブジェクトを代入する変数

        if (alphaStart)
        {
            alphaTimer -= Time.deltaTime / timer;//透明化
            StartCoroutine(DestroyTimer(this.gameObject));//一定時間後に破壊
        }
        for (int i = 0; i < colliders.Length; i++)
         {
             colliders[i].enabled = !alphaStart;//colliderのオンオフをalphaStartの反対に設定
         }
        spriteRenderer.color = new Color(1, 1, 1, alphaTimer);//透明化を反映
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
