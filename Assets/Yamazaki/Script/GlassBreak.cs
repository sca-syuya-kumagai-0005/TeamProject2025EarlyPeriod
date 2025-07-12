using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GlassBrake : MonoBehaviour
{
    const string nextScene = "Result";
    [SerializeField] private float flashDuration = 0.5f;
    [SerializeField] private GameObject[] glassObj;
    [SerializeField] Sprite[] ghostSprite; 
    public float explosionForce = 500f;
    public float explosionRadius = 2f;
    public float upwardsModifier = 0.5f;

    [Header("�ړ��E�X�P�[���p�C�[�W���O�J�[�u")]
    [SerializeField] private AnimationCurve easeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("�ړ���̍��W")]
    public Vector3 targetPosition = new Vector3(5, 0, 0);

    [Header("�ړI�̃X�P�[��")]
    public Vector3 targetScale = new Vector3(2, 2, 2);

    [Header("�ړ����ԁi�b�j")]
    public float duration = 3f;

    [Header("�ړ��Ώۂ̃I�u�W�F�N�gTransform")]
    public Transform targetObject_Transform;

    [Header("�ړ��Ώۂ̃I�u�W�F�N�g")]
    public SpriteRenderer targetObject_SpriteRenderer;

    public List<Rigidbody> shardRigidbodies = new List<Rigidbody>();

    public Canvas GameOver;//��ŏ���
    public GameObject Camera;

    private void Start()
    {
        Flash();
        Camera.SetActive(true);
    }

    public void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
        StartCoroutine(MoveAndScale(targetObject_Transform, targetPosition, targetScale, duration));

    }

    public System.Collections.IEnumerator MoveAndScale(Transform targetTransform, Vector3 targetPosition, Vector3 targetScale, float duration)
    {
        if (targetTransform == null)
        {
            Debug.LogWarning("�^�[�Q�b�gTransform���w�肳��Ă��܂���B");
            yield break;
        }
        for (int i = 0; i < ghostSprite.Length; i++)
        {
            yield return new WaitForSeconds(1.1f);
            targetObject_SpriteRenderer.sprite = ghostSprite[i];
            
        }



        Vector3 startPos = targetTransform.position;
        Vector3 startScale = targetTransform.localScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float easedT = easeCurve.Evaluate(t);

            targetTransform.position = Vector3.Lerp(startPos, targetPosition, easedT);
            targetTransform.localScale = Vector3.Lerp(startScale, targetScale, easedT);

            elapsed += Time.deltaTime;
            yield return null;
        }

        GameOver.enabled = true;
        targetTransform.position = targetPosition;
        targetTransform.localScale = targetScale;
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(nextScene);
    }

    private IEnumerator FlashRoutine()
    {
        yield return new WaitForSeconds(2.5f);
        for (int i = 0; i < glassObj.Length; i++)
        {
            glassObj[i].SetActive(true);
            yield return new WaitForSeconds(flashDuration);
        }
        Camera.SetActive(false);
        PrepareRigidbodies();

        // �j�ЌQ�̏d�S���v�Z
        Vector3 explosionCenter = Vector3.zero;
        int count = 0;
        foreach (GameObject glass in glassObj)
        {
            explosionCenter += glass.transform.position;
            count++;
        }
        if (count > 0) explosionCenter /= count;
        else explosionCenter = transform.position;

        // �����͂�������
        foreach (Rigidbody rb in shardRigidbodies)
        {
            rb.AddExplosionForce(
                explosionForce,
                explosionCenter,
                explosionRadius,
                upwardsModifier,
                ForceMode.Impulse
            );
            rb.mass = 0.1f;
        }
    }

    private void PrepareRigidbodies()
    {
        shardRigidbodies.Clear();

        foreach (GameObject glass in glassObj)
        {
            var shards = glass.GetComponentsInChildren<MeshRenderer>();

            foreach (MeshRenderer shardRenderer in shards)
            {
                GameObject shard = shardRenderer.gameObject;

                Rigidbody rb = shard.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    rb = shard.AddComponent<Rigidbody>();
                }

                var col = shard.GetComponent<MeshCollider>();
                if (col == null)
                {
                    col = shard.AddComponent<MeshCollider>();
                }
                col.convex = true;

                shardRigidbodies.Add(rb);
            }
        }
    }
}
