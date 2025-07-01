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
        // �ŏ���Door�V�[���ֈړ�
        SceneManager.LoadScene(doorSceneName);
    }

    void Update()
    {
        // Door������Ăяo���ꂽ��V�[���؂�ւ����s��
        if (sceneChangeRequested)
        {
            sceneChangeRequested = false;

            // �����_���ɃV�[���I���i�O��ƈقȂ�j
            do
            {
                currentSceneIndex = Random.Range(0, sceneNames.Length);
            } while (currentSceneIndex == previousSceneIndex);

            previousSceneIndex = currentSceneIndex;

            SceneManager.LoadScene(sceneNames[currentSceneIndex]);
        }
    }

    // �� Door.cs ����Ăяo���p�̊֐�
    public static void RequestSceneChange()
    {
        if (instance != null)
        {
            instance.sceneChangeRequested = true;
        }
    }
}
