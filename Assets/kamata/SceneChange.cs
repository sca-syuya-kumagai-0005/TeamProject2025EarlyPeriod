using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class SceneChange : MonoBehaviour
{
    [Tooltip("切り替えるシーンの名前")]
    public string targetSceneName;

    [Tooltip("クリック後に待つ秒数")]
    public float delaySeconds = 1f;

    private bool isSwitching = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isSwitching)
        {
            StartCoroutine(SwitchSceneWithDelay());
        }
    }

    IEnumerator SwitchSceneWithDelay()
    {
        isSwitching = true;
        yield return new WaitForSeconds(delaySeconds);
        SceneManager.LoadScene(targetSceneName);
    }
}
