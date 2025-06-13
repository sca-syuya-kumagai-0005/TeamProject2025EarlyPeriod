using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemySpriteAnimator : MonoBehaviour
{
    [Header("アニメーション用スプライト一覧")]
    public Sprite[] animationFrames;

    [Header("1枚ごとの表示時間（秒）")]
    public float frameDuration = 0.1f;

    [Header("再生開始時にアニメーションするか")]
    public bool playOnStart = true;

    [Header("特殊スプライト一覧（表示 & 生成用）")]
    public Sprite[] specialAnimationFrames;

    [Header("特殊表示の1枚あたり時間（秒）")]
    public float specialFrameDuration = 0.1f;

    [Header("特殊スプライトの生存時間（秒）")]
    public float generatedLifetime = 3.0f;

    [Header("一時停止する秒数（カメラ外）")]
    public float pauseDuration = 3.0f;

    [Header("スケール拡大を有効にするか")]
    public bool enableScaling = true;

    [Header("拡大後の目標スケール")]
    public Vector3 targetScale = new Vector3(2f, 2f, 1f);

    [Header("拡大にかける秒数")]
    public float scaleDuration = 5.0f;

    [Header("フラッシュのImage名")]
    public string targetImageName = "FlashImage";

    [Header("カメラフレームのCollider2D")]
    public Collider2D cameraFrameCollider;

    private SpriteRenderer spriteRenderer;
    private int currentFrame = 0;
    private bool isPlaying = false;
    private bool isPaused = false;
    private bool isScalingPaused = false;
    private Coroutine scalingCoroutine;
    private bool isLockedByCameraFrame = false;
    public bool IsScalingPaused {  get { return isScalingPaused; } }
    

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // ✅ 最初にアルファ値 0.8 に設定
        Color c = spriteRenderer.color;
        c.a = 0.8f;
        spriteRenderer.color = c;

        if (playOnStart)
        {
            StartAnimation();
        }

        if (enableScaling)
        {
            scalingCoroutine = StartCoroutine(ScaleOverTime(targetScale, scaleDuration));
        }

        StartCoroutine(MonitorImage());
    }
    private void Update()
    {
        if (!this.gameObject.GetComponent<Collider2D>().enabled)
        {
            Debug.Log("通過");
            isScalingPaused = true;
        }
    }
    private IEnumerator MonitorImage()
    {
        while (true)
        {
            GameObject flash = GameObject.Find(targetImageName);
            if (flash != null && flash.GetComponent<Image>() != null)
            {
                bool insideCameraFrame = IsInsideCameraFrame();
                if (insideCameraFrame)
                {
                    Debug.Log($"[Enemy] フラッシュ検知 + カメラフレーム内 → 完全停止: {name}");
                    LockByCameraFrame();
                }
                else
                {
                    Debug.Log($"[Enemy] フラッシュ検知 + カメラフレーム外 → 一時停止: {name}");
                    StartCoroutine(PauseAndShowSpecialAnimation());
                }

                //特殊スプライト生成（アルファ値もコピー）
                //if (specialAnimationFrames != null && specialAnimationFrames.Length > 0)
                //{
                //    GameObject go = new GameObject("GeneratedSpecialSprite");
                //    go.transform.position = transform.position;

                //    SpriteRenderer sr = go.AddComponent<SpriteRenderer>();

                //    //ここでアルファ値を0.8にする
                //    Color color = sr.color;
                //    color.a = 0.8f;
                //    sr.color = color;

                //    //EnemySpriteAnimator animator = go.AddComponent<EnemySpriteAnimator>();
                //    //animator.animationFrames = specialAnimationFrames;
                //    //animator.frameDuration = specialFrameDuration;
                //    //animator.playOnStart = true;
                //    //animator.enableScaling = false;

                //    go.transform.localScale = transform.localScale;

                //    go.AddComponent<AutoDestroy>().lifetime = generatedLifetime;
                //}

                yield return new WaitForSeconds(3f); // フラッシュ検知の連続実行防止
                specialAnimationFrames=new Sprite[0];
            }

            yield return null;
        }
    }

    private bool IsInsideCameraFrame()
    {
        if (cameraFrameCollider == null) return false;
        Vector2 myPos = transform.position;
        return cameraFrameCollider.OverlapPoint(myPos);
    }

    public void StartAnimation()
    {
        if (!isPlaying && animationFrames.Length > 0)
        {
            isPlaying = true;
            StartCoroutine(PlayAnimation());
        }
    }

    public void StopAnimation()
    {
        isPlaying = false;
        StopAllCoroutines();
    }

    private IEnumerator PlayAnimation()
    {
        while (isPlaying)
        {
            if (!isPaused)
            {
                spriteRenderer.sprite = animationFrames[currentFrame];
                currentFrame = (currentFrame + 1) % animationFrames.Length;
            }
            yield return new WaitForSeconds(frameDuration);
        }
    }

    private IEnumerator PauseAndShowSpecialAnimation()
    {
        isPaused = true;
        isScalingPaused = true;

        Sprite originalSprite = spriteRenderer.sprite;

        if (specialAnimationFrames != null && specialAnimationFrames.Length > 0)
        {
            for (int i = 0; i < specialAnimationFrames.Length; i++)
            {
                spriteRenderer.sprite = specialAnimationFrames[i];
                yield return new WaitForSeconds(specialFrameDuration);
            }
        }

        yield return new WaitForSeconds(pauseDuration);

        spriteRenderer.sprite = originalSprite;
        isPaused = false;
        isScalingPaused = false;
    }

    private IEnumerator ScaleOverTime(Vector3 target, float duration)
    {
        Vector3 initialScale = transform.localScale;
        float time = 0f;

        while (time < duration)
        {
            if (!isScalingPaused)
            {
                time += Time.deltaTime;
                transform.localScale = Vector3.Lerp(initialScale, target, time / duration);
            }
            yield return null;
        }

        transform.localScale = target;
    }

    public void LockByCameraFrame()
    {
        if (isLockedByCameraFrame) return;

        isLockedByCameraFrame = true;
        isPaused = true;
        isScalingPaused = true;
        StopAllCoroutines();
        StartCoroutine(PlaySpecialAnimationLoop());
    }

    private IEnumerator PlaySpecialAnimationLoop()
    {
        int frame = 0;
        while (true)
        {
            if (specialAnimationFrames.Length > 0)
            {
                spriteRenderer.sprite = specialAnimationFrames[frame];
                frame = (frame + 1) % specialAnimationFrames.Length;
            }
            yield return new WaitForSeconds(specialFrameDuration);
        }
    }

    public bool IsLockedByCameraFrame()
    {
        return isLockedByCameraFrame;
    }
}