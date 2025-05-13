using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoopSwitcher : MonoBehaviour
{
    public string[] sceneNames; // �؂�ւ������V�[������z��Őݒ�
    public float interval = 3f; // �b��

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

            currentSceneIndex = (currentSceneIndex + 1) % sceneNames.Length; // ���[�v
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