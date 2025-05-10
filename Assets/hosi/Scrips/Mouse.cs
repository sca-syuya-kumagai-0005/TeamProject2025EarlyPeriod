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
        Collider2D[] colliders = GameObject.FindObjectsOfType<Collider2D>();
        int nAddPoints = 0;
        int tAddPoints = 0;

        int AddedScore = 0;

        foreach (var col in colliders)
        {
            if (col.CompareTag("nEye") && IsFullyInside(circleCollider.bounds, col.bounds))
            {
                nAddPoints++; //�m�[�}���̖ځA1��������2�ǉ������
            }
            else if (col.CompareTag("tEye") && IsFullyInside(circleCollider.bounds, col.bounds))
            {
                tAddPoints++; //�������̖ځA2��������5�ǉ������
            }
        }

        // �m�[�}���ڂ̃X�R�A����
        if (nAddPoints == 1) //�m�[�}���̖�:1�̎�
        {
            AddedScore += 1;
        }
        else if(nAddPoints == 2) //�m�[�}���̖�:2�̎�
        {
            AddedScore += 2;
        }
            
        // �������ڂ̃X�R�A����
        if (tAddPoints == 1) //�������̖�:1�̎�
        {
            AddedScore += 2;
        }
        else if (tAddPoints == 2) //�������̖�:1�̎�
        {
            AddedScore += 5;
        }

        // �X�R�A���Z�ƕ\��
        if (AddedScore > 0)
        {
            score += AddedScore;
            Debug.Log("Score: " + score);
        }
    }

    bool IsFullyInside(Bounds outer, Bounds inner)
    {
        return outer.Contains(inner.min) && outer.Contains(inner.max);
    }
}