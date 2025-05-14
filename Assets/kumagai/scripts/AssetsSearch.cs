using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AssetsSearch : MonoBehaviour
{
    const string frontPath = "Assets/kumagai/Sound/";
    const string backPath = ".mp3";
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetBGM("Test.mp3");
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    private void BGMPlayer()
    {

    }

    protected void SetBGM(string name)
    {
        Debug.Log(name.Substring(name.Length - 4));
        if(name.Substring(name.Length-4)==backPath)
        {
            Debug.LogError("関数SetBGMの引数nameは.mp3を必要としません");
        }
        if (AssetDatabase.LoadAssetAtPath<AudioClip>(frontPath + name + backPath)==null) 
        {
            Debug.LogError("指定されたAudioClipが見つけられませんでした。引数nameを確認してください");
        }
    }
}
