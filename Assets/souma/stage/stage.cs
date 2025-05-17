using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoopSwitcher : MonoBehaviour
{
    public string[] sceneNames; // 切り替えたいシーン名を配列で設定
    public float interval = 3f; // 秒数

    private int previousSceneIndex = -1; // 前回選ばれたシーンのインデックス（初期値は-1）
    private int currentSceneIndex = 0;

    void Start()
    {
        StartCoroutine(SwitchScenesLoop());
    }

    IEnumerator SwitchScenesLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            // ランダムにシーンを選ぶ（前回選ばれたシーンを避ける）
            do
            {
                currentSceneIndex = Random.Range(0, sceneNames.Length);
            } while (currentSceneIndex == previousSceneIndex);

            // 前回選ばれたシーンのインデックスを更新
            previousSceneIndex = currentSceneIndex;

            // シーンのロード
            SceneManager.LoadScene(sceneNames[currentSceneIndex]);
        }
    }

    void Awake()
    {
        if (FindObjectsOfType<SceneLoopSwitcher>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}
