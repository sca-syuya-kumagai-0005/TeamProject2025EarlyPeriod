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

    [Header("�ꎞ��~����b���i�J�����O�j")]
    public float pauseDuration = 3.0f;

    [Header("�X�P�[���g���L���ɂ��邩")]
    public bool enableScaling = true;

    [Header("�g���̖ڕW�X�P�[��")]
    public Vector3 targetScale = new Vector3(2f, 2f, 1f);

    [Header("�g��ɂ�����b��")]
    public float scaleDuration = 5.0f;

    [Header("�t���b�V����Image��")]
    public string targetImageName = "FlashImage";

    [Header("�J�����t���[����Collider2D")]
    public Collider2D cameraFrameCollider;

    private SpriteRenderer spriteRenderer;
    private int currentFrame = 0;
    private bool isPlaying = false;
    private bool isPaused = false;
    private bool isScalingPaused = false;
    private Coroutine scalingCoroutine;

    private bool isLockedByCameraFrame = false;

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
            GameObject flash = GameObject.Find(targetImageName);
            if (flash != null && flash.GetComponent<Image>() != null)
            {
                bool insideCameraFrame = IsInsideCameraFrame();

                if (insideCameraFrame)
                {
                    Debug.Log($"[Enemy] �t���b�V�����m + �J�����t���[���� �� ���S��~: {name}");
                    LockByCameraFrame();
                }
                else
                {
                    Debug.Log($"[Enemy] �t���b�V�����m + �J�����t���[���O �� �ꎞ��~: {name}");
                    StartCoroutine(PauseAndShowSpecialAnimation());
                }

                // ����G�t�F�N�g����
                if (specialAnimationFrames != null && specialAnimationFrames.Length > 0)
                {
                    GameObject go = new GameObject("GeneratedSpecialSprite");
                    go.transform.position = transform.position;

                    SpriteRenderer sr = go.AddComponent<SpriteRenderer>();

                    EnemySpriteAnimator animator = go.AddComponent<EnemySpriteAnimator>();
                    animator.animationFrames = specialAnimationFrames;
                    animator.frameDuration = specialFrameDuration;
                    animator.playOnStart = true;
                    animator.enableScaling = false;

                    // ���� �����ŃX�P�[�����R�s�[���Ċg��\��
                    go.transform.localScale = transform.localScale;

                    go.AddComponent<AutoDestroy>().lifetime = generatedLifetime;
                }

                yield return new WaitForSeconds(1f); // �ߏ茟�o�}��
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
