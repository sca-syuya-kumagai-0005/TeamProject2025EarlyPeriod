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

    //public bool Touched_nred = false;
    //public bool Touched_nblue = false;
    
    //public bool Touched_tred = false;
    //public bool Touched_tblue = false;

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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "nred")
        {
            Touched_nred = true;
            Debug.Log("n�Ԃ����");
        }
        
        if(collision.gameObject.tag == "nblue")
        {
            Touched_nblue = true;
            Debug.Log("n�����");
        }
        
        if(collision.gameObject.tag == "tred")
        {
            Touched_tred = true;
            Debug.Log("t�Ԃ����");
        }
        
        if(collision.gameObject.tag == "tblue")
        {
            Touched_tblue = true;
            Debug.Log("t�����");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "nred")
        {
            Touched_nred = false;
            Debug.Log("n�Ԃ�����ĂȂ�");
        }
        
        if (collision.gameObject.tag == "nblue")
        {
            Touched_nblue = false;
            Debug.Log("n������ĂȂ�");
        }
        
        if (collision.gameObject.tag == "tred")
        {
            Touched_tred = false;
            Debug.Log("t�Ԃ�����ĂȂ�");
        }
        
        if (collision.gameObject.tag == "tblue")
        {
            Touched_tblue = false;
            Debug.Log("t������ĂȂ�");
        }
    }
    */

    //�X�R�A�̉��Z
    void AddScore()
    {
        Collider2D[] colliders = GameObject.FindObjectsOfType<Collider2D>();
        int nEye = 0; //�m�[�}���̖�
        int tEye = 0; //�������̖�
        int nRed = 0; //�������̖�
        int tRed = 0; //�������̖�
        int nBlue = 0; //�������̖�
        int tBlue = 0; //�������̖�

        int TotalEyesScore = 0; //�m�[�}�� +�@�������̃X�R�A

        int nRarebonus = 0;
        int tRarebonus = 0;

        //�e�R���C�_�[������g���ɓ����Ă��邩�̃`�F�b�N
        foreach (var col in colliders)
        {
            if (!IsFullyInside(circleCollider.bounds, col.bounds)) continue;

            switch (col.tag)
            {
                case "nEye":
                    nEye++;
                    break;
                case "tEye":
                    tEye++;
                    break;
                case "nred":
                    nRed++;
                    nRarebonus += 50;
                    break;
                case "nblue":
                    nBlue++;
                    nRarebonus += 100;
                    break;
                case "tred":
                    nRed++;
                    nRarebonus += 70;
                    break;
                case "tblue":
                    tBlue++;
                    tRarebonus += 120;
                    break;
            }
            /*
            if (col.CompareTag("nEye") && IsFullyInside(circleCollider.bounds, col.bounds))
            {
                nEye++;

            }
            else if (col.CompareTag("tEye") && IsFullyInside(circleCollider.bounds, col.bounds))
            {
                tEye++;
            }
            */
        }

        //�m�[�}���̖ڂ̃|�C���g�̉��Z
        if (nEye + nRed + nBlue == 1)
        {
            TotalEyesScore += 1;
        }
        else if (nEye + nRed + nBlue == 2)
        {
            TotalEyesScore += 2;
        }

        //�������̖ڂ̃|�C���g�̉��Z
        if (tEye + tRed + tBlue == 1)
        {
            TotalEyesScore += 2;
        }
        else if (tEye + tRed + tBlue == 2)
        {
            TotalEyesScore += 5;
        }
 

        int TotalEyes = nEye + tEye + nRed + tRed + nBlue + tBlue; //�ŏI�I�ɔ��肳���ڂ̐�

        int bonus = GetBonusPoint(TotalEyes); //���肳�ꂽ�ڂ̐��ɂ��{�[�i�X

         int AddedScore = TotalEyesScore + bonus + nRarebonus + tRarebonus; //�ŏI�X�R�A

        if (AddedScore > 0)
        {
            score += AddedScore;

            Debug.Log("TotalEyesScore:" + TotalEyesScore);
            Debug.Log("BonusScore:" + bonus);
            Debug.Log("nRarebouns" + nRarebonus);
            Debug.Log("tRarebouns" + tRarebonus);
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