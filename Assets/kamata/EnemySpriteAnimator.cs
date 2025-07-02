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

    [Header("特殊スプライト一覧（差し替え表示用）")]
    public Sprite[] specialAnimationFrames;

    [Header("特殊表示の1枚あたり時間（秒）")]
    public float specialFrameDuration = 0.1f;

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

    [Header("カメラフレームのオブジェクト名（任意）")]
    public string cameraFrameColliderName = "";

    private SpriteRenderer spriteRenderer;
    private int currentFrame = 0;
    private bool isPlaying = false;
    private bool isPaused = false;
    private bool isScalingPaused = false;
    private Coroutine scalingCoroutine;
    private bool isLockedByCameraFrame = false;
    private bool isSpecialAnimationRunning = false;

    public bool IsScalingPaused { get { return isScalingPaused; } }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        Color c = spriteRenderer.color;
        c.a = 0.8f;
        spriteRenderer.color = c;

        // 名前からCameraFrameのCollider2Dを取得（任意設定）
        if (cameraFrameCollider == null && !string.IsNullOrEmpty(cameraFrameColliderName))
        {
            GameObject obj = GameObject.Find(cameraFrameColliderName);
            if (obj != null)
            {
                Collider2D col = obj.GetComponent<Collider2D>();
                if (col != null)
                {
                    cameraFrameCollider = col;
                }
                else
                {
                    Debug.LogWarning($"[{name}] 指定されたオブジェクト '{cameraFrameColliderName}' に Collider2D がありません。");
                }
            }
            else
            {
                Debug.LogWarning($"[{name}] オブジェクト名 '{cameraFrameColliderName}' が見つかりませんでした。");
            }
        }

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

    void Update()
    {
        if (!GetComponent<Collider2D>().enabled)
        {
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
                    if (!isSpecialAnimationRunning)
                    {
                        StartCoroutine(PauseAndShowSpecialAnimation());
                    }
                }

                yield return new WaitForSeconds(3f); // クールタイム
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
        isSpecialAnimationRunning = true;
        isPaused = true;
        isScalingPaused = true;

        Sprite originalSprite = spriteRenderer.sprite;

        if (specialAnimationFrames != null && specialAnimationFrames.Length > 0)
        {
            foreach (var specialSprite in specialAnimationFrames)
            {
                spriteRenderer.sprite = specialSprite;
                yield return new WaitForSeconds(specialFrameDuration);
            }
        }

        yield return new WaitForSeconds(pauseDuration);

        spriteRenderer.sprite = originalSprite;
        isPaused = false;
        isScalingPaused = false;
        isSpecialAnimationRunning = false;
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

        StopAllCoroutines(); // 通常アニメや拡大などを停止
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

    public void SetSpecialFrames(Sprite[] newFrames)
    {
        specialAnimationFrames = newFrames;
    }

    public bool IsLockedByCameraFrame()
    {
        return isLockedByCameraFrame;
    }
}
