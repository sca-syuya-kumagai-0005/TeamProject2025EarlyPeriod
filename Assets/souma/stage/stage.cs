using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoopSwitcher : MonoBehaviour
{
    [Header("メインシーンの名前リスト（Build Settingsに含める）")]
    // ランダムに選ばれるメインシーンの名前（例: Main1, Main2 など）
    public string[] sceneNames;

    [Header("最初にロードするDoorシーン名")]
    // 起動直後に最初に遷移するDoorシーンの名前
    public string doorSceneName = "Door";

    // 前回選ばれたシーンのインデックス（同じシーンを連続で選ばないため）
    private int previousSceneIndex = -1;
    // 今回選ばれるシーンのインデックス
    private int currentSceneIndex = 0;

    // Doorシーンから呼び出されるフラグ。trueになるとシーンを切り替える。
    private bool sceneChangeRequested = false;

    // シングルトンインスタンス（1つだけ存在するように管理）
    private static SceneLoopSwitcher instance;

    void Awake()
    {
        // シングルトン処理：すでに別のインスタンスが存在していたら自分を破棄する
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        // このオブジェクトを唯一のインスタンスに設定
        instance = this;
        GameObject[] objs=GameObject.FindGameObjectsWithTag("Canvas");
        if (objs.Length > 1)
        {
            for (int i = 1; i < objs.Length; i++)
            {
                Destroy(objs[i]);
            }
        }
        // シーンを跨いでもこのオブジェクトが消えないようにする
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // 現在のシーンがDoorシーンでない場合、自動的にDoorシーンへ切り替える
        if (SceneManager.GetActiveScene().name != doorSceneName)
        {
            SceneManager.LoadScene(doorSceneName);
        }
    }

    void Update()
    {
        // Door.cs から RequestSceneChange() が呼ばれると、このフラグが true になる
        if (sceneChangeRequested)
        {
            sceneChangeRequested = false; // 一度だけ実行するため false に戻す

            // メインシーン名が未設定または空であれば警告を出して終了
            if (sceneNames == null || sceneNames.Length == 0)
            {
                Debug.LogWarning("SceneLoopSwitcher: sceneNames が空です！");
                return;
            }

            // ランダムにシーンを選ぶ。ただし、前回と同じシーンは避ける
            do
            {
                currentSceneIndex = Random.Range(0, sceneNames.Length);
            } while (sceneNames.Length > 1 && currentSceneIndex == previousSceneIndex);

            previousSceneIndex = currentSceneIndex;

            // 選ばれたシーンに切り替える
            SceneManager.LoadScene(sceneNames[currentSceneIndex]);
        }
    }

    // Door.cs から呼ばれる静的関数。フラグを立てて、次のUpdateでシーン切り替えを行う。
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
}
