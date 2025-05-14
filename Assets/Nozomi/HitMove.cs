using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitMove : MonoBehaviour
{
    Vector3 mousePos, objPos; //�}�E�X�ƃQ�[���I�u�W�F�N�g�̍��W�f�[�^���i�[
    Vector3 rightTopPos;
    Vector3 leftBottomPos;
    public float depth = 10.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //�}�E�X�̍��W���擾����
        mousePos = Input.mousePosition;
        //�X�N���[�����W�����[���h���W�ɕϊ�����
        objPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, depth));
        //���[���h���W���Q�[���I�u�W�F�N�g�̍��W�ɐݒ肷��
        transform.position = objPos;

        rightTopPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, depth));
        leftBottomPos = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, depth));

        //�I�u�W�F�N�g���X�N���[������͂ݏo���Ȃ��悤�ɂ�����
        if (objPos.x >= rightTopPos.x) 
        {
            objPos.x = rightTopPos.x;
        }
    }
}
