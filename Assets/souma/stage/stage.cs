using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoopSwitcher : MonoBehaviour
{
    public string[] sceneNames; // �؂�ւ������V�[������z��Őݒ�
    public float interval = 3f; // �b��

    private int previousSceneIndex = -1; // �O��I�΂ꂽ�V�[���̃C���f�b�N�X�i�����l��-1�j
    private int currentSceneIndex = 0;

    void Start()
    {
        StartCoroutine(SwitchScenesLoop());
    }

    IEnumerator SwitchScenesLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            // �����_���ɃV�[����I�ԁi�O��I�΂ꂽ�V�[���������j
            do
            {
                currentSceneIndex = Random.Range(0, sceneNames.Length);
            } while (currentSceneIndex == previousSceneIndex);

            // �O��I�΂ꂽ�V�[���̃C���f�b�N�X���X�V
            previousSceneIndex = currentSceneIndex;

            // �V�[���̃��[�h
            SceneManager.LoadScene(sceneNames[currentSceneIndex]);
        }
    }

    void Awake()
    {
        if (FindObjectsOfType<SceneLoopSwitcher>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}
