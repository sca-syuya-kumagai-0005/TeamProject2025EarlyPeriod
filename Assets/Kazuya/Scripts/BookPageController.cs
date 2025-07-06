using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BookPageController : MonoBehaviour
{
    [Header("�y�[�W�摜��ݒ�")]
    public Image pageImage;

    [Header("�y�[�W�ꗗ�iSprite�j")]
    public List<Sprite> pages = new List<Sprite>();

    [Header("�؂�ւ����o�̎���")]
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

        // �t�F�[�h�A�E�g
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            pageImage.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }

        // �y�[�W�ύX
        currentPage = newPage;
        pageImage.sprite = pages[currentPage];

        // �t�F�[�h�C��
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
