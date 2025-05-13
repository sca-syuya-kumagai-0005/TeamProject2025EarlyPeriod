using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScaleToSceneChange : MonoBehaviour
{
    [Header("対象のImage")]
    public Image targetImage;

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
        if (targetImage == null || string.IsNullOrEmpty(nextSceneName)) return;

        Vector3 currentScale = targetImage.rectTransform.localScale;

        // 誤差範囲を含めてスケールを比較
        if (Vector3.Distance(currentScale, targetScale) <= tolerance)
        {
            hasSceneChanged = true;
            SceneManager.LoadScene(nextSceneName);
        }
    }
}