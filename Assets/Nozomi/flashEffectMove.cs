using UnityEngine;

public class flashEffectMove : MonoBehaviour
{
    private GameObject mouseSquare;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mouseSquare = GameObject.Find("MouseSquare");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = mouseSquare.transform.position;
    }
}
