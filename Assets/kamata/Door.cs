using TMPro;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("�h�A�ݒ�")]
    public Transform leftDoor;
    public Transform rightDoor;
    public float openAngle = 90f;
    public float delayBeforeOpen = 1.0f;
    public float doorOpenSpeed = 2f;

    [Header("�J�����ړ��ݒ�")]
    public Camera targetCamera;
    public Transform cameraTargetPosition;
    public float cameraMoveDelay = 2.0f;
    public float cameraMoveSpeed = 1.5f;

    [Header("�e�L�X�g���b�V���v���ݒ�")]
    public TextMeshProUGUI loopAlphaText;
    public float alphaLoopSpeed = 2.0f;
    public float minAlpha = 0.2f;
    public float maxAlpha = 1.0f;

    private Quaternion leftClosedRot;
    private Quaternion rightClosedRot;
    private Quaternion leftOpenRot;
    private Quaternion rightOpenRot;

    private bool doorIsOpening = false;
    private bool cameraIsMoving = false;
    private bool requestedSceneChange = false;

    private float timer = 0f;
    private float cameraMoveTimer = 0f;

    void Start()
    {
        leftClosedRot = leftDoor.localRotation;
        rightClosedRot = rightDoor.localRotation;

        leftOpenRot = leftClosedRot * Quaternion.Euler(0, 0, -openAngle);
        rightOpenRot = rightClosedRot * Quaternion.Euler(0, 0, openAngle);
    }

    void Update()
    {
        // �� ���N���b�N�ő��V�[���؂�ւ��iDoor�X�L�b�v�j
        if (!requestedSceneChange && Input.GetMouseButtonDown(0))
        {
            requestedSceneChange = true;
            RequestSceneChange();
            return;
        }

        // �� TextMeshPro �A���t�@���[�v�i�t�F�[�h���o�j
        if (loopAlphaText != null)
        {
            float alpha = Mathf.Lerp(minAlpha, maxAlpha, Mathf.PingPong(Time.time * alphaLoopSpeed, 1f));
            Color color = loopAlphaText.color;
            color.a = alpha;
            loopAlphaText.color = color;
        }

        // �� �h�A�J������
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

        // �� �J�����ړ������ƃV�[���J�ڔ���
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

            if (!requestedSceneChange &&
                Vector3.Distance(targetCamera.transform.position, cameraTargetPosition.position) < 0.1f)
            {
                requestedSceneChange = true;
                RequestSceneChange();
            }
        }
    }

    void RequestSceneChange()
    {
        var switcher = Object.FindFirstObjectByType<SceneLoopSwitcher>();
        if (switcher != null)
        {
            SceneLoopSwitcher.RequestSceneChange();
        }
        else
        {
            Debug.LogWarning("SceneLoopSwitcher ��������܂���B");
        }
    }
}
