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
    private bool hasShownTutorial = false; // ← チュートリアル一回だけ表示

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
        manager = spawnManager;
        if (SceneManager.GetActiveScene().name != doorSceneName)
        {
            SceneManager.LoadScene(doorSceneName);
        }
    }

    void Update()
    {
        // Doorから次へ（チュートリアル or メイン）
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

            if (!hasShownTutorial && !string.IsNullOrEmpty(tutorialSceneName))
            {
                hasShownTutorial = true;
                SceneManager.LoadScene(tutorialSceneName);
            }
            else
            {
                LoadNextMainScene();
            }
        }
    }

    // メインシーン読み込み処理
    private void LoadNextMainScene()
    {
        if (sceneNames == null || sceneNames.Length == 0)
        {
            Debug.LogWarning("SceneLoopSwitcher: sceneNames が設定されていません。");
            return;
        }

        do
        {
            currentSceneIndex = Random.Range(0, sceneNames.Length);
        } while (sceneNames.Length > 1 && currentSceneIndex == previousSceneIndex);

        previousSceneIndex = currentSceneIndex;

        SceneManager.LoadScene(sceneNames[currentSceneIndex]);
    }

    // Doorシーンから呼ばれる：次のシーンへ進む
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

    // 外部からDoorシーンを挟むかResultに行くか判断
    public static void TriggerNextScene(bool clear)
    {
        if (clear)
        {
            SceneManager.LoadScene(instance.doorSceneName);
        }
        else
        {
            SceneManager.LoadScene(instance.finalSceneName);
        }
    }
}
