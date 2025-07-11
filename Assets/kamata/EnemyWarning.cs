using UnityEngine;

public class EnemyWarning : MonoBehaviour
{
    [Header("�x���T�E���h")]
    public AudioClip warningSE;
    private AudioSource audioSource;

    private bool hasPlayed = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        bool conditionMet = false;

        foreach (GameObject enemy in enemies)
        {
            Vector3 scale = enemy.transform.localScale;
            // ���ꂼ�� (3,3,1) �҂�����Ŕ��肷��
            if (Mathf.Approximately(scale.x, 3f) &&
                Mathf.Approximately(scale.y, 3f) &&
                Mathf.Approximately(scale.z, 1f))
            {
                conditionMet = true;
                break;
            }
        }

        if (conditionMet)
        {
            if (!hasPlayed)
            {
                Debug.Log("�x��SE�Đ��i�X�P�[����v�j");
                audioSource.PlayOneShot(warningSE);
                hasPlayed = true;
            }
        }
        else
        {
            hasPlayed = false; // �������O�ꂽ�烊�Z�b�g�i�J��Ԃ��Đ��������Ȃ�j
        }
    }
}
