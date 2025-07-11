using UnityEngine;

public class EnemyWarning : MonoBehaviour
{

    [Header("警告サウンド")]
    public AudioClip warningSE;
    private AudioSource audioSource;

    [Header("スケール判定値（インスペクターで設定可能）")]
    public Vector3 targetScale = new Vector3(3f, 3f, 1f);

    [Tooltip("スケール比較時の許容誤差（例：0.01〜0.1）")]
    public float tolerance = 0.01f;

    private bool hasPlayed = false;

    void Start()
    {
        // AudioSource を取得 or 自動追加
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        // Enemyタグが付いている全てのオブジェクトを取得
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            Vector3 scale = enemy.transform.localScale;

            // スケールが targetScale ± tolerance の範囲内かどうか判定
            if (IsApproximatelyEqual(scale, targetScale, tolerance))
            {
                if (!hasPlayed)
                {
                    Debug.Log("警告SE再生：スケールが " + targetScale + " に一致したエネミーを検出");
                    audioSource.PlayOneShot(warningSE);
                    hasPlayed = true;
                }
                return;
            }
        }

        // 条件を満たすエネミーがいない場合、フラグリセット
        hasPlayed = false;
    }

    // Vector3の各要素がほぼ等しいか判定するヘルパーメソッド
    private bool IsApproximatelyEqual(Vector3 a, Vector3 b, float tol)
    {
        return Mathf.Abs(a.x - b.x) < tol &&
               Mathf.Abs(a.y - b.y) < tol &&
               Mathf.Abs(a.z - b.z) < tol;
    }
}
