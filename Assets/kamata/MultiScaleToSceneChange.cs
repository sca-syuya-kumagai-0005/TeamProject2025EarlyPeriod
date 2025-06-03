using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiScaleToSceneChange : MonoBehaviour
{
    [Header("�G�l�~�[�^�O")]
    public string enemyTag = "Enemy";

    [Header("�J�ڐ�V�[����")]
    public string nextSceneName;

    [Header("�X�P�[���̂������l")]
    public Vector3 targetScale = new Vector3(1.5f, 1.5f, 1.5f);

    [Header("�X�P�[���̔���덷���e�͈�")]
    public float tolerance = 0.01f;

    private bool hasSceneChanged = false;

    void Update()
    {
        if (hasSceneChanged) return;
        if (string.IsNullOrEmpty(nextSceneName)) return;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        if (enemies.Length == 0) return;

        foreach (GameObject enemy in enemies)
        {
            if (enemy == null) continue;

            Vector3 currentScale = enemy.transform.localScale;

            if (Vector3.Distance(currentScale, targetScale) > tolerance)
            {
                return; // 1�ł��X�P�[�������B �� �V�[���؂�ւ����Ȃ�
            }
        }

        // ���ׂăX�P�[�����B �� �V�[���؂�ւ�
        hasSceneChanged = true;
        SceneManager.LoadScene(nextSceneName);
    }
}
