using UnityEngine;
using System.Collections;
public class Animation : MonoBehaviour
{
    //�X�i�b�v�V���b�g�Ŏg�p
    [Header("�ړ��J�n�I�t�Z�b�g�i���[�J�����W�j")]
    [SerializeField] private Vector3 startOffset = new Vector3(25f, 0, 0);
    [SerializeField] private GameObject snapShot;
    [Header("���o�ɂ�����b��")]
    [SerializeField] private float duration_Phot = 1.0f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private SpriteRenderer spriteRenderer_Phot;

    //�A�j���[�V����1�Ŏ��p����
    [Header("Canvas���UI�I�u�W�F�N�g")]
    [SerializeField] private RectTransform uiObject;
    [Header("�ړI�ʒu (UI�̃��[�J�����W)")]
    [SerializeField] private Vector3 targetUIPosition;
    [Header("���C���J����")]
    [SerializeField] private Camera targetCamera;
    [Header("�J�����̖ڕWX��]�p")]
    [SerializeField] private float targetCameraXAngle = 30f;
    [Header("�ړ��ɂ����鎞��")]
    [SerializeField] private float duration = 1.5f;
    [Header("�Ō�ɐ؂�ւ���X�v���C�g")]
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite newSprite;
    [SerializeField] ParticleSystem anime1_ParticleSystem;
    //�A�j���[�V����2�Ŏg�p����
    [SerializeField] private Material noise_Mat;
    [SerializeField] private SpriteRenderer backGround_anim2;

    [Header("�X�v���C�g���t�F�[�h����b��")]
    private float fadeDuration = 1f;

    //�A�j���[�V�����R�Ŏg�p����
    [Header("�������o")]
    [SerializeField]private SpriteRenderer surprise;
    [Header("�ʐ^��ɏo�����邨����")]
    [SerializeField] private SpriteRenderer onPhotGhost;
    [Header("�㏸��̍ő�A���t�@�l")]
    [SerializeField] private float peakAlpha = 1f;
    [Header("�ŏI�I�ɉ�����A���t�@�l")]
    [SerializeField] private float finalAlpha = 0.4f;
    [Header("�㏸�E���~�ɂ�����b��")]
    [SerializeField] private float durationUp = 1f;
    [SerializeField] private float durationDown = 1f;
    [Header("�Ō�ɐ؂�ւ���X�v���C�g")]
    [SerializeField] SpriteRenderer spriteRenderer_3;
    [SerializeField] Sprite newSprite_3;
    [Header("����������j�̉��o")]
    [SerializeField] ParticleSystem[] anime3_ParticleSystem;


    [Header("�A�j���[�V�����̐e�i�[(�g�p���\��)")]
    [SerializeField] GameObject[] titleAnimeObj;

    int anime_num;//�A�j���[�V�����̐�
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
                Debug.Log("�A�j���i���o�[���Ȃ���");
                break;
        }
    }
    /*
    public IEnumerator SlideAndFadeIn()
    {
        Debug.Log("�ʐ^�ړ�");
        float elapsed = 0f;

        while (elapsed < duration_Phot)
        {
            float t = elapsed / duration_Phot;

            // �ʒu����
            snapShot.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);

            // ������
            Color color = spriteRenderer_Phot.color;
            color.a = Mathf.Lerp(0f, 1f, t);
            spriteRenderer_Phot.color = color;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // �ŏI�l�ɌŒ�
        snapShot.transform.localPosition = targetPosition;
        Color finalColor = spriteRenderer_Phot.color;
        finalColor.a = 1f;
        spriteRenderer_Phot.color = finalColor;
        anime_num = 3;
    }*/

    private IEnumerator Anime_1()
    {
        yield return new WaitForSeconds(0.5f);
        // ---- UI�ړ� ----
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
        // ---- �J������] ----
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
        // ---- �X�v���C�g�����ւ� ----
        if (spriteRenderer != null && newSprite != null)
        {
            spriteRenderer.sprite = newSprite;
            Color originalColor = spriteRenderer.color;
            originalColor.a = 1f;
            spriteRenderer.color = originalColor;

            // ---- �t�F�[�h�A�E�g���� ----
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
            
            // �ŏI�I�Ɋ��S�ɓ����ɂ���
            Color finalColor = spriteRenderer.color;
            finalColor.a = 0f;
            spriteRenderer.color = finalColor;
        }
        Debug.Log("���J�A�j���[�V����");
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
            // �㏸
            
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

            // ���~
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

            // ---- �t�F�[�h�A�E�g���� ----
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
                    // �ŏI�I�Ɋ��S�ɓ����ɂ���
                   
                    
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
