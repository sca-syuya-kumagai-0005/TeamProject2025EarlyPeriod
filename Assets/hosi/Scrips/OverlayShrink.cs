using UnityEngine;

public class OverlayShrink : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        gameObject.SetActive(false); // �ŏ��͔�\��
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameObject.SetActive(true);     // �\��
            animator.SetTrigger("Shrink"); // �A�j���[�V�������s
        }
    }
}