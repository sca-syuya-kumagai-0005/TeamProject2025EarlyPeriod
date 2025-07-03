using UnityEngine;

public class TimerSceneTrigger : MonoBehaviour
{
    public float waitTime = 5f;
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= waitTime)
        {
            SceneLoopSwitcher.TriggerNextScene(); // Door�V�[�� �� ���C���V�[���Ɍq����
            enabled = false;
        }
    }
}
