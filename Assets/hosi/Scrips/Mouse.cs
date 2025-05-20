using System.Collections;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    Vector3 mousePos, pos;
    int score;
    Collider2D circleCollider;

    public Transform cameraCenter;

    bool canMove = true; //�ړ��\���ǂ���

    public ShutterEffect shutterEffect; //�V���b�^�[�G�t�F�N�g�ւ̎Q��

    void Start()
    {
        score = 0;
        circleCollider = GetComponent<Collider2D>();

        if (cameraCenter == null)
        {
            cameraCenter = transform.Find("CameraCenter"); //�q�I�u�W�F�N�g"CameraCenter"��T��

        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canMove) //�ړ��\�ŃN���b�N���ꂽ�Ƃ�
        {
            AddScore();                                     //�X�R�A�ǉ�
            StartCoroutine(DisableMovementForSeconds(3f));  //�ړ��̖�����
            shutterEffect.TriggerEffect();                  //�V���b�^�[�G�t�F�N�g�̔���

        }
        if (canMove)
        {
            //�}�E�X�̃X�N���[�����W�����[���h���W�ɕϊ�
            mousePos = Input.mousePosition;
            Vector3 desiredWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));

            //�J�������S�̃I�t�Z�b�g��ێ����Ȃ���ړ�����v�Z
            Vector3 offset = cameraCenter.position - transform.position;
            Vector3 desiredCameraCenterPos = desiredWorldPos + offset;
            Vector3 clampedCameraCenterPos = ClampToScreenBounds(desiredCameraCenterPos); //�ړ������ʔ͈͓��ɐ���

            transform.position = clampedCameraCenterPos - offset;
        }
    }

    //��莞�Ԉړ��𖳌�
    IEnumerator DisableMovementForSeconds(float seconds)
    {
        canMove = false;
        yield return new WaitForSeconds(seconds);
        canMove = true;
    }

    //�w��̃��[���h���W����ʓ��Ɏ��߂�
    Vector3 ClampToScreenBounds(Vector3 worldPos)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        float marginX = 0f;
        float marginY = 0f;

        //SpriteRenderer�̃T�C�Y���擾���ă}�[�W����ݒ�
        if (cameraCenter.TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
        {
            Vector3 extents = sr.bounds.extents;
            Vector3 screenExtents = Camera.main.WorldToScreenPoint(extents + cameraCenter.position) - Camera.main.WorldToScreenPoint(cameraCenter.position);

            marginX = screenExtents.x;
            marginY = screenExtents.y;
        }
        //��ʓ��Ɏ��܂�悤�ɃX�N���[�����W�𐧌�
        screenPos.x = Mathf.Clamp(screenPos.x, marginX, Screen.width - marginX);
        screenPos.y = Mathf.Clamp(screenPos.y, marginY, Screen.height - marginY);
        //�X�N���[�����W���Ăу��[���h���W�ɕϊ�
        return Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, screenPos.z));
    }
    
    //�X�R�A�̉��Z
    void AddScore()
    {
        Collider2D[] colliders = GameObject.FindObjectsOfType<Collider2D>();
        int nAddPoints = 0;
        int tAddPoints = 0;
        int AddedScore = 0;

        //�e�R���C�_�[������g���ɓ����Ă��邩�̃`�F�b�N
        foreach (var col in colliders)
        {
            if (col.CompareTag("nEye") && IsFullyInside(circleCollider.bounds, col.bounds))
            {
                nAddPoints++;
            }
            else if (col.CompareTag("tEye") && IsFullyInside(circleCollider.bounds, col.bounds))
            {
                tAddPoints++;
            }
        }

        if (nAddPoints == 1)
        {
            AddedScore += 1;
        }
        else if (nAddPoints == 2)
        {
            AddedScore += 2;
        }

        if (tAddPoints == 1)
        {
            AddedScore += 2;
        }
        else if (tAddPoints == 2)
        {
            AddedScore += 5;
        }

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