using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class SceneChange : MonoBehaviour
{
    [Tooltip("�؂�ւ���V�[���̖��O")]
    public string targetSceneName;

    [Tooltip("�N���b�N��ɑ҂b��")]
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
