using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BookPageController : MonoBehaviour
{
    [Header("ページ画像を設定")]
    public Image pageImage;

    [Header("ページ一覧（Sprite）")]
    public List<Sprite> pages = new List<Sprite>();

    [Header("切り替え演出の時間")]
    public float fadeDuration = 0.3f;

    private int currentPage = 0;
    private bool isTransitioning = false;

    void Start()
    {
        if (pages.Count > 0)
        {
            pageImage.sprite = pages[0];
        }
    }

    public void NextPage()
    {
        if (isTransitioning || currentPage >= pages.Count - 1) return;
        StartCoroutine(FadeToPage(currentPage + 1));
    }

    public void PreviousPage()
    {
        if (isTransitioning || currentPage <= 0) return;
        StartCoroutine(FadeToPage(currentPage - 1));
    }

    IEnumerator FadeToPage(int newPage)
    {
        isTransitioning = true;

        // フェードアウト
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            pageImage.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }

        // ページ変更
        currentPage = newPage;
        pageImage.sprite = pages[currentPage];

        // フェードイン
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            pageImage.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }

        pageImage.color = Color.white;
        isTransitioning = false;
    }
}
