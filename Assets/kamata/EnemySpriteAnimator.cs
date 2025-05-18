using UnityEngine;

public class EnemySpriteAnimator : MonoBehaviour
{
    [Header("アニメーション用スプライト一覧")]
    public Sprite[] animationFrames;

    [Header("1枚ごとの表示時間（秒）")]
    public float frameDuration = 0.1f;

    [Header("再生開始時にアニメーションするか")]
    public bool playOnStart = true;

    [Header("一時表示するスプライト（例: 出現時）")]
    public Sprite temporarySprite;

    [Header("一時停止する秒数")]
    public float pauseDuration = 3.0f;

    [Header("スケール拡大を有効にするか")]
    public bool enableScaling = true;

    [Header("拡大後の目標スケール")]
    public Vector3 targetScale = new Vector3(2f, 2f, 1f);

    [Header("拡大にかける秒数")]
    public float scaleDuration = 5.0f;

    private SpriteRenderer spriteRenderer;
    private int currentFrame = 0;
    private bool isPlaying = false;
    private bool isPaused = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (playOnStart)
        {
            StartAnimation();
        }

        if (enableScaling)
        {
            StartScaling();
        }
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

    private System.Collections.IEnumerator PlayAnimation()
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

    /// <summary>
    /// 指定されたタグのオブジェクトが出現したら一時停止処理
    /// </summary>
    public void OnTaggedObjectAppeared()
    {
        StartCoroutine(PauseAndShowTemporarySprite());
    }

    private System.Collections.IEnumerator PauseAndShowTemporarySprite()
    {
        isPaused = true;

        Sprite originalSprite = spriteRenderer.sprite;
        if (temporarySprite != null)
        {
            spriteRenderer.sprite = temporarySprite;
        }

        yield return new WaitForSeconds(pauseDuration);

        spriteRenderer.sprite = originalSprite;
        isPaused = false;
    }

    /// <summary>
    /// スケールを徐々に大きくする
    /// </summary>
    public void StartScaling()
    {
        StopCoroutine("ScaleOverTime");
        StartCoroutine(ScaleOverTime(targetScale, scaleDuration));
    }

    private System.Collections.IEnumerator ScaleOverTime(Vector3 target, float duration)
    {
        Vector3 initialScale = transform.localScale;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            transform.localScale = Vector3.Lerp(initialScale, target, time / duration);
            yield return null;
        }

        transform.localScale = target;
    }
}