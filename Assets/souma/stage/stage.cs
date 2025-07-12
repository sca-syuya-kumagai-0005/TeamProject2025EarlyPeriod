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

    private bool goToFinalSceneNext = false;
    private bool pendingFinalTransition = false;

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

        // リザルト到達時に状態リセット
        if (currentScene == finalSceneName)
        {
            ResetLoopState();
        }

        if (goToNextScene)
        {
            goToNextScene = false;
            if (currentScene != doorSceneName)
            {
                SceneManager.LoadScene(doorSceneName);
            }
        }

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
                SceneManager.LoadScene(doorSceneName);
            }
            else
            {
                LoadNextMainScene();
            }
        }

        if (pendingFinalTransition && currentScene == doorSceneName)
        {
            pendingFinalTransition = false;
            SceneManager.LoadScene(finalSceneName);
        }
    }

    private void LoadNextMainScene()
    {
        if (sceneNames == null || sceneNames.Length == 0)
        {
            Debug.LogWarning("SceneLoopSwitcher: sceneNames が設定されていません。");
            return;
        }

        List<int> remaining = new List<int>();
        for (int i = 0; i < sceneNames.Length; i++)
        {
            if (!playedSceneIndices.Contains(i)) remaining.Add(i);
        }

        if (remaining.Count == 0)
        {
            goToFinalSceneNext = true;
            sceneChangeRequested = true;
            return;
        }

        int newIndex = remaining[Random.Range(0, remaining.Count)];
        currentSceneIndex = newIndex;
        previousSceneIndex = newIndex;
        playedSceneIndices.Add(newIndex);

        SceneManager.LoadScene(sceneNames[currentSceneIndex]);
    }

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

    public static bool InstanceExists()
    {
        return instance != null;
    }

    // 🔄 状態をすべて初期化（Result到達時に呼ばれる）
    private void ResetLoopState()
    {
        playedSceneIndices.Clear();
        currentSceneIndex = 0;
        previousSceneIndex = -1;
        hasShownTutorial = false;
        goToFinalSceneNext = false;
        pendingFinalTransition = false;
        sceneChangeRequested = false;
        goToNextScene = false;
    }
}
