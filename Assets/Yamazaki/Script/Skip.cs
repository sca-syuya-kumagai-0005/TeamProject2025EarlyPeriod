using UnityEngine;
using UnityEngine.SceneManagement;

public class Skip : MonoBehaviour
{
    string doorScene = "DoorScene1";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Skip_Button()
    {
        SceneManager.LoadScene(doorScene);
    }
}
