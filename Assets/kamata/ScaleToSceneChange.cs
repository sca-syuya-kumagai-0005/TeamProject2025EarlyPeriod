using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiScaleToSceneChange : MonoBehaviour
{
    [Header("対象のSpriteRendererリスト")]
    public SpriteRenderer[] targetSprites;

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
        if (targetSprites == null || targetSprites.Length == 0 || string.IsNullOrEmpty(nextSceneName)) return;

        foreach (SpriteRenderer sr in targetSprites)
        {
            if (sr == null) continue;
            Debug.Log(targetSprites);
            Vector3 currentScale = sr.transform.localScale;

            if (Vector3.Distance(currentScale, targetScale) > tolerance)
            {
                return; // 1つでも未達成ならシーン切り替えしない
            }
        }

        // すべてスケール到達 → シーン切り替え
        hasSceneChanged = true;
        SceneManager.LoadScene(nextSceneName);
    }
}