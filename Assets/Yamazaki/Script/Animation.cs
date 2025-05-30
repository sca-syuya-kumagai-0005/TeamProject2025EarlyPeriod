using UnityEngine;
using System.Collections;
public class Animation : MonoBehaviour
{
    //アニメーション1で私用する
    [Header("Canvas上のUIオブジェクト")]
    [SerializeField] private RectTransform uiObject;
    [Header("目的位置 (UIのローカル座標)")]
    [SerializeField] private Vector3 targetUIPosition;
    [Header("メインカメラ")]
    [SerializeField] private Camera targetCamera;
    [Header("カメラの目標X回転角")]
    [SerializeField] private float targetCameraXAngle = 30f;
    [Header("移動にかける時間")]
    [SerializeField] private float duration = 1.5f;
    [Header("最後に切り替えるスプライト")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite newSprite;

    //アニメーション2で使用する
    [SerializeField] private Material noise_Mat;

    [Header("スプライトをフェードする秒数")]
    public float fadeDuration = 1f;



    [Header("アニメーションの親格納(使用時表示)")]
    [SerializeField] GameObject[] titleAnimeObj;

    int anime_num;//アニメーションの数
    public int GetSetProperty
    {
        get { return anime_num; }
        set { anime_num = value; }
    }

    void Start()
    {
        for(int i= 0;i < titleAnimeObj.Length;i++)
        {
            titleAnimeObj[i].SetActive(false);
            Debug.Log(i);
        }

    }

    private void Update()
    {
        switch(anime_num)
        {
             case 1:
                titleAnimeObj[0].SetActive(true);
                StartCoroutine(Anime_1());
                anime_num = 0;
                break;
            case 2:
                titleAnimeObj[1].SetActive(true);
                StartCoroutine(Anime_2());
                anime_num = 0;
                break;
            default:
                Debug.Log("アニメナンバーがないよ");
                break;
        }
    }

    private IEnumerator Anime_1()
    {
        yield return new WaitForSeconds(0.5f);
        // ---- UI移動 ----
        Vector2 startPos = uiObject.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = Mathf.SmoothStep(0, 1, elapsed / duration);
            uiObject.anchoredPosition = Vector2.Lerp(startPos, targetUIPosition, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        uiObject.anchoredPosition = targetUIPosition;
        yield return new WaitForSeconds(0.5f);
        // ---- カメラ回転 ----
        Quaternion startRot = targetCamera.transform.rotation;
        Vector3 camEuler = targetCamera.transform.eulerAngles;
        Quaternion endRot = Quaternion.Euler(targetCameraXAngle, camEuler.y, camEuler.z);

        elapsed = 0f;
        duration = 0.5f;
        while (elapsed < duration)
        {
            float t = Mathf.SmoothStep(0, 1, elapsed / duration);
            targetCamera.transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        targetCamera.transform.rotation = endRot;
        yield return new WaitForSeconds(0.5f);
        // ---- スプライト差し替え ----
        if (spriteRenderer != null && newSprite != null)
        {
            spriteRenderer.sprite = newSprite;
            Color originalColor = spriteRenderer.color;
            originalColor.a = 1f;
            spriteRenderer.color = originalColor;

            // ---- フェードアウト処理 ----
            yield return new WaitForSeconds(0.5f);
            float fadeElapsed = 0f;
            while (fadeElapsed < fadeDuration)
            {
                float fadeT = fadeElapsed / fadeDuration;
                Color newColor = spriteRenderer.color;
                newColor.a = Mathf.Lerp(1f, 0f, fadeT);
                spriteRenderer.color = newColor;

                fadeElapsed += Time.deltaTime;
                yield return null;
            }
            
            // 最終的に完全に透明にする
            Color finalColor = spriteRenderer.color;
            finalColor.a = 0f;
            spriteRenderer.color = finalColor;
        }
        Debug.Log("扉開閉アニメーション");
    }

    private IEnumerator Anime_2()
    {
        yield return new WaitForSeconds(1f);
        for (int i= 0; i<2;i++)
        {
            
            noise_Mat.SetFloat("_Alpha", 0.4f);
            yield return new WaitForSeconds(0.1f);
            noise_Mat.SetFloat("_Alpha", 0f);
            yield return new WaitForSeconds(0.3f);
        }
        noise_Mat.SetFloat("_Alpha", 0.4f);
        yield return new WaitForSeconds(0.1f);
        noise_Mat.SetFloat("_Alpha", 1f);
        yield return new WaitForSeconds(1f);
        noise_Mat.SetFloat("_Alpha", 0.4f);
        yield return new WaitForSeconds(0.2f);
        noise_Mat.SetFloat("_Alpha", 0.8f);
        yield return new WaitForSeconds(0.2f);
        noise_Mat.SetFloat("_Alpha", 1.2f);
        yield return new WaitForSeconds(0.7f);
        yield return null; 
    }
}
