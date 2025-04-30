using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultButton : MonoBehaviour
{

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            ResulttoTaitle();
        }
    }

    /// <summary>
    /// リザルトからタイトルシーンへ移行
    /// </summary>
    void ResulttoTaitle()
    {
        SceneManager.LoadSceneAsync("Title");
    }
}
