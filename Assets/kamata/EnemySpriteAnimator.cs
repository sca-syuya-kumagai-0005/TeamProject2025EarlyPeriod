using UnityEngine;

public class EnemySpriteAnimator : MonoBehaviour
{
    [Header("�A�j���[�V�����p�X�v���C�g�ꗗ")]
    public Sprite[] animationFrames;

    [Header("1�����Ƃ̕\�����ԁi�b�j")]
    public float frameDuration = 0.1f;

    [Header("�Đ��J�n���ɃA�j���[�V�������邩")]
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