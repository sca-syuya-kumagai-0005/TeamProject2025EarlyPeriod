using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MouseManager : MonoBehaviour
{
    [SerializeField] Material click_Mat;
    float alpha;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //�X�N���[���̌��m
        var scroll = Input.mouseScrollDelta.y * Time.deltaTime * 5;
        alpha = click_Mat.GetFloat("_Alpha");
        
        if (alpha <= 0.5 )
        {
            Debug.Log("�Y�[���͂���");
            click_Mat.SetFloat("_Alpha", alpha�@+=�@scroll);          
        }

        //�g��k����Max_Min�̔���
        if (alpha > 0.5)
        {
            Debug.Log("�k���������I�I");
            alpha = 0.5f;
            click_Mat.SetFloat("_Alpha", alpha);
        }
        if (alpha < -0.2)
        {
            Debug.Log("�g�債�����I�I");
            alpha = -0.2f;
            click_Mat.SetFloat("_Alpha", alpha);
        }
    }
}
