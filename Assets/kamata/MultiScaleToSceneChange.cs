using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiScaleToSceneChange : MonoBehaviour
{
    [Header("エネミータグ")]
    public string enemyTag = "Enemy";

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
        if (string.IsNullOrEmpty(nextSceneName)) return;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        if (enemies.Length == 0) return;

        foreach (GameObject enemy in enemies)
        {
            if (enemy == null) continue;

            Vector3 currentScale = enemy.transform.localScale;

            if (Vector3.Distance(currentScale, targetScale) > tolerance)
            {
                return; // 1つでもスケール未到達 → シーン切り替えしない
            }
        }

        // すべてスケール到達 → シーン切り替え
        hasSceneChanged = true;
        SceneManager.LoadScene(nextSceneName);
    }
}
