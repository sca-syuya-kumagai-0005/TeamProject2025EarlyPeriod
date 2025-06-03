using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiScaleToSceneChange : MonoBehaviour
{
    [Header("�Ώۂ�SpriteRenderer���X�g")]
    public SpriteRenderer[] targetSprites;

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
        if (targetSprites == null || targetSprites.Length == 0 || string.IsNullOrEmpty(nextSceneName)) return;

        foreach (SpriteRenderer sr in targetSprites)
        {
            if (sr == null) continue;
            Debug.Log(targetSprites);
            Vector3 currentScale = sr.transform.localScale;

            if (Vector3.Distance(currentScale, targetScale) > tolerance)
            {
                return; // 1�ł����B���Ȃ�V�[���؂�ւ����Ȃ�
            }
        }

        // ���ׂăX�P�[�����B �� �V�[���؂�ւ�
        hasSceneChanged = true;
        SceneManager.LoadScene(nextSceneName);
    }
}