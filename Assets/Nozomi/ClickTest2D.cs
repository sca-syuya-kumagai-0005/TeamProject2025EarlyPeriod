using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClickTest2D : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject clickedGameObject;//�N���b�N���ꂽ�Q�[���I�u�W�F�N�g��������ϐ�

        // Update is called once per frame
        
           

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

                if (hit2d)
                {
                    clickedGameObject = hit2d.transform.gameObject;
                    Debug.Log(clickedGameObject.name);//�Q�[���I�u�W�F�N�g�̖��O���o��
                    Destroy(clickedGameObject);//�Q�[���I�u�W�F�N�g��j��
                }

            }
        }
    
}
