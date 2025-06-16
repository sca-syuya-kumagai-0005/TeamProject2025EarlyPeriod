using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;


public class DisplayScores : MonoBehaviour
{
    [Header("�X�R�A�f�[�^")]
    [SerializeField] PointList pointlist;

    [Header("UI�\���e�L�X�g")]
    [SerializeField] Text NumberEyes;//�G�l�~�[�̖ڂ̐�
    [SerializeField] Text GostType;//�G�l�~�[�̎��
    [SerializeField] Text Rarity;//���A�x
    [SerializeField] Text BonusPoints;//�{�[�i�X�|�C���g
    [SerializeField] Text AddScore;//�݌vPoint
    
    [Header("�ʐ^�̕\���̐ݒ�")]
    [SerializeField] float WholePhotoTime = 1.5f;//������ʐ^�̕\������
    [SerializeField] float FocusedPhoto = 1.0f;//�s���g���ʐ^�̕\������
    [SerializeField] float Information = 1.0f;//���_�̏ڍא���

    [SerializeField] float cropAreaWidth = 0.2f;//�؂���̈�̕�
    [SerializeField] float cropAreaHeight = 0.2f;//�؂��鍂��
    [Header("�V�[���ڍs")]
    public string nextSceneName = "RankingScene";//���ړ���̃V�[����

    public List<GameObject> PhotoList = new List<GameObject>();//�ʐ^���i�[���郊�X�g
    [SerializeField]private List<GameObject> clonedEnemies = new List<GameObject>();//��������I�o�P�̃��X�g
    [SerializeField] GameObject cameraMask;//�}�X�N�S�̂̃I�u�W�F�N�g
    Transform PhotoObject;//�ʐ^�̃I�u�W�F�N�g

    //������/�X�L�b�v�̃t���O
    private bool skipRequested = false;
    private bool fastForwardRequested = false;

    [Header("�����肷��Ƃ��̔{���x")]
    [SerializeField]float acceleration = 0.3f;

    [Header("�ʐ^���ړ�����Ƃ��̍��W")]
    [SerializeField]
    Vector3 MaskPosition = new Vector3((float)-4.5,1,1);
    [SerializeField]
    Vector3 MaskScale = new Vector3((float)0.2,(float)0.2,1);


    Vector3 enemyCopyDestination = new Vector3(0,0,0);


    private int CumulativeScore = 0;//�݌v�X�R�A

    private void Awake()
    {
        //�V�[�����܂������I�o�P���擾
        cameraMask = GameObject.Find("PhotoStorage");
        if(cameraMask != null )
        {
            PhotoObject = cameraMask.transform;
            SceneManager.MoveGameObjectToScene(cameraMask, SceneManager.GetActiveScene());
            cameraMask.transform.position = MaskPosition;
            cameraMask.transform.localScale = MaskScale;
        }
        else
        {
            Debug.Log("�w�肳�ꂽ�I�u�W�F�N�g��������܂���");
        }
    }
    void Start()
    {
        if (PhotoObject == null) return;
        //UI�̕\�����Z�b�g
        ResetScoreUI();
            //�ʐ^�����X�g�����������APhotoStorage�̎q�v�f(�B�e�����ʐ^)�����X�g�ɒǉ�
            PhotoList.Clear();
            for (int i = 0; i < PhotoObject.childCount; i++)
            {
                PhotoList.Add(PhotoObject.GetChild(i).gameObject);
            }
        //�ʐ^�����Ԃɕ\������R���[�`�����J�n
        StartCoroutine(ProcessPhotos());
    }

    void Update()
    {
        //���[�U�[���͂̎�t
         HandleUserInput();
    }
    //�R���[�`��

    //�ʐ^�����Ԃɏ������郁�C���̃R���[�`��
    IEnumerator ProcessPhotos()
    {
        //�ʐ^���X�g���̊e�I�u�W�F�N�g�ɑ΂��ď������J��Ԃ�
        for(int i = 0;i < PhotoList.Count;i++)
        {
            Debug.Log(PhotoList.Count);
            //���X�g�͈̔͊O�ɂȂ�I��������
            if (i >= pointlist.point.Count)
            {
                break;
            }
            GameObject currentPhoto = PhotoList[i];
            var currentScoreData = pointlist.point[i];

            UpdataScores(currentScoreData);

            //�ʐ^�̓��_��݌v�ɒǉ�
            int photoScore = currentScoreData.eyes + currentScoreData.rarity + currentScoreData.bonus;
            CumulativeScore += photoScore;

            AddScore.text = $"{CumulativeScore}";
            //���̎ʐ^�̂��߂̃X�L�b�v�v�������Z�b�g
            skipRequested = false;

            //�ʐ^��1���\������V�[�P���X�R���[�`��
            yield return StartCoroutine(PhotoDisplay(currentPhoto));

            if (currentPhoto != null)
            {
                Destroy(currentPhoto);
            }
            Debug.Log(i+1 + "���ڏI��");
        }
        //�\���̏I��or�X�L�b�v���ꂽ �ʐ^�͔j��
        yield return new WaitForSeconds(1.0f);


        //���ׂĂ̎ʐ^�̏������I�������A���̃V�[���ɑJ�ڂ���
         //SceneManager.LoadScene(nextSceneName);
    }

    //�ʐ^1���̕\������
    IEnumerator PhotoDisplay(GameObject photo)
    {
        photo.SetActive(true);

        //�ʐ^�S�̂̕\��
        //�X�L�b�v���K�p���ꂽ�瑦�I����
        if(skipRequested)yield break;
        //�w�肳�ꂽ���Ԃ����ҋ@(��������l��)
        yield return new WaitForSeconds(GetInterval(WholePhotoTime));

        //�s���ƂȂ��ʐ^�\��

        //--�؂���@�\�ǉ�
        if (!skipRequested)
        {
            yield return new WaitForSeconds(GetInterval(FocusedPhoto));
            DuplicateAndMoveEnemies(photo);
        }

        if (skipRequested ) yield break;
        
        //���_�ڍׂ̕\��
        if(skipRequested ) yield break;
        yield return new WaitForSeconds(GetInterval(Information));
        // ��������Enemy�����ׂč폜
        foreach (GameObject enemy in clonedEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }
        clonedEnemies.Clear(); // ���X�g���N���A
    }

    /* void DuplicateAndMoveEnemies(GameObject photo)
     {
         // �q�I�u�W�F�N�g����"Enemy"�^�O�̂��̂�T��
         Transform[] children = photo.GetComponentsInChildren<Transform>();
         foreach (Transform child in children)
         {
             if (child.CompareTag("Enemy"))
             {
                 // ����
                 GameObject clone = Instantiate(child.gameObject);

                 // ���[���h���W���̂܂܂ŕ�����A�C�ӂ̈ʒu�Ɉړ�
                 clone.transform.position = enemyCopyDestination;
                 clone.transform.rotation = child.rotation;

                 // �I�v�V�����F�X�P�[�������Ɠ����ɂ���
                 clone.transform.localScale = child.lossyScale;
                 clone.name = child.name + "_Copy";

                 // �q�G�����L�[�����p�i�C�Ӂj
                 clone.name = child.name + "_Copy";
             }
         }
     }*/
    void DuplicateAndMoveEnemies(GameObject photo)
    {
        Transform[] children = photo.GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            if (child.CompareTag("Enemy"))
            {
                GameObject clone = Instantiate(child.gameObject);
                clone.transform.position = enemyCopyDestination;
                clone.transform.rotation = child.rotation;
                clone.transform.localScale = child.lossyScale;
                clone.name = child.name + "_Copy";

                // ���X�g�ɒǉ����Č�ŏ���
                clonedEnemies.Add(clone);
            }
        }
    }



    //�X�R�A�̏�����
    void ResetScoreUI()
    {
        NumberEyes.text = "_";
        GostType.text = "_";
        Rarity.text = "_";
        BonusPoints.text = "_";
        AddScore.text = "0";
    }



    // �X�R�A���W�v���AUI�e�L�X�g���X�V����
    void UpdataScores(EnemyData data)
    {
        if (PhotoList == null || pointlist.point == null)
        {
            Debug.LogError("PointList���ݒ肳��Ă��܂���B");
            return;
        }

        // UI�e�L�X�g�Ɍv�Z���ʂ𔽉f
        NumberEyes.text = $"{data.eyes}��";
        // ToDo: Coward, Furious�̃J�E���g���@�͌��X�N���v�g�Ŗ���`�̂��߁A��U0�ŕ\��
        GostType.text = $"0�́@0��";
        Rarity.text = $"{data.rarity}";
        BonusPoints.text = $"{data.bonus}";
    }


    //���[�U�[���͏���
    void HandleUserInput()
    {
        //�X�L�b�v����
        if (Input.GetKeyDown(KeyCode.S))
        {
            skipRequested = true;
            Debug.Log("�X�L�b�v����");
        }
        //�����菈��
        fastForwardRequested = Input.GetKey(KeyCode.F);
    }

    //�������Ԃ��l�������A�ҋ@����
    float GetInterval(float baseInterval)
    {
        //������v��������Ί�{���Ԃ�n�{�A�Ȃ���Ί�{���Ԃ����̂܂ܕԂ�
        return fastForwardRequested ? baseInterval* acceleration : baseInterval;
    }
}
