using UnityEngine;

public class EnemySpriteAnimator : MonoBehaviour
{
    [Header("アニメーション用スプライト一覧")]
    public Sprite[] animationFrames;

    [Header("1枚ごとの表示時間（秒）")]
    public float frameDuration = 0.1f;

    [Header("再生開始時にアニメーションするか")]
    public bool playOnStart = true;

    private SpriteRenderer spriteRenderer;
    private int currentFrame = 0;
    private bool isPlaying = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (playOnStart)
        {
            StartAnimation();
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
            spriteRenderer.sprite = animationFrames[currentFrame];
            currentFrame = (currentFrame + 1) % animationFrames.Length;
            yield return new WaitForSeconds(frameDuration);
        }
    }
}