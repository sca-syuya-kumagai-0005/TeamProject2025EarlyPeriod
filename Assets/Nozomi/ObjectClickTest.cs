using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectClickTest : MonoBehaviour
{
    //�N���b�N���ꂽ�Q�[���I�u�W�F�N�g��������ϐ�
    GameObject clickedGameObject;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit))
            {
                clickedGameObject = hit.collider.gameObject;
                Debug.Log(clickedGameObject.name);//�Q�[���I�u�W�F�N�g�̖��O���o��
                Destroy(clickedGameObject);//�Q�[���I�u�W�F�N�g��j��
            }
        }
    }
}
