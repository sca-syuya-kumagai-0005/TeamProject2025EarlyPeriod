using UnityEngine;

public class CameraFrameTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[CameraFrameTrigger] OnTriggerEnter2D: {other.name}");

        var enemy = other.GetComponent<EnemySpriteAnimator>();
        if (enemy != null)
        {
            Debug.Log($"[カメラフレーム] 敵がフレーム内に入りました: {enemy.name}");
            enemy.LockByCameraFrame();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log($"[CameraFrameTrigger] OnTriggerExit2D: {other.name}");

        var enemy = other.GetComponent<EnemySpriteAnimator>();
        if (enemy != null)
        {
            Debug.Log($"[カメラフレーム] 敵がフレームから出ました: {enemy.name}");
        }
    }
}
