using UnityEngine;

public class CameraFrameTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[CameraFrameTrigger] OnTriggerEnter2D: {other.name}");

        var enemy = other.GetComponent<EnemySpriteAnimator>();
        if (enemy != null)
        {
            Debug.Log($"[�J�����t���[��] �G���t���[�����ɓ���܂���: {enemy.name}");
            enemy.LockByCameraFrame();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log($"[CameraFrameTrigger] OnTriggerExit2D: {other.name}");

        var enemy = other.GetComponent<EnemySpriteAnimator>();
        if (enemy != null)
        {
            Debug.Log($"[�J�����t���[��] �G���t���[������o�܂���: {enemy.name}");
        }
    }
}
