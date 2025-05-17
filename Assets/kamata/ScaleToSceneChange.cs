using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MultiScaleToSceneChange : MonoBehaviour
{
    [Header("�Ώۂ�Image���X�g")]
    public Image[] targetImages;

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
        if (targetImages == null || targetImages.Length == 0 || string.IsNullOrEmpty(nextSceneName)) return;

        // �S�Ă�Image���ڕW�X�P�[���ɒB���Ă��邩�`�F�b�N
        foreach (Image img in targetImages)
        {
            if (img == null) continue;

            Vector3 currentScale = img.rectTransform.localScale;
            if (Vector3.Distance(currentScale, targetScale) > tolerance)
            {
                return; // 1�ł����B��������Δ�����
            }
        }

        // �S�ĒB�����Ă�����V�[���J��
        hasSceneChanged = true;
        SceneManager.LoadScene(nextSceneName);
    }
}