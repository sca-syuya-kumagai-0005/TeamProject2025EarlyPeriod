using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using System.Linq;

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
    [SerializeField] Transform cloneRoot;//�\���G���A�ɔz�u����e�I�u�W�F�N�g

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

            UpdateScores();

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

        Transform maskTransform = photo.transform.Find(maskName);
        if (maskTransform == null) { Debug.LogError("Mask�I�u�W�F�N�g���I������Ă��܂���:" + maskName); return; }

        GameObject maskObject = maskTransform.gameObject;
        var maskCollider = maskObject.GetComponent<Collider>();
        if (maskCollider == null)
        {
            var tempCollider = maskObject.AddComponent<BoxCollider>();
            maskCollider = tempCollider;
        }

        var maskBounds = maskCollider.bounds;

        //. �������̎ʐ^�̑S�q�v�f����A�ʐ^�S�̂�Bounds���v�Z
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
        // 1. �S�̓I�ȏk�������v�Z�i�ʐ^���\���G���A�j
        float baseScaleRatio = Mathf.Min(destBounds.size.x / sourceBounds.size.x, destBounds.size.y / sourceBounds.size.y);
        GameObject maskClone = Instantiate(maskObject);
        maskClone.transform.position = destBounds.center;
        maskClone.transform.localScale = maskObject.transform.lossyScale * baseScaleRatio * Magnification;

        clonedEnemies.Add(maskClone);
        
        Transform[] allDescendants = photo.GetComponentsInChildren<Transform>();
        int count = 1;

        // �S�Ă̎q���̒�����"Enemy"�^�O�����I�u�W�F�N�g��T��
        foreach (Transform descendant in allDescendants)
        {
        if (!descendant.CompareTag("Enemy")) continue;
            // --- �X�e�b�v1: �܂��S�ẴI�o�P�𕡐����� ---
            // ����Ō���descendant�I�u�W�F�N�g�͈�ؕύX����܂���B
            GameObject clone = Instantiate(descendant.gameObject);

            // --- �X�e�b�v2: ���������N���[���̈ʒu�ƃX�P�[�����v�Z���� ---
            Vector3 originalCenterPos = maskObject.transform.position;
            Vector3 newCenterPos = destBounds.center;
            Vector3 relativePos = descendant.position - originalCenterPos;
            Vector3 newPosition = newCenterPos + (relativePos * baseScaleRatio * Magnification);

            clone.transform.position = newPosition;
            clone.transform.localScale = descendant.lossyScale * baseScaleRatio * Magnification;
            clone.transform.localScale = descendant.lossyScale * baseScaleRatio * Magnification;

            if (maskBounds.Contains(descendant.position))
            {
                Renderer cloneRenderer = clone.GetComponent<Renderer>();
                if (cloneRenderer != null) cloneRenderer.sortingOrder = 10;

                if (cloneRoot != null) clone.transform.SetParent(cloneRoot);

                clone.name = descendant.name + "_Copy" + $"{count}";
                clonedEnemies.Add(clone); // �\�����X�g�ɒǉ��i��ł܂Ƃ߂ď����j
                count++;
            }
            else
            {
                // �y�͈͊O�̏ꍇ�z -> ���̃I�u�W�F�N�g�������ɔj������
                Destroy(clone);
            }
        }
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
    void UpdateScores()
    {
        if (clonedEnemies.Count > 0)
        {
            GameObject maskClone = clonedEnemies[0]; // �ŏ��̗v�f�̓}�X�N
            int addedScore = CalculateScore(maskClone);
            cumulativeScore += addedScore;
            AddScore.text = $"{cumulativeScore}";
        }
    }


    int CalculateScore(GameObject maskClone)
    {
        int nEye = 0,tEye = 0,nRed = 0, tRed = 0,nBlue = 0,tBlue = 0;
        string[] validTags = {"nEye","tEye","nred","tred","nblue","tblue"};

        Collider maskCol = maskClone.GetComponent<Collider>();

        Bounds maskBounds = maskCol.bounds;
        foreach(GameObject ghot in clonedEnemies)
        {
            if(ghot == null) continue;

            Collider[] childCols = ghot.GetComponentsInChildren<Collider>();
            foreach(Collider col in childCols)
            {
                if (!validTags.Contains(col.tag)) continue;

                if (maskBounds.Contains(col.bounds.min) && maskBounds.Contains(col.bounds.max))
                {
                    switch (col.tag)
                    {
                        case "nEye": nEye++; break;
                        case "tEye": tEye++; break;
                        case "nred": nRed++; break;
                        case "tred": tRed++; break;
                        case "nblue": nBlue++; break;
                        case "tblue": tBlue++; break;
                    }
                }
            }
        }
        // �X�R�A�v�Z�iMouse.cs �Ɠ������[���j
        int score = 0;
        int normal = nEye + nRed + nBlue;
        score += (normal / 2) * 2;
        if (normal % 2 == 1) score += 1;

        int threaten = tEye + tRed + tBlue;
        score += (threaten / 2) * 5;
        if (threaten % 2 == 1) score += 2;

        score += GetRareBonus(nRed, 50);
        score += GetRareBonus(nBlue, 100);
        score += GetRareBonus(tRed, 70);
        score += GetRareBonus(tBlue, 120);

        score += GetBonusPoint(normal + threaten);
        return score;
    }

    int GetBonusPoint(int eyes)
    {
        switch (eyes)
        {
            case 3: return 5;
            case 4: return 10;
            case 5: return 20;
            case 6: return 50;
            case 7: return 100;
            case 8: return 250;
            case 9: return 300;
            case 10: return 500;
            default: return 0;
        }
    }

    int GetRareBonus(int count, int baseScore)
    {
        if (count == 0) return 0;
        if (count <= 2) return baseScore;
        if (count <= 4) return baseScore * 2;
        if (count <= 6) return baseScore * 3;
        if (count <= 8) return baseScore * 4;
        if (count <= 10) return baseScore * 5;
        return baseScore * 6;
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
