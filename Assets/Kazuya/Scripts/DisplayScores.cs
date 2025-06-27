using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

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
    [SerializeField] GameObject photoDisplayReference; // �ʐ^��\������ʒu�Ƒ傫���̊�ƂȂ�I�u�W�F�N�g
    [SerializeField] GameObject displayArea; // ���������I�o�P��\������͈͂ƂȂ�I�u�W�F�N�g
    [SerializeField] string maskName;

    [Header("�����肷��Ƃ��̔{���x")]
    [SerializeField] float acceleration = 0.3f;

    [SerializeField] GameObject cameraMask; // �V�[�����܂����ŉ^�΂�Ă����ʐ^�̐e�I�u�W�F�N�g("PhotoStorage")
    Transform photoContainer; // �ʐ^�I�u�W�F�N�g��Transform
    [SerializeField] List<GameObject> photoList = new List<GameObject>(); // �ʐ^���i�[���郊�X�g
    [SerializeField] List<GameObject> clonedEnemies = new List<GameObject>(); // ���������I�o�P�̃��X�g
    [Header("�ʐ^�̊g��{��")]
    [SerializeField] float Magnification =3.0f;

    int cumulativeScore = 0; // �݌v�X�R�A
                             //������/�X�L�b�v�̃t���O
    bool skipRequested = false;
    bool fastForwardRequested = false;

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
            SceneManager.LoadScene(nextSceneName);
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
    /// <summary>
    /// �ʐ^�����X�g�ɂ܂Ƃ߂�
    /// </summary>
    /// <returns></returns>
    IEnumerator ProcessPhotos()
    {
        for (int i = 0; i < photoList.Count; i++)
        {
            //if (i >= pointlist.point.Count)
            //{
            //    Debug.LogWarning("�ʐ^�̐��ƃX�R�A�f�[�^�̐�����v���Ȃ����߁A�����𒆒f���܂��B");
            //    break;
            //}

            GameObject currentPhoto = photoList[i];
            //var currentScoreData = pointlist.point[i];

            //UpdateScores(currentScoreData);

            //int photoScore = currentScoreData.eyes + currentScoreData.rarity + currentScoreData.bonus;
            //cumulativeScore += photoScore;
            //AddScore.text = $"{cumulativeScore}";

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
    /// <summary>
    /// �ʐ^��\������V�[�N�G���X
    /// </summary>
    /// <param name="photo">�ʐ^�̖��O</param>
    /// <returns></returns>
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
    /// <summary>
    /// �I�o�P���ʐ^����؂����ĕ�������
    /// </summary>
    /// <param name="photo">�ʐ^�I�u�W�F�N�g���O</param>
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
        Transform maskTransform = photo.transform.Find(maskName);
        if (maskTransform == null) { Debug.LogError("Mask�I�u�W�F�N�g���I������Ă��܂���:" + maskName); return; }

        GameObject maskObject = maskTransform.gameObject;
        if (maskObject.GetComponent<Collider>() == null)
        {
            maskObject.AddComponent<BoxCollider>();
        }

        Transform[] allDescendants = photo.GetComponentsInChildren<Transform>();

        bool maskDuplicated = false;

        // �S�Ă̎q���̒�����"Enemy"�^�O�����I�u�W�F�N�g��T��
        foreach (Transform descendant in allDescendants)
        {

            if (!descendant.CompareTag("Enemy"))  continue; 
            {
                //���C�L���X�g�ɂ�锻��
                Vector3 rayorigin = descendant.position;
                Vector3 rayDirection = (maskObject.transform.position - rayorigin).normalized;
                float rayDistance = Vector3.Distance(rayorigin, maskObject.transform.position);
                Ray ray = new Ray(rayorigin, rayDirection);
                RaycastHit hit;
                Debug.DrawRay(rayorigin, rayDirection * rayDistance, Color.red, 1.0f);
                Debug.Log("���C�̐ݒ�I��");
                //if (Physics.Raycast(rayorigin, rayDirection, out hit, rayDistance + 0.1f))
                {
                    //if (hit.transform == maskTransform)
                    {
                        if (!maskDuplicated)
                        {
                            Vector3 originalPos = maskObject.transform.position;

                            float relatveX = Mathf.InverseLerp(sourceBounds.min.x, sourceBounds.max.x, originalPos.x);
                            float relatveY = Mathf.InverseLerp(sourceBounds.min.y, sourceBounds.max.y, originalPos.y);
                            // 1. �X�P�[�������i�ʐ^�ƕ\���G���A�̔䗦�ɍ��킹�āj
                            float scaleRatiomaskX = destBounds.size.x / sourceBounds.size.x;
                            float scaleRatiomaskY = destBounds.size.y / sourceBounds.size.y;
                            float finalScaleRatiomask = Mathf.Min(scaleRatiomaskX, scaleRatiomaskY);

                            // 2. �������� center �ɕ\��
                            GameObject maskClone = Instantiate(maskObject.gameObject);

                            //  displayArea �̒��S�ɔz�u
                            maskClone.transform.position = new Vector3(destBounds.center.x, destBounds.center.y, destBounds.center.z + 0.01f);

                            // �X�P�[���E��]�̒���
                            maskClone.transform.localScale *= finalScaleRatiomask * Magnification;
                            maskClone.transform.rotation = maskObject.transform.rotation;
                            maskClone.name = maskObject.name + "_Copy";

                            clonedEnemies.Add(maskClone);
                            maskDuplicated = true;
                        }
                        Debug.Log("���C�����m�@�I�u�W�F�N�g�̕������J�n");
                        // �X�P�[���䗦
                        float scaleX = destBounds.size.x / sourceBounds.size.x;
                        float scaleY = destBounds.size.y / sourceBounds.size.y;
                        float finalScaleRatio = Mathf.Min(scaleX, scaleY);
                        //�}�X�N�̌����W�ƕ�����̒��S���W
                        Vector3 originalMaskPos = maskObject.transform.position;
                        Vector3 maskCenterPos = destBounds.center;

                        //�I�o�P�̕���
                        GameObject clone = Instantiate(descendant.gameObject);

                        Renderer cloneRenderer = clone.GetComponent<Renderer>();
                        if (cloneRenderer == null) { Debug.LogError("Renderer�̎Q�ƕs��"); return; }

                        if (cloneRenderer != null)
                        {
                            cloneRenderer.sortingOrder = 10;
                        }

                        Vector3 offset = descendant.position - originalMaskPos;
                        // �v�Z�����V�������[���h���W�𕡐������I�o�P�ɐݒ�
                        clone.transform.position =maskCenterPos+offset * finalScaleRatio;



                        //�v�Z�����䗦��K�p
                        clone.transform.localScale *= finalScaleRatio* Magnification;
                        clone.transform.rotation = descendant.rotation;
                        clone.name = descendant.name + "_Copy";
                        // ��ł܂Ƃ߂č폜���邽�߂Ƀ��X�g�ɒǉ�
                        clonedEnemies.Add(clone);

                    }
                }
            }
        }
        maskTransform = null;
    }
    /// <summary>
    /// �X�R�A�̏�����
    /// </summary>
    void ResetScoreUI()
    {
        NumberEyes.text = "_";
        GostType.text = "_";
        Rarity.text = "_";
        BonusPoints.text = "_";
        AddScore.text = "0";
    }
    /// <summary>
    /// �X�R�A���W�v���AUI�e�L�X�g���X�V����
    /// </summary>
    /// <param name="data"></param>
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

    /// <summary>
    /// ���[�U�[���͏���
    /// </summary>
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

    /// <summary>
    /// �������Ԃ��l�������A�ҋ@����
    /// </summary>
    /// <param name="baseInterval">�W���̑ҋ@����</param>
    /// <returns></returns>
    float GetInterval(float baseInterval)
    {
        //������v��������Ί�{���Ԃ�n�{�A�Ȃ���Ί�{���Ԃ����̂܂ܕԂ�
        return fastForwardRequested ? baseInterval * acceleration : baseInterval;
    }
}
