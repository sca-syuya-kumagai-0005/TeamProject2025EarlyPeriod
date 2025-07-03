using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneLoopSwitcher : MonoBehaviour
{
    [Header("メインシーンの名前リスト（Build Settingsに含める）")]
    public string[] sceneNames;

    [Header("Doorシーン名（必ずBuild Settingsに含める）")]
    public string doorSceneName = "DoorScene1";

    [Header("全シーン再生後に遷移するシーン名")]
    public string finalSceneName = "Result";

    private int currentSceneIndex = 0;
    private int previousSceneIndex = -1;

    private bool goToNextScene = false;
    private bool sceneChangeRequested = false;

    private static SceneLoopSwitcher instance;

    // 再生済みのシーンインデックス一覧
    private List<int> playedSceneIndices = new List<int>();

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        // Canvas の重複防止（任意）
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Canvas");
        if (objs.Length > 1)
        {
            for (int i = 1; i < objs.Length; i++)
            {
                Destroy(objs[i]);
            }
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name != doorSceneName)
        {
            SceneManager.LoadScene(doorSceneName);
        }
    }

    void Update()
    {
        // Doorへ移行
        if (goToNextScene)
        {
            goToNextScene = false;

            if (SceneManager.GetActiveScene().name != doorSceneName)
            {
                SceneManager.LoadScene(doorSceneName);
            }
        }

        // メインシーン遷移要求
        if (sceneChangeRequested)
        {
            sceneChangeRequested = false;

            // 全シーン再生済みなら FinalScene へ
            if (playedSceneIndices.Count >= sceneNames.Length)
            {
                SceneManager.LoadScene(finalSceneName);
                return;
            }

            // 再生されていないシーンのインデックスを集める
            List<int> availableIndices = new List<int>();
            for (int i = 0; i < sceneNames.Length; i++)
            {
                if (!playedSceneIndices.Contains(i))
                {
                    availableIndices.Add(i);
                }
            }

            // ランダムに未再生シーンを選択
            if (availableIndices.Count > 0)
            {
                do
                {
                    int randomPick = Random.Range(0, availableIndices.Count);
                    currentSceneIndex = availableIndices[randomPick];
                } while (sceneNames.Length > 1 && currentSceneIndex == previousSceneIndex);

                previousSceneIndex = currentSceneIndex;
                playedSceneIndices.Add(currentSceneIndex);

                SceneManager.LoadScene(sceneNames[currentSceneIndex]);
            }
            else
            {
                // 念のため（到達しない想定）
                Debug.LogWarning("再生可能なシーンが見つかりません");
            }
        }
    }

    // Door シーン側から呼ばれる：メインシーンに移動するトリガー
    public static void RequestSceneChange()
    {
        if (instance != null)
        {
            instance.sceneChangeRequested = true;
        }
        else
        {
            Debug.LogWarning("SceneLoopSwitcher: インスタンスが存在しません。");
        }
    }

    // 外部スクリプト（タイマーなど）から Door シーンへ移動
    public static void TriggerNextScene()
    {
        if (instance != null)
        {
            instance.goToNextScene = true;
        }
        else
        {
            Debug.LogWarning("SceneLoopSwitcher: TriggerNextScene に失敗。インスタンスが見つかりません。");
        }
    }
}
