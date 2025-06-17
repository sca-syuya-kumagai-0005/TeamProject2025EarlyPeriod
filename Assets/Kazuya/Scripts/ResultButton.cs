using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultButton : MonoBehaviour
{

    [Header("�V�[���ڍs")]
    [SerializeField] string TitleSceneName = "Title";//���ړ���̃V�[����
    [SerializeField] string MainGameSceneName = "";
    /// <summary>
    /// ���U���g����^�C�g���V�[���ֈڍs
    /// </summary>
   public void ResultToTaitle()
    {
        SceneManager.LoadSceneAsync(TitleSceneName);
    }



    public void ResultToMainGame()
    {
        SceneManager.LoadScene(MainGameSceneName);
    }
}
