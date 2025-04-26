using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SceneFade : MonoBehaviour
{
    [Header("�t�F�[�h�pImage")]
    public Image fadeImage;

    [Header("�V�[���J�n���̃t�F�[�h�ݒ�")]
    public bool fadeInOnStart = false;
    public bool fadeOutOnStart = false;

    [Header("���̃V�[���� (�t�F�[�h�A�E�g���̂ݎg�p)")]
    public string nextSceneName;

    [Header("�t�F�[�h�̏��v����(�b)")]
    public float fadeDuration = 1.0f;

    private bool isFading = false;

    void Start()
    {
        if (fadeInOnStart && fadeOutOnStart)
        {
            Debug.LogWarning("FadeInOnStart �� FadeOutOnStart ������ON�ɂȂ��Ă��܂��B�ǂ��炩1�ɂ��Ă��������B");
            return;
        }

        if (fadeInOnStart)
        {
            StartCoroutine(FadeIn());
        }
    }

    void Update()
    {
        // �N���b�N�Ńt�F�[�h�A�E�g���s
        if (Input.GetMouseButtonDown(0) && !isFading)
        {
            StartCoroutine(FadeOut(nextSceneName));
        }
    }

    public IEnumerator FadeIn()
    {
        isFading = true;

        float time = 0f;
        Color color = fadeImage.color;
        color.a = 1f;
        fadeImage.color = color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, time / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = 0f;
        fadeImage.color = color;
        isFading = false;
    }

    public IEnumerator FadeOut(string sceneName)
    {
        isFading = true;

        float time = 0f;
        Color color = fadeImage.color;
        color.a = 0f;
        fadeImage.color = color;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, time / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;
        isFading = false;

        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}