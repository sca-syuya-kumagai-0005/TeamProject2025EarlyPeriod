using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SceneFade : MonoBehaviour
{
    [Header("フェード用Image")]
    public Image fadeImage;

    [Header("シーン開始時のフェード設定")]
    public bool fadeInOnStart = false;
    public bool fadeOutOnStart = false;

    [Header("次のシーン名 (フェードアウト時のみ使用)")]
    public string nextSceneName;

    [Header("フェードの所要時間(秒)")]
    public float fadeDuration = 1.0f;

    private bool isFading = false;

    void Start()
    {
        if (fadeInOnStart && fadeOutOnStart)
        {
            Debug.LogWarning("FadeInOnStart と FadeOutOnStart が両方ONになっています。どちらか1つにしてください。");
            return;
        }

        if (fadeInOnStart)
        {
            StartCoroutine(FadeIn());
        }
    }

    void Update()
    {
        // クリックでフェードアウト実行
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