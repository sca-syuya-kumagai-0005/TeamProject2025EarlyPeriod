/*
    [Header("�����肷��Ƃ��̔{���x")]
    [SerializeField]float acceleration = 0.3f;

    [Header("UI�̊�I�u�W�F�N�g")]
    [SerializeField] GameObject photoDisplayReference;//�ʐ^��\������ʒu�Ƒ傫���̊�ƂȂ�I�u�W�F�N�g
    [SerializeField] GameObject displayArea;//���������I�o�P��\������͈͂ƂȂ�I�u�W�F�N�g

    [Header("�ʐ^�̃I�u�W�F�N�g��")]
    [SerializeField] GameObject cameraMask;//�V�[�����܂����ł����ʐ^�̐e�I�u�W�F�N�g
    Transform photoContainer;//�ʐ^�̃I�u�W�F�N�g
    [SerializeField] List<GameObject> photoList = new List<GameObject>();//�ʐ^���i�[���郊�X�g
    [SerializeField] List<GameObject> clonedEnemies = new List<GameObject>();//��������I�o�P�̃��X�g

    private int cumulativeScore = 0;//�݌v�X�R�A
                                    //������/�X�L�b�v�̃t���O
    private bool skipRequested = false;
    private bool fastForwardRequested = false;

    private void Awake()
    {
        //�V�[�����܂������e�I�u�W�F�N�g���擾
        cameraMask = GameObject.Find("PhotoStorage");
        if(cameraMask == null)
        {
            Debug.LogError("PhotoStorage��������Ȃ���");
            return;
        }
        //��ƂȂ�I�u�W�F�N�g���ݒ肳��Ă��邩
        if(photoDisplayReference == null)
        {
            Debug.LogError("��ƂȂ�I�u�W�F�N�g��������Ȃ���");
            return;
        }
        //�ʐ^�I�u�W�F�N�g�����݂̃V�[���Ɉړ�
        SceneManager.MoveGameObjectToScene(cameraMask, SceneManager.GetActiveScene());
        photoContainer = cameraMask.transform;

        //�\����̊�I�u�W�F�N�g���擾
        var targetzBounds = photoDisplayReference.GetComponent<Collider>().bounds;

        Renderer[] childRenderers = cameraMask.GetComponentsInChildren<Renderer>();
        if(childRenderers.Length == 0)
        {
            Debug.LogError("�q�I�u�W�F�N�g�ɁARenderer�����Ă܂���");
            return;
        }

        //�S�Ă̎q���͂�Bounds���v�Z
        Bounds totalBounds = childRenderers[0].bounds;
        for(int i = 1; i < childRenderers.Length; i++)
        {
            totalBounds.Encapsulate(childRenderers[i].bounds);
        }
        
        cameraMask.transform.position = targetzBounds.center;

        //��ƂȂ�I�u�W�F�N�g�̃T�C�Y�ƁA�v�Z�����ʐ^�S�̂̃T�C�Y�̔䗦��r
        float scaleX = targetzBounds.size.x / totalBounds.size.x;
        float scaleY = targetzBounds.size.y / totalBounds.size.y;

        float finalScaleRatio = Mathf.Min(scaleX, scaleY);

        //�v�Z�����䗦��K�p
        cameraMask.transform.localScale *= finalScaleRatio;
    }
    void Start()
    {
        if (photoContainer == null) 
        {
            Debug.LogError("�ʐ^�I�u�W�F�N�g������������Ă��܂���"); 
            return; 
        }
        //UI�̕\�����Z�b�g
        ResetScoreUI();
            //�ʐ^�����X�g�����������APhotoStorage�̎q�v�f(�B�e�����ʐ^)�����X�g�ɒǉ�
            photoList.Clear();
            for (int i = 0; i < photoContainer.childCount; i++)
            {
                photoList.Add(photoContainer.GetChild(i).gameObject);
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
        for(int i = 0;i < photoList.Count;i++)
        {
            Debug.Log(photoList.Count);
            //���X�g�͈̔͊O�ɂȂ�I��������
            if (i >= pointlist.point.Count)
            {
                Debug.Log("���X�g�͈̔͊O�ɂȂ�܂����B");
                break;
            }
            GameObject currentPhoto = photoList[i];
            var currentScoreData = pointlist.point[i];

            UpdateScores(currentScoreData);

            //�ʐ^�̓��_��݌v�ɒǉ�
            int photoScore = currentScoreData.eyes + currentScoreData.rarity + currentScoreData.bonus;
            cumulativeScore += photoScore;

            AddScore.text = $"{cumulativeScore}";
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
         SceneManager.LoadScene(nextSceneName);
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
    void DuplicateAndMoveEnemies(GameObject photo)
    {
        //�\����͈̔͂ƂȂ�I�u�W�F�N�g�Ƃ���Collider���擾
        if(displayArea == null)
        {
            Debug.LogError("�ΏۂƂȂ�I�u�W�F�N�g���ݒ肳��Ă��܂���");
            return;
        }
        var destCollider = displayArea.GetComponent<Collider>();
        if(destCollider ==  null )
        {
            Debug.LogError("�R���C�_�[���ݒ肳��Ă��܂���");
            return;
        }

        //�������̎ʐ^�I�u�W�F�N�g�Ƃ��̃R���C�_�[�̎擾
        Renderer[] sourceCollider = photo.GetComponentsInChildren<Renderer>();
        if(sourceCollider == null)
        {
            Debug.LogError("�ʐ^�ɃR���C�_�[�����ĂȂ���");
            return;
        }

        //���ꂼ���Collider����A���[���h��Ԃł̋��E���擾
        Bounds sourceBounds = sourceCollider[0].bounds;
        for(int i = 1; i < sourceCollider.Length; i++)
        {
            sourceBounds.Encapsulate(sourceCollider[i].bounds);
        }

        var destBounds = destCollider.bounds;

        //�ʐ^����"Enemy"�^�O�����I�u�W�F�N�g��S�ĒT��
        foreach (Transform child in photo.transform)
        {
            if (child.CompareTag("Enemy"))
            {
                // �I�o�P�̕���
                GameObject clone = Instantiate(child.gameObject);

                //���̍��W���A���͈̔͂̂ǂ̂��炢�̊����ɂȂ邩�̌v�Z

                float relativeX = Mathf.InverseLerp(sourceBounds.min.x, sourceBounds.max.x, child.position.x);
                float relativeY = Mathf.InverseLerp(sourceBounds.min.y,sourceBounds.max.y, child.position.y);

                //�V�����͈͓��ō��W������
                float newX = Mathf.Lerp(destBounds.min.x,destBounds.max.x, relativeX);
                float newY = Mathf.Lerp(destBounds.min.y,destBounds.max.y, relativeY);
                float newZ = destBounds.center.z;

                //�v�Z�������W�𕡐������I�o�P�̐ݒ�
                clone.transform.localPosition = new Vector3(newX, newY, newZ);
                //��I�u�W�F�N�g�̃T�C�Y�ƌ��݂̃X�P�[���̔䗦���v�Z
                float scaleX = destBounds.size.x / sourceBounds.size.x;
                float scaleY = destBounds.size.y / sourceBounds.size.y;

                // �c������ێ����邽�߁A�䗦�̏����������̗p
                float finalScaleRatio = Mathf.Min(scaleX, scaleY);
                //�v�Z�����䗦��K�p
            }
        }
    }




}*/

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

    [Header("�V�[���ڍs")]
    public string nextSceneName = "RankingScene";//���ړ���̃V�[����

    [Header("UI�̊�I�u�W�F�N�g")]
    [SerializeField]  GameObject photoDisplayReference; // �ʐ^��\������ʒu�Ƒ傫���̊�ƂȂ�I�u�W�F�N�g
    [SerializeField]  GameObject displayArea; // ���������I�o�P��\������͈͂ƂȂ�I�u�W�F�N�g

    [Header("�����肷��Ƃ��̔{���x")]
    [SerializeField] float acceleration = 0.3f;

     [SerializeField]GameObject cameraMask; // �V�[�����܂����ŉ^�΂�Ă����ʐ^�̐e�I�u�W�F�N�g("PhotoStorage")
     Transform photoContainer; // �ʐ^�I�u�W�F�N�g��Transform
    [SerializeField] List<GameObject> photoList = new List<GameObject>(); // �ʐ^���i�[���郊�X�g
    [SerializeField] List<GameObject> clonedEnemies = new List<GameObject>(); // ���������I�o�P�̃��X�g

     int cumulativeScore = 0; // �݌v�X�R�A
    //������/�X�L�b�v�̃t���O
     bool skipRequested = false;
     bool fastForwardRequested = false;

    private void Awake()
    {

    }

    void Start()
    {
        // �V�[�����܂������ʐ^�̐e�I�u�W�F�N�g���擾
        cameraMask = GameObject.Find("PhotoStorage");
        if (cameraMask == null)
        {
            Debug.LogError("PhotoStorage�I�u�W�F�N�g��������܂���B");
            return;
        }

        // �\���ʒu�̊�ƂȂ�I�u�W�F�N�g���ݒ肳��Ă��邩�m�F
        if (photoDisplayReference == null)
        {
            Debug.LogError("��I�u�W�F�N�g(photoDisplayReference)���ݒ肳��Ă��܂���B");
            return;
        }

        // �ʐ^�I�u�W�F�N�g�����݂̃V�[���Ɉړ�
        SceneManager.MoveGameObjectToScene(cameraMask, SceneManager.GetActiveScene());
        photoContainer = cameraMask.transform;

        // --- �ʐ^�S�̂̕\���ʒu�ƃX�P�[������������ ---

        // 1. �\����̊�I�u�W�F�N�g��Bounds���擾
        var targetBounds = photoDisplayReference.GetComponent<Collider>().bounds;

        // 2. �ʐ^�̑S�q�v�f��Renderer����A�ʐ^�S�̂�Bounds���v�Z
        Renderer[] childRenderers = cameraMask.GetComponentsInChildren<Renderer>(true);
        Debug.Log(childRenderers.Length);
        if (childRenderers.Length == 0)
        {
            Debug.LogError("PhotoStorage�̎q�I�u�W�F�N�g�ɁA�\�����邽�߂�Renderer�R���|�[�l���g��������܂���B");
            return;
        }

        // �S�Ă̎q���͂�Bounds���v�Z�i�ŏ��̎q��Bounds�ŏ��������A�c����������Ă����j
        Bounds totalSourceBounds = childRenderers[0].bounds;
        for (int i = 1; i < childRenderers.Length; i++)
        {
            totalSourceBounds.Encapsulate(childRenderers[i].bounds);
        }

        // 3. �ʐ^�̈ʒu����I�u�W�F�N�g�̒����ɐݒ�
        cameraMask.transform.position = new Vector3(targetBounds.center.x, targetBounds.center.y, 70); //targetBounds.center;

        // 4. ��I�u�W�F�N�g�̃T�C�Y�Ǝʐ^�S�̂̃T�C�Y�̔䗦���v�Z
        float scaleX = targetBounds.size.x / totalSourceBounds.size.x;
        float scaleY = targetBounds.size.y / totalSourceBounds.size.y;

        // �c������ێ����邽�߁AX��Y�̔䗦�̂��������������̗p
        float finalScaleRatio = Mathf.Min(scaleX, scaleY);

        // 5. �v�Z�����䗦�����݂̃X�P�[���ɓK�p
        cameraMask.transform.localScale *= finalScaleRatio;
        if (photoContainer == null)
        {
            Debug.LogError("�ʐ^�I�u�W�F�N�g������������Ă��܂���B");
            return;
        }

        ResetScoreUI();

        // PhotoStorage�̎q�v�f�i�B�e�����ʐ^�j�����X�g�ɒǉ�
        photoList.Clear();
        for (int i = 0; i < photoContainer.childCount; i++)
        {
            photoList.Add(photoContainer.GetChild(i).gameObject);
        }

        StartCoroutine(ProcessPhotos());
    }

    void Update()
    {
        HandleUserInput();
    }

    IEnumerator ProcessPhotos()
    {
        for (int i = 0; i < photoList.Count; i++)
        {
            if (i >= pointlist.point.Count)
            {
                Debug.LogWarning("�ʐ^�̐��ƃX�R�A�f�[�^�̐�����v���Ȃ����߁A�����𒆒f���܂��B");
                break;
            }

            GameObject currentPhoto = photoList[i];
            var currentScoreData = pointlist.point[i];

            UpdateScores(currentScoreData);

            int photoScore = currentScoreData.eyes + currentScoreData.rarity + currentScoreData.bonus;
            cumulativeScore += photoScore;
            AddScore.text = $"{cumulativeScore}";

            skipRequested = false;

            yield return StartCoroutine(PhotoDisplaySequence(currentPhoto));

            if (currentPhoto != null)
            {
                Destroy(currentPhoto);
            }
        }

        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator PhotoDisplaySequence(GameObject photo)
    {
        photo.SetActive(true);

        if (skipRequested) yield break;
        yield return new WaitForSeconds(GetInterval(WholePhotoTime));

        if (skipRequested) yield break;
        DuplicateAndMoveEnemies(photo);
        yield return new WaitForSeconds(GetInterval(FocusedPhoto));

        if (skipRequested) yield break;
        yield return new WaitForSeconds(GetInterval(Information));

        // ��������Enemy�����ׂč폜
        foreach (GameObject enemy in clonedEnemies)
        {
            if (enemy != null) Destroy(enemy);
        }
        clonedEnemies.Clear();
    }

    void DuplicateAndMoveEnemies(GameObject photo)
    {
        // 1. �\����͈̔̓I�u�W�F�N�g�Ƃ���Collider���擾
        if (displayArea == null)
        {
            Debug.LogError("�I�o�P�̕\���͈�(displayArea)���ݒ肳��Ă��܂���B");
            return;
        }
        var destCollider = displayArea.GetComponent<Collider>();
        if (destCollider == null)
        {
            Debug.LogError("displayArea��Collider���A�^�b�`����Ă��܂���B");
            return;
        }

        // 2. �������̎ʐ^�̑S�q�v�f����A�ʐ^�S�̂�Bounds���v�Z
        Renderer[] sourceRenderers = photo.GetComponentsInChildren<Renderer>();
        if (sourceRenderers.Length == 0)
        {
            Debug.LogError("�������̎ʐ^�ɕ\���\�Ȏq�v�f(Renderer)������܂���B");
            return;
        }
        Bounds sourceBounds = sourceRenderers[0].bounds;
        for (int i = 1; i < sourceRenderers.Length; i++)
        {
            sourceBounds.Encapsulate(sourceRenderers[i].bounds);
        }

        var destBounds = destCollider.bounds;

        Transform[] allDescendants = photo.GetComponentsInChildren<Transform>();

        // �S�Ă̎q���̒�����"Enemy"�^�O�����I�u�W�F�N�g��T��
        foreach (Transform descendant in allDescendants)
        {
            if (descendant.CompareTag("Enemy"))
            {
                GameObject clone = Instantiate(descendant.gameObject);

                Renderer cloneRenderer = clone.GetComponent<Renderer>();
                if(cloneRenderer != null)
                {
                    cloneRenderer.sortingOrder = 10;
                }

                // --- ���W�ƃX�P�[���̃}�b�s���O ---
                // ���̍��W���A���͈̔�(sourceBounds)�̂ǂ̂��炢�̊����̈ʒu�ɂ��邩���v�Z
                float relativeX = Mathf.InverseLerp(sourceBounds.min.x, sourceBounds.max.x, descendant.position.x);
                float relativeY = Mathf.InverseLerp(sourceBounds.min.y, sourceBounds.max.y, descendant.position.y);

                // �v�Z�����������g���āA�V�����͈�(destBounds)���ł̍��W������
                float newX = Mathf.Lerp(destBounds.min.x, destBounds.max.x, relativeX);
                float newY = Mathf.Lerp(destBounds.min.y, destBounds.max.y, relativeY);

                // �v�Z�����V�������[���h���W�𕡐������I�o�P�ɐݒ�
                clone.transform.position = new Vector3(newX, newY, destBounds.center.z);

                // �X�P�[�����ʐ^�S�̂̏k�����ɍ��킹�Ē���
                float scaleRatioX = destBounds.size.x / sourceBounds.size.x;
                float scaleRatioY = destBounds.size.y / sourceBounds.size.y;
                float finalScaleRatio = Mathf.Min(scaleRatioX, scaleRatioY);
                
                Debug.Log(finalScaleRatio);
                //�v�Z�����䗦��K�p
                clone.transform.localScale *= finalScaleRatio;
                clone.transform.rotation = descendant.rotation;
                clone.name = descendant.name + "_Copy";
                // ��ł܂Ƃ߂č폜���邽�߂Ƀ��X�g�ɒǉ�
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
    void UpdateScores(EnemyData data)
    {
        if (pointlist.point == null)
        {
            Debug.LogError("PointList���ݒ肳��Ă��܂���B");
            return;
        }
        // UI�e�L�X�g�Ɍv�Z���ʂ𔽉f
        NumberEyes.text = $"{data.eyes}��";
        // ToDo: Coward, Furious�̃J�E���g���@�͌��X�N���v�g�Ŗ���`�̂��߁A��U0�ŕ\��
        GostType.text = $"0�� 0��"; // ToDo
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
        }
        //�����菈��
        fastForwardRequested = Input.GetKey(KeyCode.F);
    }

    //�������Ԃ��l�������A�ҋ@����
    float GetInterval(float baseInterval)
    {
        //������v��������Ί�{���Ԃ�n�{�A�Ȃ���Ί�{���Ԃ����̂܂ܕԂ�
        return fastForwardRequested ? baseInterval * acceleration : baseInterval;
    }
}
