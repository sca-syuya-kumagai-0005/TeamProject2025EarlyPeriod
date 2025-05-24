using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float lifetime = 3.0f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}