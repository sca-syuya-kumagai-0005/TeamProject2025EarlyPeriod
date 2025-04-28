using UnityEngine;

public class ColorTest : MonoBehaviour
{
    [SerializeField] Material material;
    [SerializeField] MeshRenderer meshRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log(GetComponent<MeshRenderer>());
        material = GetComponent<MeshRenderer>().material;
        Debug.Log(material);    
        material.color = new Color(0,0,0,0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
