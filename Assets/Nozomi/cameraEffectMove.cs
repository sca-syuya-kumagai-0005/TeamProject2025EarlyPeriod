using UnityEngine;

public class cameraEffectMove : MonoBehaviour
{
    private GameObject cameraCenter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraCenter = GameObject.Find("CameraCenter");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = cameraCenter.transform.position;
    }
}
