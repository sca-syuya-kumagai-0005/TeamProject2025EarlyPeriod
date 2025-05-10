using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFade : MonoBehaviour
{
    [Header("�t�F�[�h�pImage")]
    public Image fadeImage;

    [Header("���̃V�[���� (�t�F�[�h�A�E�g���̂ݎg�p)")]
    public string nextSceneName;

    [Header("�t�F�[�h�C���̏��v����(�b)")]
    public float fadeInDuration = 1.0f;

    [Header("�t�F�[�h�A�E�g�̏��v����(�b)")]
    public float fadeOutDuration = 1.0f;

    [Header("�t�F�[�h�A�E�g�p�̃{�^�� (�h���b�O���h���b�v�Őݒ�)")]
    public Button fadeButton;

    private bool isFading = false;

    void Start()
    {
        // �t�F�[�h�C�����s
        StartCoroutine(FadeIn());

        // �{�^�����ݒ肳��Ă���΁A�C�x���g��o�^
        if (fadeButton != null)
        {
            fadeButton.onClick.AddListener(FadeOutStart);
        }
    }

    public void FadeOutStart()
    {
        if (!isFading)
        {
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeIn()
    {
        isFading = true;

        float time = 0f;
        Color color = fadeImage.color;
        color.a = 1f;
        fadeImage.color = color;

        while (time < fadeInDuration)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, time / fadeInDuration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = 0f;
        fadeImage.color = color;
        isFading = false;
    }

    private IEnumerator FadeOut()
    {
        isFading = true;

        float time = 0f;
        Color color = fadeImage.color;
        color.a = 0f;
        fadeImage.color = color;

        while (time < fadeOutDuration)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, time / fadeOutDuration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}