using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoopSwitcher : MonoBehaviour
{
    public string[] sceneNames; // 切り替えたいシーン名を配列で設定
    public float interval = 3f; // 秒数

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

            currentSceneIndex = (currentSceneIndex + 1) % sceneNames.Length; // ループ
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