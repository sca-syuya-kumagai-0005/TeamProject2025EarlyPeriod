using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneLoopSwitcher : MonoBehaviour
{
    [Header("���C���V�[���̖��O���X�g�iBuild Settings�Ɋ܂߂�j")]
    public string[] sceneNames;

    [Header("Door�V�[�����i�K��Build Settings�Ɋ܂߂�j")]
    public string doorSceneName = "DoorScene1";

    [Header("�S�V�[���Đ���ɑJ�ڂ���V�[����")]
    public string finalSceneName = "Result";

    [Header("�ŏ��������ރ`���[�g���A���V�[����")]
    public string tutorialSceneName = "TutorialScene";

    private int currentSceneIndex = 0;
    private int previousSceneIndex = -1;

    private bool goToNextScene = false;
    private bool sceneChangeRequested = false;
    private bool hasShownTutorial = false; // �� �`���[�g���A����񂾂��\��

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
        // Door���玟�ցi�`���[�g���A�� or ���C���j
        if (goToNextScene)
        {
            goToNextScene = false;

            if (SceneManager.GetActiveScene().name != doorSceneName)
            {
                SceneManager.LoadScene(doorSceneName);
            }
        }

        // ���C���V�[���J�ڗv��
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

    // ���C���V�[���ǂݍ��ݏ���
    private void LoadNextMainScene()
    {
        if (sceneNames == null || sceneNames.Length == 0)
        {
            Debug.LogWarning("SceneLoopSwitcher: sceneNames ���ݒ肳��Ă��܂���B");
            return;
        }

        do
        {
            currentSceneIndex = Random.Range(0, sceneNames.Length);
        } while (sceneNames.Length > 1 && currentSceneIndex == previousSceneIndex);

        previousSceneIndex = currentSceneIndex;

        SceneManager.LoadScene(sceneNames[currentSceneIndex]);
    }

    // Door�V�[������Ă΂��F���̃V�[���֐i��
    public static void RequestSceneChange()
    {
        if (instance != null)
        {
            instance.sceneChangeRequested = true;
        }
        else
        {
            Debug.LogWarning("SceneLoopSwitcher: �C���X�^���X�����݂��܂���B");
        }
    }

    // �O������Door�V�[�������ނ�Result�ɍs�������f
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
