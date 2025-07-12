using UnityEngine;

public class Cursor : MonoBehaviour
{
    [Header("�N�����ɃJ�[�\�����\���ɂ���")]
    public bool hideOnStart = true;

    void Start()
    {
        if (hideOnStart)
        {
            UnityEngine.Cursor.visible = false; // �����ڂ�����\��
        }
    }

    void OnDestroy()
    {
        // �V�[�����؂�ւ���Ă��̃I�u�W�F�N�g���j�����ꂽ��A�J�[�\���\���𕜌�
        UnityEngine.Cursor.visible = true;
    }
}
