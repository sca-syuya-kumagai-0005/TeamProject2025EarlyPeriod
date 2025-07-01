using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoopSwitcher : MonoBehaviour
{
    [Header("���C���V�[���̖��O���X�g�iBuild Settings�Ɋ܂߂�j")]
    // �����_���ɑI�΂�郁�C���V�[���̖��O�i��: Main1, Main2 �Ȃǁj
    public string[] sceneNames;

    [Header("�ŏ��Ƀ��[�h����Door�V�[����")]
    // �N������ɍŏ��ɑJ�ڂ���Door�V�[���̖��O
    public string doorSceneName = "Door";

    // �O��I�΂ꂽ�V�[���̃C���f�b�N�X�i�����V�[����A���őI�΂Ȃ����߁j
    private int previousSceneIndex = -1;
    // ����I�΂��V�[���̃C���f�b�N�X
    private int currentSceneIndex = 0;

    // Door�V�[������Ăяo�����t���O�Btrue�ɂȂ�ƃV�[����؂�ւ���B
    private bool sceneChangeRequested = false;

    // �V���O���g���C���X�^���X�i1�������݂���悤�ɊǗ��j
    private static SceneLoopSwitcher instance;

    void Awake()
    {
        // �V���O���g�������F���łɕʂ̃C���X�^���X�����݂��Ă����玩����j������
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        // ���̃I�u�W�F�N�g��B��̃C���X�^���X�ɐݒ�
        instance = this;
        GameObject[] objs=GameObject.FindGameObjectsWithTag("Canvas");
        if (objs.Length > 1)
        {
            for (int i = 1; i < objs.Length; i++)
            {
                Destroy(objs[i]);
            }
        }
        // �V�[�����ׂ��ł����̃I�u�W�F�N�g�������Ȃ��悤�ɂ���
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // ���݂̃V�[����Door�V�[���łȂ��ꍇ�A�����I��Door�V�[���֐؂�ւ���
        if (SceneManager.GetActiveScene().name != doorSceneName)
        {
            SceneManager.LoadScene(doorSceneName);
        }
    }

    void Update()
    {
        // Door.cs ���� RequestSceneChange() ���Ă΂��ƁA���̃t���O�� true �ɂȂ�
        if (sceneChangeRequested)
        {
            sceneChangeRequested = false; // ��x�������s���邽�� false �ɖ߂�

            // ���C���V�[���������ݒ�܂��͋�ł���Όx�����o���ďI��
            if (sceneNames == null || sceneNames.Length == 0)
            {
                Debug.LogWarning("SceneLoopSwitcher: sceneNames ����ł��I");
                return;
            }

            // �����_���ɃV�[����I�ԁB�������A�O��Ɠ����V�[���͔�����
            do
            {
                currentSceneIndex = Random.Range(0, sceneNames.Length);
            } while (sceneNames.Length > 1 && currentSceneIndex == previousSceneIndex);

            previousSceneIndex = currentSceneIndex;

            // �I�΂ꂽ�V�[���ɐ؂�ւ���
            SceneManager.LoadScene(sceneNames[currentSceneIndex]);
        }
    }

    // Door.cs ����Ă΂��ÓI�֐��B�t���O�𗧂ĂāA����Update�ŃV�[���؂�ւ����s���B
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
}
