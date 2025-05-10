using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFade : MonoBehaviour
{
    [Header("フェード用Image")]
    public Image fadeImage;

    [Header("次のシーン名 (フェードアウト時のみ使用)")]
    public string nextSceneName;

    [Header("フェードインの所要時間(秒)")]
    public float fadeInDuration = 1.0f;

    [Header("フェードアウトの所要時間(秒)")]
    public float fadeOutDuration = 1.0f;

    private bool isFading = false;

    void Start()
    {
        // シーン開始時にフェードインを実行
        StartCoroutine(FadeIn());
    }

    // ボタンから呼び出せる（フェードアウト→シーン遷移）
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