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
        //スクロールの検知
        var scroll = Input.mouseScrollDelta.y * Time.deltaTime * 5;
        alpha = click_Mat.GetFloat("_Alpha");
        
        if (alpha <= 0.5 )
        {
            Debug.Log("ズームはじめ");
            click_Mat.SetFloat("_Alpha", alpha　+=　scroll);          
        }

        //拡大縮小のMax_Minの判定
        if (alpha > 0.5)
        {
            Debug.Log("縮小しすぎ！！");
            alpha = 0.5f;
            click_Mat.SetFloat("_Alpha", alpha);
        }
        if (alpha < -0.2)
        {
            Debug.Log("拡大しすぎ！！");
            alpha = -0.2f;
            click_Mat.SetFloat("_Alpha", alpha);
        }
    }
}
