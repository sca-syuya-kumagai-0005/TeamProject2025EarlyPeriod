using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScaleToSceneChange : MonoBehaviour
{
    [Header("�Ώۂ�Image")]
    public Image targetImage;

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
        if (targetImage == null || string.IsNullOrEmpty(nextSceneName)) return;

        Vector3 currentScale = targetImage.rectTransform.localScale;

        // �덷�͈͂��܂߂ăX�P�[�����r
        if (Vector3.Distance(currentScale, targetScale) <= tolerance)
        {
            hasSceneChanged = true;
            SceneManager.LoadScene(nextSceneName);
        }
    }
}