using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    Vector3 mousePos, pos;
    int score;
    Collider2D circleCollider;

    void Start()
    {
        score = 0;
        circleCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        // �}�E�X���W���烏�[���h���W�֕ϊ����ʒu���X�V
        mousePos = Input.mousePosition;
        pos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
        transform.position = pos;

        // �}�E�X���N���b�N�ŃX�R�A�v�Z
        if (Input.GetMouseButtonDown(0))
        {
            AddScore();
        }
    }

    void AddScore()
    {
        Collider2D[] eyes = GameObject.FindObjectsOfType<Collider2D>();
        int addPoints = 0;

        foreach (var eye in eyes)
        {
            if (eye.CompareTag("Eye"))
            {
                // �e�ڂ̑SBounds���T�[�N�����Ɋ܂܂�Ă��邩�`�F�b�N
                if (IsFullyInside(circleCollider.bounds, eye.bounds))
                {
                    addPoints++;
                }
            }
        }

        if (addPoints > 0)
        {
            score += addPoints;
            Debug.Log("Score: " + score);
        }
    }

    bool IsFullyInside(Bounds outer, Bounds inner)
    {
        return outer.Contains(inner.min) && outer.Contains(inner.max);
    }
}