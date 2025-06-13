using UnityEngine;
using System.Collections;

public class HitCheakOdokasi : MonoBehaviour
{
    const float timer = 3.0f;
    float alphaTimer;
    bool alphaStart;

    public bool AlphaStart { set { alphaStart = value; } }

    Collider2D[] colliders;
    SpriteRenderer spriteRenderer;
    HitManager hitManager;

    void Start()
    {
        GameObject hitObj = GameObject.Find("Hit");
        if (hitObj != null)
        {
            hitManager = hitObj.GetComponent<HitManager>();
            if (hitManager == null)
            {
                Debug.LogWarning("[HitCheakOdokasi] 'Hit' オブジェクトに HitManager コンポーネントが見つかりません。");
            }
        }
        else
        {
            Debug.LogWarning("[HitCheakOdokasi] 'Hit' オブジェクトが見つかりません。");
        }

        alphaStart = false;
        alphaTimer = 1.0f;
        colliders = GetComponents<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (alphaStart)
        {
            alphaTimer -= Time.deltaTime / timer;
            StartCoroutine(DestroyTimer(this.gameObject));
        }

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = !alphaStart;
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(1, 1, 1, alphaTimer);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemySpriteAnimator esa = GetComponent<EnemySpriteAnimator>();
        if (esa != null && !esa.IsScalingPaused) return;

        if (collision.CompareTag("PlayerCamera"))
        {
            if (hitManager != null && hitManager.Mode == HitManager.modeChange.cameraMode)
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
        if (obj != null)
        {
            Destroy(obj);
        }
    }
}
