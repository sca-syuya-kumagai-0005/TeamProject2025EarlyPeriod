using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    Vector3 mousePos, pos; //�}�E�X�ƃQ�[���I�u�W�F�N�g�̍��W�f�[�^���i�[

    public bool AddPoint = false;
    int score;

    private void Start()
    {
        AddPoint = false;
        score = 0;
    }

    void Update()
    {
        //�}�E�X�̍��W���擾����
        mousePos = Input.mousePosition;
        //�}�E�X�ʒu���m�F
        //Debug.Log(mousePos);
        //�X�N���[�����W�����[���h���W�ɕϊ�����
        pos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
        //���[���h���W���Q�[���I�u�W�F�N�g�̍��W�ɐݒ肷��
        transform.position = pos;

        if(AddPoint == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                score++;
                Debug.Log(score);
            }
        }
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        AddPoint = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        AddPoint = false;
    }

}