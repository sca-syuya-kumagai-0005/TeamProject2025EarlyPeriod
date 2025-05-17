using UnityEngine;

public class OverlayShrink : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        gameObject.SetActive(false); // 最初は非表示
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameObject.SetActive(true);     // 表示
            animator.SetTrigger("Shrink"); // アニメーション実行
        }
    }
}