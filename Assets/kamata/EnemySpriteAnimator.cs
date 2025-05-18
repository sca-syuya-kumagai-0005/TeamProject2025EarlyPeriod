using UnityEngine;

public class EnemySpriteAnimator : MonoBehaviour
{
    [Header("�A�j���[�V�����p�X�v���C�g�ꗗ")]
    public Sprite[] animationFrames;

    [Header("1�����Ƃ̕\�����ԁi�b�j")]
    public float frameDuration = 0.1f;

    [Header("�Đ��J�n���ɃA�j���[�V�������邩")]
    public bool playOnStart = true;

    [Header("�ꎞ�\������X�v���C�g�i��: �o�����j")]
    public Sprite temporarySprite;

    [Header("�ꎞ��~����b��")]
    public float pauseDuration = 3.0f;

    [Header("�X�P�[���g���L���ɂ��邩")]
    public bool enableScaling = true;

    [Header("�g���̖ڕW�X�P�[��")]
    public Vector3 targetScale = new Vector3(2f, 2f, 1f);

    [Header("�g��ɂ�����b��")]
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
    /// �w�肳�ꂽ�^�O�̃I�u�W�F�N�g���o��������ꎞ��~����
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
    /// �X�P�[�������X�ɑ傫������
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