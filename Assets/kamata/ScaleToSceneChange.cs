using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MultiScaleToSceneChange : MonoBehaviour
{
    [Header("対象のImageリスト")]
    public Image[] targetImages;

    [Header("遷移先シーン名")]
    public string nextSceneName;

    [Header("スケールのしきい値")]
    public Vector3 targetScale = new Vector3(1.5f, 1.5f, 1.5f);

    [Header("スケールの判定誤差許容範囲")]
    public float tolerance = 0.01f;

    private bool hasSceneChanged = false;

    void Update()
    {
        if (hasSceneChanged) return;
        if (targetImages == null || targetImages.Length == 0 || string.IsNullOrEmpty(nextSceneName)) return;

        // 全てのImageが目標スケールに達しているかチェック
        foreach (Image img in targetImages)
        {
            if (img == null) continue;

            Vector3 currentScale = img.rectTransform.localScale;
            if (Vector3.Distance(currentScale, targetScale) > tolerance)
            {
                return; // 1つでも未達成があれば抜ける
            }
        }

        // 全て達成していたらシーン遷移
        hasSceneChanged = true;
        SceneManager.LoadScene(nextSceneName);
    }
}