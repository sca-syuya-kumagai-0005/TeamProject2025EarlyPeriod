using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemySpriteAnimator : MonoBehaviour
{
    [Header("�A�j���[�V�����p�X�v���C�g�ꗗ")]
    public Sprite[] animationFrames;

    [Header("1�����Ƃ̕\�����ԁi�b�j")]
    public float frameDuration = 0.1f;

    [Header("�Đ��J�n���ɃA�j���[�V�������邩")]
    public bool playOnStart = true;

    [Header("����X�v���C�g�ꗗ�i�\�� & �����p�j")]
    public Sprite[] specialAnimationFrames;

    [Header("����\����1�������莞�ԁi�b�j")]
    public float specialFrameDuration = 0.1f;

    [Header("����X�v���C�g�̐������ԁi�b�j")]
    public float generatedLifetime = 3.0f;

    [Header("�ꎞ��~����b��")]
    public float pauseDuration = 3.0f;

    [Header("�X�P�[���g���L���ɂ��邩")]
    public bool enableScaling = true;

    [Header("�g���̖ڕW�X�P�[��")]
    public Vector3 targetScale = new Vector3(2f, 2f, 1f);

    [Header("�g��ɂ�����b��")]
    public float scaleDuration = 5.0f;

    [Header("�o�������m����Image��")]
    public string targetImageName = "FlashImage";

    private SpriteRenderer spriteRenderer;
    private int currentFrame = 0;
    private bool isPlaying = false;
    private bool isPaused = false;
    private bool isScalingPaused = false;
    private Coroutine scalingCoroutine;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

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

    private IEnumerator MonitorImage()
    {
        while (true)
        {
            GameObject foundImage = GameObject.Find(targetImageName);
            if (foundImage != null && foundImage.GetComponent<Image>() != null)
            {
                OnTargetImageDetected(foundImage.transform.position);
                yield return new WaitForSeconds(1f); // �ߌ��o�h�~
            }
            yield return null;
        }
    }

    private void OnTargetImageDetected(Vector3 spawnPosition)
    {
        StartCoroutine(PauseAndShowSpecialAnimation());

        if (specialAnimationFrames != null && specialAnimationFrames.Length > 0)
        {
            GameObject go = new GameObject("GeneratedSpecialSprite");
            go.transform.position = spawnPosition;

            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            EnemySpriteAnimator animator = go.AddComponent<EnemySpriteAnimator>();
            animator.animationFrames = specialAnimationFrames;
            animator.frameDuration = specialFrameDuration;
            animator.playOnStart = true;
            animator.enableScaling = false; // �������ꂽ���͊g�喳��

            // �����폜�p�X�N���v�g�ǉ�
            go.AddComponent<AutoDestroy>().lifetime = generatedLifetime;
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
}