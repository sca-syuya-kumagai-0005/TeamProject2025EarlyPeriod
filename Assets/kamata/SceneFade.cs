using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SceneFade : MonoBehaviour
{
    [Header("�t�F�[�h�pImage")]
    public Image fadeImage;

    [Header("���̃V�[���� (�t�F�[�h�A�E�g���̂ݎg�p)")]
    public string nextSceneName;

    [Header("�t�F�[�h�̏��v����(�b)")]
    public float fadeDuration = 1.0f;

    private bool isFading = false;

    void Update()
    {
        // �}�E�X���N���b�N�Ńt�F�[�h�A�E�g���s
        if (Input.GetMouseButtonDown(0) && !isFading)
        {
            StartCoroutine(FadeOut(nextSceneName));
        }
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