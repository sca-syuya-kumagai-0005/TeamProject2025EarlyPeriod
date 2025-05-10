using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoopSwitcher : MonoBehaviour
{
    public string[] sceneNames; // �؂�ւ������V�[������z��Őݒ�
    public float interval = 3f; // �b���i30�b�j

    private int currentSceneIndex = 0;

    void Start()
    {
        DontDestroyOnLoad(gameObject); // �V�[���؂�ւ����ɂ��c���i�C�Ӂj
        StartCoroutine(SwitchScenesLoop());
    }

    IEnumerator SwitchScenesLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            currentSceneIndex = (currentSceneIndex + 1) % sceneNames.Length; // ���[�v
            SceneManager.LoadScene(sceneNames[currentSceneIndex]);
        }
    }
}
