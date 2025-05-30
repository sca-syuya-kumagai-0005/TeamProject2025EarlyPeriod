
using UnityEngine;

public class HitMove : MonoBehaviour
{
    Vector3 mousePos, objPos; //�}�E�X�ƃQ�[���I�u�W�F�N�g�̍��W�f�[�^���i�[
    Vector3 rightTopPos;
    Vector3 leftBottomPos;
    public float depth = 10.0f;
    public float mouseVertical = 2.0f;
    public float mouseWidth = 2.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        rightTopPos = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 10.0f));
        leftBottomPos = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 10.0f));

    }

    // Update is called once per frame
    void Update()
    {
        //�}�E�X�̍��W���擾����
        mousePos = Input.mousePosition;
        //�X�N���[�����W�����[���h���W�ɕϊ�����
        objPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, depth));
        //���[���h���W���Q�[���I�u�W�F�N�g�̍��W�ɐݒ肷��
        transform.position = new Vector2(Mathf.Clamp(objPos.x, leftBottomPos.x + mouseWidth, rightTopPos.x - mouseWidth), Mathf.Clamp(objPos.y, leftBottomPos.y + mouseVertical, rightTopPos.y - mouseVertical));

    }
}
