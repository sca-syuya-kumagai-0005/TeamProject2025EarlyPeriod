using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultButton : MonoBehaviour
{

    [Header("シーン移行")]
    [SerializeField] string TitleSceneName = "Title";//←移動先のシーン名
    [SerializeField] string MainGameSceneName = "";
    /// <summary>
    /// リザルトからタイトルシーンへ移行
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
