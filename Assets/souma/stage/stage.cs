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

    private int currentSceneIndex = 0;
    private int previousSceneIndex = -1;

    private bool goToNextScene = false;
    private bool sceneChangeRequested = false;

    private static SceneLoopSwitcher instance;

    // �Đ��ς݂̃V�[���C���f�b�N�X�ꗗ
    private List<int> playedSceneIndices = new List<int>();

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        // Canvas �̏d���h�~�i�C�Ӂj
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
        // Door�ֈڍs
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

            // �S�V�[���Đ��ς݂Ȃ� FinalScene ��
            if (playedSceneIndices.Count >= sceneNames.Length)
            {
                SceneManager.LoadScene(finalSceneName);
                return;
            }

            // �Đ�����Ă��Ȃ��V�[���̃C���f�b�N�X���W�߂�
            List<int> availableIndices = new List<int>();
            for (int i = 0; i < sceneNames.Length; i++)
            {
                if (!playedSceneIndices.Contains(i))
                {
                    availableIndices.Add(i);
                }
            }

            // �����_���ɖ��Đ��V�[����I��
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
                // �O�̂��߁i���B���Ȃ��z��j
                Debug.LogWarning("�Đ��\�ȃV�[����������܂���");
            }
        }
    }

    // Door �V�[��������Ă΂��F���C���V�[���Ɉړ�����g���K�[
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

    // �O���X�N���v�g�i�^�C�}�[�Ȃǁj���� Door �V�[���ֈړ�
    public static void TriggerNextScene()
    {
        if (instance != null)
        {
            instance.goToNextScene = true;
        }
        else
        {
            Debug.LogWarning("SceneLoopSwitcher: TriggerNextScene �Ɏ��s�B�C���X�^���X��������܂���B");
        }
    }
}
