using System.Collections;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    Vector3 mousePos, pos;
    public static int score;
    Collider2D circleCollider;

    public Transform cameraCenter;

    bool canMove = true; //�ړ��\���ǂ���

    public ShutterEffect shutterEffect; //�V���b�^�[�G�t�F�N�g�ւ̎Q��

    //public bool Touched_red = false;

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
    /*
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "red")
        {
            Touched_red = true;
        }
        
        else
        {
            Touched_red = false;
        }
    }
    */

    //�X�R�A�̉��Z
    void AddScore()
    {
        Collider2D[] colliders = GameObject.FindObjectsOfType<Collider2D>();
        int nAddPoints = 0; //�m�[�}���̖�
        int tAddPoints = 0; //�������̖�
        int totalEyesScore = 0; //�m�[�}�� +�@�������̃X�R�A

        int nAddRarebonus_red = 0;
        int nAddRarebonus_blue = 0;

        //�e�R���C�_�[������g���ɓ����Ă��邩�̃`�F�b�N
        foreach (var col in colliders)
        {
            if (col.CompareTag("nEye") && IsFullyInside(circleCollider.bounds, col.bounds))
            {
                nAddPoints++;
                /*
                if (CompareTag("red"))
                {
                    nAddRarebonus_red += 50;
                }
                else if (CompareTag("blue"))
                {
                    nAddRarebonus_blue += 100;
                }*/
            }
            else if (col.CompareTag("tEye") && IsFullyInside(circleCollider.bounds, col.bounds))
            {
                tAddPoints++;
            }
        }

        //�m�[�}���̖ڂ̃|�C���g�̉��Z
        if (nAddPoints == 1)
        {
            totalEyesScore += 1;
        }
        else if (nAddPoints == 2)
        {
            totalEyesScore += 2;
        }

        //�������̖ڂ̃|�C���g�̉��Z
        if (tAddPoints == 1)
        {
            totalEyesScore += 2;
        }
        else if (tAddPoints == 2)
        {
            totalEyesScore += 5;
        }

        int totalEyes = nAddPoints + tAddPoints; //�ŏI�I�ɔ��肳���ڂ̐�
        int bonus = GetBonusPoint(totalEyes); //���肳�ꂽ�ڂ̐��ɂ��{�[�i�X

        //int Rarebonus = nAddRarebonus_red + nAddRarebonus_blue;

         int AddedScore = totalEyesScore + bonus/* + Rarebonus*/; //�ŏI�X�R�A

        if (AddedScore > 0)
        {
            score += AddedScore;

            Debug.Log("TotalEyesScore:" + totalEyesScore);
            Debug.Log("BonusScore:" + bonus);
            //Debug.Log("Rarebouns" + Rarebonus);
            Debug.Log("Score: " + score);
        }
    }

    //�{�[�i�X�X�R�A�̔z�_ case:�ڂ̐� return:�擾�X�R�A
    int GetBonusPoint(int eyes)
    {
        switch (eyes)
        {
            case 1:
            case 2:
                return 0;
            case 3:
                return 5;
            case 4:
                return 10;
            case 5:
                return 20;
            case 6:
                return 50;
            case 7:
                return 100;
            case 8:
                return 250;
            case 9:
                return 300;
            case 10:
                return 500;
            default:
                return 0;
        }
    }

    bool IsFullyInside(Bounds outer, Bounds inner)
    {
        return outer.Contains(inner.min) && outer.Contains(inner.max);
    }
}