using UnityEngine;

public class TimerSceneTrigger : MonoBehaviour
{
    private float waitTime = 5f;
    private float timer = 0f;
    const string timerTag = "Battery";
    TimeManager timeManager;
    private void Start()
    {
        GameObject obj = GameObject.Find(timerTag).gameObject;
        timeManager=obj.GetComponent<TimeManager>();
        waitTime = timeManager.WaitTimer;
    }
    void Update()
    {
        timer = timeManager.Timer;
        if (timer >= waitTime)
        {
            SceneLoopSwitcher.TriggerNextScene(); // Doorシーン → メインシーンに繋がる
            enabled = false;
        }
    }
}
