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

    [Header("最初だけ挟むチュートリアルシーン名")]
    public string tutorialSceneName = "TutorialScene";

    private int currentSceneIndex = 0;
    private int previousSceneIndex = -1;

    private bool goToNextScene = false;
    private bool sceneChangeRequested = false;
    private bool hasShownTutorial = false;

    private bool goToFinalSceneNext = false;      // 全シーン再生後に Result へ向かうフラグ
    private bool pendingFinalTransition = false;  // Result への移行を Door 経由で待機中

    private static SceneLoopSwitcher instance;

    [SerializeField] private SpawnManager spawnManager;
    public static SpawnManager manager;

    private List<int> playedSceneIndices = new List<int>();

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        // Canvas の重複防止（任意）
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Canvas");
        if (objs.Length > 1)
        {
            for (int i = 1; i < objs.Length; i++)
            {
                Destroy(objs[i]);
            }
        }
    }

    void Start()
    {
        manager = spawnManager;

        if (SceneManager.GetActiveScene().name != doorSceneName)
        {
            SceneManager.LoadScene(doorSceneName);
        }
    }

    void Update()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        // Door → Tutorial or MainScene
        if (goToNextScene)
        {
            goToNextScene = false;
            if (currentScene != doorSceneName)
            {
                SceneManager.LoadScene(doorSceneName);
            }
        }

        // Door → 次のシーンへ遷移
        if (sceneChangeRequested)
        {
            sceneChangeRequested = false;

            if (!hasShownTutorial && !string.IsNullOrEmpty(tutorialSceneName))
            {
                hasShownTutorial = true;
                SceneManager.LoadScene(tutorialSceneName);
            }
            else if (goToFinalSceneNext)
            {
                goToFinalSceneNext = false;
                pendingFinalTransition = true;
                SceneManager.LoadScene(doorSceneName); // 最後に Door 挟む
            }
            else
            {
                LoadNextMainScene();
            }
        }

        // Door 経由で Result へ移行する
        if (pendingFinalTransition && currentScene == doorSceneName)
        {
            pendingFinalTransition = false;
            SceneManager.LoadScene(finalSceneName);
        }
    }

    // メインシーンから未再生を1つ選びロード
    private void LoadNextMainScene()
    {
        if (sceneNames == null || sceneNames.Length == 0)
        {
            Debug.LogWarning("SceneLoopSwitcher: sceneNames が設定されていません。");
            return;
        }

        // 未再生のインデックスをリスト化
        List<int> remaining = new List<int>();
        for (int i = 0; i < sceneNames.Length; i++)
        {
            if (!playedSceneIndices.Contains(i)) remaining.Add(i);
        }

        if (remaining.Count == 0)
        {
            // 全部再生済み → Door → Result
            goToFinalSceneNext = true;
            sceneChangeRequested = true;
            return;
        }

        // ランダムに未再生のシーンを選ぶ
        int newIndex = remaining[Random.Range(0, remaining.Count)];
        currentSceneIndex = newIndex;
        previousSceneIndex = newIndex;
        playedSceneIndices.Add(newIndex);

        SceneManager.LoadScene(sceneNames[currentSceneIndex]);
    }

    // Door から次のシーンへ進める
    public static void RequestSceneChange()
    {
        if (instance != null)
        {
            instance.sceneChangeRequested = true;
        }
        else
        {
            Debug.LogWarning("SceneLoopSwitcher: RequestSceneChange 呼び出しに失敗しました。");
        }
    }

    // Tutorial から MainScene に進める
    public static void EndTutorialAndProceed()
    {
        if (instance != null)
        {
            instance.hasShownTutorial = true;
            instance.sceneChangeRequested = true;
        }
        else
        {
            Debug.LogWarning("SceneLoopSwitcher: EndTutorialAndProceed 呼び出しに失敗しました。");
        }
    }

    // Door または Result を外部から指定遷移
    public static void TriggerNextScene(bool goToDoor)
    {
        if (instance == null)
        {
            Debug.LogWarning("SceneLoopSwitcher: TriggerNextScene 呼び出しに失敗しました。");
            return;
        }

        if (goToDoor)
        {
            SceneManager.LoadScene(instance.doorSceneName);
        }
        else
        {
            SceneManager.LoadScene(instance.finalSceneName);
        }
    }

    // Tutorial → MainScene 遷移の存在確認用
    public static bool InstanceExists()
    {
        return instance != null;
    }
}
