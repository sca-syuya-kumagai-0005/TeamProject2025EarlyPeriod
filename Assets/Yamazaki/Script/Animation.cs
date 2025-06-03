using UnityEngine;
using System.Collections;
public class Animation : MonoBehaviour
{
    //スナップショットで使用
    [Header("移動開始オフセット（ローカル座標）")]
    [SerializeField] private Vector3 startOffset = new Vector3(25f, 0, 0);
    [SerializeField] private GameObject snapShot;
    [Header("演出にかける秒数")]
    [SerializeField] private float duration_Phot = 1.0f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private SpriteRenderer spriteRenderer_Phot;

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
    [SerializeField] ParticleSystem anime1_ParticleSystem;
    //アニメーション2で使用する
    [SerializeField] private Material noise_Mat;
    [SerializeField] private SpriteRenderer backGround_anim2;

    [Header("スプライトをフェードする秒数")]
    private float fadeDuration = 1f;

    //アニメーション３で使用する
    [Header("驚き演出")]
    [SerializeField]private SpriteRenderer surprise;
    [Header("写真上に出現するお化け")]
    [SerializeField] private SpriteRenderer onPhotGhost;
    [Header("上昇後の最大アルファ値")]
    [SerializeField] private float peakAlpha = 1f;
    [Header("最終的に下げるアルファ値")]
    [SerializeField] private float finalAlpha = 0.4f;
    [Header("上昇・下降にかける秒数")]
    [SerializeField] private float durationUp = 1f;
    [SerializeField] private float durationDown = 1f;
    [Header("最後に切り替えるスプライト")]
    [SerializeField] SpriteRenderer spriteRenderer_3;
    [SerializeField] Sprite newSprite_3;
    [Header("お化け成仏jの演出")]
    [SerializeField] ParticleSystem[] anime3_ParticleSystem;


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
        backGround_anim2.enabled = false;
        for (int i= 0;i < titleAnimeObj.Length;i++)
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
            case 3:
                titleAnimeObj[2].SetActive(true);
                StartCoroutine(Anime_3());
                anime_num = 0;
                break;
            default:
                Debug.Log("アニメナンバーがないよ");
                break;
        }
    }
    /*
    public IEnumerator SlideAndFadeIn()
    {
        Debug.Log("写真移動");
        float elapsed = 0f;

        while (elapsed < duration_Phot)
        {
            float t = elapsed / duration_Phot;

            // 位置を補間
            snapShot.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);

            // αを補間
            Color color = spriteRenderer_Phot.color;
            color.a = Mathf.Lerp(0f, 1f, t);
            spriteRenderer_Phot.color = color;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 最終値に固定
        snapShot.transform.localPosition = targetPosition;
        Color finalColor = spriteRenderer_Phot.color;
        finalColor.a = 1f;
        spriteRenderer_Phot.color = finalColor;
        anime_num = 3;
    }*/

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
            anime1_ParticleSystem.Play();
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
        noise_Mat.SetFloat("_Alpha", 0.05f);
        yield return new WaitForSeconds(1f);
        noise_Mat.SetFloat("_Alpha", 0f);
        yield return new WaitForSeconds(0.5f);
        noise_Mat.SetFloat("_Alpha", 0.05f);
        yield return new WaitForSeconds(0.25f);
        noise_Mat.SetFloat("_Alpha", 0f);
        yield return new WaitForSeconds(0.25f);

        for (int i= 0; i<2;i++)
        {
            
            noise_Mat.SetFloat("_Alpha", 0.4f);
            yield return new WaitForSeconds(0.1f);
            noise_Mat.SetFloat("_Alpha", 0.8f);
            yield return new WaitForSeconds(0.02f);
            noise_Mat.SetFloat("_Alpha", 3f);
            yield return new WaitForSeconds(0.3f);
            noise_Mat.SetFloat("_Alpha", 0f);
            yield return new WaitForSeconds(1f);
        }

        noise_Mat.SetFloat("_Alpha", 0.4f);
        yield return new WaitForSeconds(0.1f);
        noise_Mat.SetFloat("_Alpha", 1f);
        yield return new WaitForSeconds(0.5f);
        noise_Mat.SetFloat("_Alpha", 0f);
        yield return new WaitForSeconds(0.1f);
        noise_Mat.SetFloat("_Alpha", 4f);
        yield return new WaitForSeconds(0.13f);
        backGround_anim2.enabled = true;
        noise_Mat.SetFloat("_Alpha", 1.2f);
        yield return new WaitForSeconds(0.35f);
        noise_Mat.SetFloat("_Alpha", 0f);
       
    }

    private IEnumerator Anime_3()
    {
        yield return new WaitForSeconds(1f);
        surprise.enabled = true;
        Color color = surprise.color;
        float elapsed = 0f;
        for (int i = 0; i < 3; i++)
        {
            // 上昇
            
            float startAlpha = color.a;

            while (elapsed < durationUp)
            {
                float t = elapsed / durationUp;
                color.a = Mathf.Lerp(startAlpha, peakAlpha, t);
                surprise.color = color;
                elapsed += Time.deltaTime;
                yield return null;
            }
            color.a = peakAlpha;
            surprise.color = color;

            // 下降
            elapsed = 0f;
            while (elapsed < durationDown)
            {
                float t = elapsed / durationDown;
                color.a = Mathf.Lerp(peakAlpha, finalAlpha, t);
                surprise.color = color;
                elapsed += Time.deltaTime;
                yield return null;
            }
            color.a = finalAlpha;
            surprise.color = color;
        }
        surprise.enabled = false;

        yield return new WaitForSeconds(0.5f);
        Quaternion startRot = targetCamera.transform.rotation;
        Vector3 camEuler = targetCamera.transform.eulerAngles;
        targetCameraXAngle = -60f;
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

        if (spriteRenderer_3 != null && newSprite_3 != null)
        {
            spriteRenderer_3.sprite = newSprite_3;
            Color originalColor = spriteRenderer_3.color;
            originalColor.a = 1f;
            spriteRenderer_3.color = originalColor;

            // ---- フェードアウト処理 ----
            yield return new WaitForSeconds(0.5f);
            float fadeElapsed = 0f;
            bool aOne = false;
            while (fadeElapsed < fadeDuration)
            {
                float fadeT = fadeElapsed / fadeDuration;
                Color newColor = spriteRenderer_3.color;
                newColor.a = Mathf.Lerp(1f, 0f, fadeT);
                spriteRenderer_3.color = newColor;

                fadeElapsed += Time.deltaTime;
                
                if(fadeT < 0.3 && !aOne)
                {
                    // 最終的に完全に透明にする
                   
                    
                    anime3_ParticleSystem[0].Play();
                    anime3_ParticleSystem[1].Play();
                    anime3_ParticleSystem[2].Play();
                    aOne = true;
                    fadeElapsed = 0;
                    yield return null;
                }
                
            }

            Color finalColor = spriteRenderer_3.color;
            finalColor.a = 0f;
            spriteRenderer_3.color = finalColor;

        }
    }
}
