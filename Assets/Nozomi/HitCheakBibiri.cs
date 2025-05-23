using UnityEngine;
using System.Collections;

public class HitCheakBibiri : MonoBehaviour
{
    const float timer = 3.0f;
    [SerializeField] float alphaTimer;
    bool alphaStart;
    public bool AlphaStart { set { alphaStart = value; } }
    [SerializeField] Collider2D[] colliders;
    [SerializeField] SpriteRenderer spriteRenderer;

    void Start()
    {
        alphaStart = false;
        alphaTimer = 1.0f;
        colliders = GetComponents<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        GameObject clickedGameObject;//クリックされたゲームオブジェクトを代入する変数

        if (alphaStart)
        {
            alphaTimer -= Time.deltaTime / timer;
            StartCoroutine(DestroyTimer(this.gameObject));
        }
        for (int i = 0; i < colliders.Length; i++)
         {
             colliders[i].enabled = !alphaStart;
         }
        spriteRenderer.color = new Color(1, 1, 1, alphaTimer);
        //spriteRenderer.color = new Color(1, 1, 1, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerCamera"))
        {
            alphaStart = true;
        }
        for(int i=0;i<colliders.Length; i++)
        {
            colliders[i].enabled=false;
        }
    }

    IEnumerator DestroyTimer(GameObject obj)
    {
        yield return new WaitForSeconds(timer);
        Destroy(obj);
    }
}
