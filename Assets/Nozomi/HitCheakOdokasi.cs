using UnityEngine;
using System.Collections;

public class HitCheakOdokasi : MonoBehaviour
{
    const float timer = 3.0f;
    [SerializeField] float alphaTimer;
    bool alphaStart;//透明化の開始判定フラグ　trueで開始
    public bool AlphaStart { set { alphaStart = value; } }//alphaStartを他でいじれるようにするセッター。あんまり使わない方がいい
    [SerializeField] Collider2D[] colliders;
    [SerializeField] SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        alphaStart = false;
        alphaTimer = 1.0f;
        colliders = GetComponents<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerCamera"))
        {
            alphaStart = true;
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
