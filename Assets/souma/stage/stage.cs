using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoopSwitcher : MonoBehaviour
{
    public string[] sceneNames;
    public string doorSceneName = "Door";

    private int previousSceneIndex = -1;
    private int currentSceneIndex = 0;
    private bool sceneChangeRequested = false;

    private static SceneLoopSwitcher instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // 最初にDoorシーンへ移動
        SceneManager.LoadScene(doorSceneName);
    }

    void Update()
    {
        // Door側から呼び出されたらシーン切り替えを行う
        if (sceneChangeRequested)
        {
            sceneChangeRequested = false;

            // ランダムにシーン選択（前回と異なる）
            do
            {
                currentSceneIndex = Random.Range(0, sceneNames.Length);
            } while (currentSceneIndex == previousSceneIndex);

            previousSceneIndex = currentSceneIndex;

            SceneManager.LoadScene(sceneNames[currentSceneIndex]);
        }
    }

    // ▼ Door.cs から呼び出す用の関数
    public static void RequestSceneChange()
    {
        if (instance != null)
        {
            instance.sceneChangeRequested = true;
        }
    }
}
