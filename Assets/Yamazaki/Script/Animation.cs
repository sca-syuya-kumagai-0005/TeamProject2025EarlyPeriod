using UnityEngine;
using System.Collections;
public class Animation : MonoBehaviour
{
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

    //�A�j���[�V����2�Ŏg�p����
    [SerializeField] private Material noise_Mat;

    [Header("�X�v���C�g���t�F�[�h����b��")]
    public float fadeDuration = 1f;



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
        for(int i= 0;i < titleAnimeObj.Length;i++)
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
            default:
                Debug.Log("�A�j���i���o�[���Ȃ���");
                break;
        }
    }

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
        yield return new WaitForSeconds(1f);
        for (int i= 0; i<2;i++)
        {
            
            noise_Mat.SetFloat("_Alpha", 0.4f);
            yield return new WaitForSeconds(0.1f);
            noise_Mat.SetFloat("_Alpha", 0f);
            yield return new WaitForSeconds(0.3f);
        }
        noise_Mat.SetFloat("_Alpha", 0.4f);
        yield return new WaitForSeconds(0.1f);
        noise_Mat.SetFloat("_Alpha", 1f);
        yield return new WaitForSeconds(1f);
        noise_Mat.SetFloat("_Alpha", 0.4f);
        yield return new WaitForSeconds(0.2f);
        noise_Mat.SetFloat("_Alpha", 0.8f);
        yield return new WaitForSeconds(0.2f);
        noise_Mat.SetFloat("_Alpha", 1.2f);
        yield return new WaitForSeconds(0.7f);
        yield return null; 
    }
}
