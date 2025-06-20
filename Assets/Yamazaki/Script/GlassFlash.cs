using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlassFlash : MonoBehaviour
{
    [SerializeField] private float flashDuration = 0.5f;
    [SerializeField] private GameObject[] GlassObj;

    public float explosionForce = 500f;
    public float explosionRadius = 2f;
    public float upwardsModifier = 0.5f;

    public List<Rigidbody> shardRigidbodies = new List<Rigidbody>();

    private void Start()
    {
        Flash();
    }

    public void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        for (int i = 0; i < GlassObj.Length; i++)
        {
            GlassObj[i].SetActive(true);
            yield return new WaitForSeconds(flashDuration);
        }

        PrepareRigidbodies();

        // ”j•ÐŒQ‚ÌdS‚ðŒvŽZ
        Vector3 explosionCenter = Vector3.zero;
        int count = 0;
        foreach (GameObject glass in GlassObj)
        {
            explosionCenter += glass.transform.position;
            count++;
        }
        if (count > 0) explosionCenter /= count;
        else explosionCenter = transform.position;

        // ”š”­—Í‚ð‰Á‚¦‚é
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

        foreach (GameObject glass in GlassObj)
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
