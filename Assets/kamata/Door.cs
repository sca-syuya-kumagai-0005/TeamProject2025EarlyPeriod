using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Door : MonoBehaviour
{
    public Transform leftDoor;
    public Transform rightDoor;

    [Tooltip("�h�A���J��Z���̊p�x�i���̒l�ŊJ���j")]
    public float openAngle = 90f;

    [Tooltip("�J���n�߂�܂ł̎��ԁi�b�j")]
    public float delayBeforeOpen = 1.0f;

    [Tooltip("�J������܂ł̑����i�傫���قǑ����j")]
    public float doorOpenSpeed = 2f;

    [Header("�J�����ړ��ݒ�")]
    public Camera targetCamera;
    public Transform cameraTargetPosition;
    public float cameraMoveDelay = 2.0f;
    public float cameraMoveSpeed = 1.5f;

    [Header("�V�[���؂�ւ��ݒ�")]
    [Tooltip("����Z���W���J��������������V�[����؂�ւ��܂�")]
    public float cameraTriggerZ = 5.0f;

    [Tooltip("�J�ڐ�̃V�[�����i�K��Build Settings�ɒǉ����Ă��������j")]
    public string nextSceneName;

    [Header("�e�L�X�g���b�V���v���ݒ�")]
    [Tooltip("�A���t�@�����[�v������TextMeshPro�I�u�W�F�N�g")]
    public TextMeshProUGUI loopAlphaText; // UGUI��
    // public TextMeshPro loopAlphaText; // 3D Text�łɂ������ꍇ�͂�����ɕύX

    [Tooltip("�A���t�@�̃��[�v���x")]
    public float alphaLoopSpeed = 2.0f;

    [Tooltip("�ŏ��A���t�@�l")]
    public float minAlpha = 0.2f;

    [Tooltip("�ő�A���t�@�l")]
    public float maxAlpha = 1.0f;

    private Quaternion leftClosedRot;
    private Quaternion rightClosedRot;
    private Quaternion leftOpenRot;
    private Quaternion rightOpenRot;

    private bool doorIsOpening = false;
    private bool cameraIsMoving = false;
    private bool sceneSwitched = false;

    private float timer = 0f;
    private float cameraMoveTimer = 0f;

    void Start()
    {
        leftClosedRot = leftDoor.localRotation;
        rightClosedRot = rightDoor.localRotation;

        leftOpenRot = leftClosedRot * Quaternion.Euler(0, 0, -openAngle);
        rightOpenRot = rightClosedRot * Quaternion.Euler(0, 0, openAngle);

        timer = 0f;
        doorIsOpening = false;
        cameraIsMoving = false;
        sceneSwitched = false;
    }

    void Update()
    {
        // �� ���N���b�N�ő��V�[���J��
        if (!sceneSwitched && Input.GetMouseButtonDown(0))
        {
            TrySwitchScene();
        }

        // �� �A���t�@�l���[�v�iTextMeshPro�j
        if (loopAlphaText != null)
        {
            float alpha = Mathf.Lerp(minAlpha, maxAlpha, Mathf.PingPong(Time.time * alphaLoopSpeed, 1f));
            Color currentColor = loopAlphaText.color;
            currentColor.a = alpha;
            loopAlphaText.color = currentColor;
        }

        // �h�A�̊J������
        if (!doorIsOpening)
        {
            timer += Time.deltaTime;
            if (timer >= delayBeforeOpen)
            {
                doorIsOpening = true;
            }
        }
        else
        {
            leftDoor.localRotation = Quaternion.Lerp(leftDoor.localRotation, leftOpenRot, Time.deltaTime * doorOpenSpeed);
            rightDoor.localRotation = Quaternion.Lerp(rightDoor.localRotation, rightOpenRot, Time.deltaTime * doorOpenSpeed);

            cameraMoveTimer += Time.deltaTime;
            if (!cameraIsMoving && cameraMoveTimer >= cameraMoveDelay)
            {
                cameraIsMoving = true;
            }
        }

        // �J�����̈ړ���Z���`�F�b�N
        if (cameraIsMoving && targetCamera != null && cameraTargetPosition != null)
        {
            targetCamera.transform.position = Vector3.Lerp(
                targetCamera.transform.position,
                cameraTargetPosition.position,
                Time.deltaTime * cameraMoveSpeed
            );

            targetCamera.transform.rotation = Quaternion.Lerp(
                targetCamera.transform.rotation,
                cameraTargetPosition.rotation,
                Time.deltaTime * cameraMoveSpeed
            );

            if (!sceneSwitched && targetCamera.transform.position.z >= cameraTriggerZ)
            {
                TrySwitchScene();
            }
        }
    }

    // �V�[���؂�ւ������i���ʉ��j
    void TrySwitchScene()
    {
        sceneSwitched = true;
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("�V�[�������ݒ肳��Ă��܂���I");
        }
    }
}
