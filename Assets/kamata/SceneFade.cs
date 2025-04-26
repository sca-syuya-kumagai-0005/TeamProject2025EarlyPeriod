using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneFade : MonoBehaviour
{
    [Header("Scene Settings")]
    public string targetSceneName;
    public float delaySeconds = 1f;

    [Header("Fade Settings")]
    public bool useFade = true;
    public float fadeDuration = 1f;
    public Image fadeImage; // ���w�i��Image�iAlpha=0�̏�ԁj

    private bool isSwitching = false;

    void Start()
    {
        if (useFade && fadeImage != null)
        {
            // �ŏ��Ƀt�F�[�h�C���i��ʕ\���j
            StartCoroutine(Fade(1, 0));
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isSwitching)
        {
            StartCoroutine(SwitchScene());
        }
    }

    IEnumerator SwitchScene()
    {
        isSwitching = true;

        yield return new WaitForSeconds(delaySeconds);

        if (useFade && fadeImage != null)
        {
            yield return StartCoroutine(Fade(0, 1)); // �t�F�[�h�A�E�g
        }

        SceneManager.LoadScene(targetSceneName);
    }

    IEnumerator Fade(float fromAlpha, float toAlpha)
    {
        float t = 0f;
        Color color = fadeImage.color;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(fromAlpha, toAlpha, t / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, a);
            yield return null;
        }

        // ���S�Ɏw�肳�ꂽ�A���t�@�ɐݒ�i�␳�j
        fadeImage.color = new Color(color.r, color.g, color.b, toAlpha);
    }
}