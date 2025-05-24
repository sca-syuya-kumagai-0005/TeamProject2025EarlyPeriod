using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MouseManager : MonoBehaviour
{
    float magnification;
    Vector3 mousePos;
    [SerializeField] RectTransform canvasRect;
    [SerializeField] GameObject cameraPanel;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        magnification = canvasRect.sizeDelta.x / Screen.width;
        mousePos.x = mousePos.x * magnification - canvasRect.sizeDelta.x;
        mousePos.y = mousePos.y * magnification - canvasRect.sizeDelta.y;
        mousePos.z = transform.localPosition.z;

        cameraPanel.transform.localPosition = mousePos;

    }
}
