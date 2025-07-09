using UnityEngine;

public class TimerSceneTrigger : MonoBehaviour
{
    private float waitTime = 5f;
    private float timer = 0f;
    const string timerTag = "Battery";
    const string spawnManagerTag = "SpawnManager";
    TimeManager timeManager;
    SpawnManager spawnManager;
    bool clear;
    private void Start()
    {
        GameObject obj = GameObject.Find(timerTag).gameObject;
        timeManager=obj.GetComponent<TimeManager>();
        waitTime = timeManager.WaitTimer;
        spawnManager = GameObject.Find(spawnManagerTag).gameObject.GetComponent<SpawnManager>();
    }
    void Update()
    {
        clear = spawnManager.ClearLine;
        timer = timeManager.Timer;
        if (timer >= waitTime)
        {
            SceneLoopSwitcher.TriggerNextScene(clear); // Doorシーン → メインシーンに繋がる
            enabled = false;
        }
    }
}
