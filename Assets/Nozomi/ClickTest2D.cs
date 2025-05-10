using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClickTest2D : MonoBehaviour
{
    const float timer=3.0f;
    float flamecount = 0.0f;
    [SerializeField] float alphaTimer;
    bool alphaStart;
    [SerializeField]Collider2D[] colliders;
    [SerializeField]SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        alphaTimer = 1.0f;
       colliders = GetComponents<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject clickedGameObject;//�N���b�N���ꂽ�Q�[���I�u�W�F�N�g��������ϐ�

        // Update is called once per frame
        
        if(alphaStart)
        {
            alphaTimer -= Time.deltaTime/timer;
        }

        spriteRenderer.color = new Color(1,1,1, alphaTimer);

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

            if (hit2d)
            {
                alphaStart = true;
                clickedGameObject = hit2d.transform.gameObject;
                Debug.Log(clickedGameObject.name);//�Q�[���I�u�W�F�N�g�̖��O���o��
                for(int i = 0;i < colliders.Length; i++)
                {
                    colliders[i].enabled = false;
                }
               // Collider.Destroy(clickedGameObject);//�Q�[���I�u�W�F�N�g�̔����j��
                StartCoroutine(DestroyTimer(clickedGameObject));
            }

        }
    }
    
    IEnumerator DestroyTimer(GameObject obj)
    {
        yield return new WaitForSeconds(timer);
        Destroy(obj);
    }
}
