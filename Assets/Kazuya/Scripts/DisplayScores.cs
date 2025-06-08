using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;
using TMPro;
using UnityEngine.Experimental.GlobalIllumination;


public class DisplayScores : MonoBehaviour
{
    [Header("UI�\���e�L�X�g")]
    [SerializeField] Text NumberEyes;//�G�l�~�[�̖ڂ̐�
    [SerializeField] Text GostType;//�G�l�~�[�̎��
    [SerializeField] Text Rarity;//���A�x
    [SerializeField] Text BonusPoints;//�{�[�i�X�|�C���g
    [SerializeField] Text AddScore;//�݌vPoint
    [SerializeField]GameObject cameraMask;//�}�X�N�S�̂̃I�u�W�F�N�g
    Transform  PhotoObject;//�ʐ^�̃I�u�W�F�N�g
    
    [Header("�ʐ^�̕\���̐ݒ�")]
    [SerializeField] float WholePhotoTime = 1.5f;//������ʐ^�̕\������
    [SerializeField] float FocusedPhoto = 1.0f;//�s���g���ʐ^�̕\������
    [SerializeField] float Information = 1.0f;//���_�̏ڍא���



    public List<GameObject> PhotoList = new List<GameObject>();
    [Header("�V�[���ڍs")]
    public string nextSceneName = "RankingScene";//���ړ���̃V�[����

    private bool skipRequested = false;
    private bool fastForwardRequested = false;

    [Header("�����肷��Ƃ��̔{���x")]
    [SerializeField]float acceleration = 0.3f;


    Vector3 MaskPosition = new Vector3((float)-4.5,1,1);
    Vector3 MaskScale = new Vector3((float)0.2,(float)0.2,1);
    int Eyse;
    int Coward;
    int Furious;
    int Raritys;
    int BonusPointss;
    int Scores;
    private void Awake()
    {
        //�V�[�����܂������I�o�P���擾
        cameraMask = GameObject.Find("PhotoStorage").gameObject;
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
            //�ʐ^�����X�g�����������APhotoStorage�̎q�v�f(�B�e�����ʐ^)�����X�g�ɒǉ�
            PhotoList.Clear();
            for (int i = 0; i < PhotoObject.childCount; i++)
            {
                Transform child = PhotoObject.GetChild(i);
                PhotoList.Add(child.gameObject);
            }

        //�X�R�A���v�Z����UI���X�V
        CalculateAndDisplayScores();
        //�ʐ^�����Ԃɕ\������R���[�`�����J�n
        StartCoroutine(ProcessPhotos());

        //ProcessObjects();
    }

    void Update()
    {
        //���[�U�[���͂̎�t
         HandleUserInput();
    }

    /*��ŏ���
    void EnemyDataUpdate()
    {
        NumberEyes.text = $"{Eyse}��";
        GostType.text = $"{Coward}�́@{Furious}��";
        Rarity.text = $"{Raritys}";
        BonusPoints.text = $"{BonusPointss}";
        AddScore.text = $"{Scores}";
    }

    IEnumerator ProcessObjects()
    {
        foreach (GameObject obj in PhotoList)
        {
            Debug.Log("�\���J�n");
            yield return StartCoroutine(FlashObject(obj,1));
            Destroy(obj);
        }
        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator FlashObject(GameObject obj, int flashCount)
    {
        for (int i = 0; i < flashCount; i++)
        {
            yield return new WaitForSeconds(WholePhotoTime);
            obj.SetActive(true);
            yield return new WaitForSeconds(FocusedPhoto);
            yield return new WaitForSeconds(Information);
        }

    }*/
    //�R���[�`��

    //�ʐ^�����Ԃɏ������郁�C���̃R���[�`��
    IEnumerator ProcessPhotos()
    {
        //�ʐ^���X�g���̊e�I�u�W�F�N�g�ɑ΂��ď������J��Ԃ�
        foreach(GameObject photo in PhotoList)
        {
            //���̎ʐ^�̂��߂̃X�L�b�v�v�������Z�b�g
            skipRequested = false;

            //�ʐ^��1���\������V�[�P���X�R���[�`��
            yield return StartCoroutine(PhotoDisplay(photo));

            //�\���̏I��or�X�L�b�v���ꂽ �ʐ^�͔j��
            if(photo != null )Destroy(photo);
        }
        //���ׂĂ̎ʐ^�̏������I�������A���̃V�[���ɑJ�ڂ���
       // SceneManager.LoadScene(nextSceneName);
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
        if(skipRequested ) yield break;
        yield return new WaitForSeconds(GetInterval(FocusedPhoto));

        //���_�ڍׂ̕\��
        if(skipRequested ) yield break;
        yield return new WaitForSeconds(GetInterval(Information));
    }

    // �X�R�A���W�v���AUI�e�L�X�g���X�V����
    void CalculateAndDisplayScores()
    {
        //if (PhotoList == null || PhotoList.point == null)
        //{
        //    Debug.LogError("PointList���ݒ肳��Ă��܂���B");
        //    return;
        //}

        //var enemies = PhotoList.point;
        //int eyesTotal = enemies.Sum(e => e.eyes);
        //int rarityTotal = enemies.Sum(e => e.rarity);
        //int bonusTotal = enemies.Sum(e => e.bonus);
        //int scoreTotal = eyesTotal + rarityTotal + bonusTotal;

        //// UI�e�L�X�g�Ɍv�Z���ʂ𔽉f
        //NumberEyes.text = $"{eyesTotal}��";
        //// ToDo: Coward, Furious�̃J�E���g���@�͌��X�N���v�g�Ŗ���`�̂��߁A��U0�ŕ\��
        //GostType.text = $"0�́@0��";
        //Rarity.text = $"{rarityTotal}";
        //BonusPoints.text = $"{bonusTotal}";
        //AddScore.text = $"{scoreTotal}";
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
