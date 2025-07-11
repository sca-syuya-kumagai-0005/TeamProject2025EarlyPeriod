using UnityEngine;

public class EnemyWarning : MonoBehaviour
{
    [Header("警告サウンド")]
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
            // それぞれ (3,3,1) ぴったりで判定する
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
                Debug.Log("警告SE再生（スケール一致）");
                audioSource.PlayOneShot(warningSE);
                hasPlayed = true;
            }
        }
        else
        {
            hasPlayed = false; // 条件が外れたらリセット（繰り返し再生を許すなら）
        }
    }
}
