using UnityEngine;
using System.Collections;

//エラー出してるので、使用しない
//エラー原因：５５行目の(hitManager.Mode == HitManager.modeChange.cameraMode)がnull
//hitmanagerをインスペクターから導入できない　Enemyがプレハブだから？（要確認）

public class HitCheakOdokasi : MonoBehaviour
{
    const float timer = 3.0f;
    float alphaTimer;
    bool alphaStart;//透明化の開始判定フラグ　trueで開始
    public bool AlphaStart { set { alphaStart = value; } }//alphaStartを他でいじれるようにするセッター。あんまり使わない方がいい
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