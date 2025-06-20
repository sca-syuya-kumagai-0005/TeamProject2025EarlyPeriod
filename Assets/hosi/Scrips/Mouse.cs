using System.Collections;
using TMPro;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    Vector3 mousePos, pos;
    public static int score;
    Collider2D circleCollider;

    public Transform cameraCenter;

    bool canMove = true; //�ړ��\���ǂ���

    public ShutterEffect shutterEffect; //�V���b�^�[�G�t�F�N�g�ւ̎Q��

    public GameObject ScoreTextPrefab;

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
        int nEye = 0; //�m�[�}���̖�
        int tEye = 0; //�������̖�
        int nRed = 0; //�m�[�}���Ԃ̖�
        int tRed = 0; //�������Ԃ̖�
        int nBlue = 0; //�m�[�}���̖�
        int tBlue = 0; //�������̖�

        int TotalEyesScore = 0; //�m�[�}�� +�@�������̃X�R�A

        int nRarebonus = 0;
        int tRarebonus = 0;

        //�e�R���C�_�[������g���ɓ����Ă��邩�̃`�F�b�N
        foreach (var col in colliders)
        {
            // �}�E�X�͈͂Ɋ��S�ɓ����Ă��邩�m�F
            if (!IsFullyInside(circleCollider.bounds, col.bounds))
            {
                continue;//���S�ɓ����Ă����玟��
            }

            Vector3 offset = new Vector3(0, 0.5f, 0); // �����̃I�t�Z�b�g
            Vector3 spawnPos = col.bounds.center + offset;
            ScoreText(spawnPos);


            //�^�O�Ŗڂ̎�ނ��m�F
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
                    break;
                case "nblue":
                    nBlue++;
                    break;
                case "tred":
                    tRed++;
                    break;
                case "tblue":
                    tBlue++;
                    break;
            }
        }

        int nTotalEye = nEye + nRed + nBlue;
        int nPieces = nTotalEye / 2;
        int nSurplus = nTotalEye % 2;
        //�m�[�}���̖ڂ̃|�C���g�̉��Z
        for(int i = 0; i < nPieces; i++)
        {
            TotalEyesScore += 2;
        }
        if(nSurplus == 1)
        {
            TotalEyesScore += 1;
        }

        int tTotalEye = tEye + tRed + tBlue;
        int tPieces = tTotalEye / 2;
        int tSurplus = tTotalEye % 2;

        //�������̖ڂ̃|�C���g�̉��Z
        for (int i = 0; i < tPieces; i++)
        {
            TotalEyesScore += 5;
        }
        if (tSurplus == 1)
        {
            TotalEyesScore += 2;
        }


        int TotalEyes = nEye + tEye + nRed + tRed + nBlue + tBlue; //�ŏI�I�ɔ��肳���ڂ̐�

        int bonus = GetBonusPoint(TotalEyes); //���肳�ꂽ�ڂ̐��ɂ��{�[�i�X

        //�F�ɂ�郌�A�{�[�i�X
        nRarebonus += GetRerebounus(nRed, 50);
        nRarebonus += GetRerebounus(nBlue, 100);
        tRarebonus += GetRerebounus(tRed, 70);
        tRarebonus += GetRerebounus(tBlue, 120);

        int AddedScore = TotalEyesScore + bonus + nRarebonus + tRarebonus; //�ŏI�X�R�A

        if (AddedScore > 0)
        {
            score += AddedScore;

            Debug.Log("TotalEyes:" + TotalEyes);
            Debug.Log("TotalEyesScore:" + TotalEyesScore);
            Debug.Log("BonusScore:" + bonus);
            Debug.Log("nRarebouns" + nRarebonus);
            Debug.Log("tRarebouns" + tRarebonus);
            Debug.Log("Score: " + score);
        }
    }

    /// <summary>
    /// �{�[�i�X�X�R�A�̔z�_
    /// </summary>
    /// <param name="eyes">�ڂ̐�</param>
    /// <returns>�擾�X�R�A</returns>
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


    /// <summary>
    /// �F�ɂ�郌�A�{�[�i�X�̖ڂ̐�(���݂T�̂܂�)
    /// </summary>
    /// <param name="rEye">���A�̖ڂ̐�</param>
    /// <param name="RereScore">���ɂ��X�R�A</param>
    /// <returns>���ȏ�̎� +0 �X�R�A</returns>
    int GetRerebounus(int rEye, int RereScore)
    {
        if (rEye == 0)              //0��
        {
            return 0;
        }
        else if (rEye <= 2)         //1��
        {
            return RereScore;
        }
        else if (rEye <= 4)         //�Q��
        {
            return RereScore * 2;
        }
        else if (rEye <= 6)         //3��
        {
            return RereScore * 3;
        }
        else if (rEye <= 8)         //�S��
        {
            return RereScore * 4;
        }
        else if (rEye <= 10)        //5��
        {
            return RereScore * 5;
        }
        else return 0;
    }

    void ScoreText(Vector3 worldPosition)
    {
        if (ScoreTextPrefab != null)
        {
            GameObject text = Instantiate(ScoreTextPrefab, worldPosition, Quaternion.identity);
            Destroy(text, 1.0f); // 1�b�ŏ�����
        }
    }

    bool IsFullyInside(Bounds outer, Bounds inner)
    {
        return outer.Contains(inner.min) && outer.Contains(inner.max);
    }
}