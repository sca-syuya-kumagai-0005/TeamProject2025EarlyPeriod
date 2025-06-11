using UnityEngine;

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
    public Camera targetCamera;                       // �ΏۃJ����
    public Transform cameraTargetPosition;            // �J�������ړ�����ړI�n�iTransform�j
    public float cameraMoveDelay = 2.0f;              // �h�A�J��A�J�����ړ����n�߂�܂ł̑ҋ@����
    public float cameraMoveSpeed = 1.5f;              // �J�����̈ړ����x

    private Quaternion leftClosedRot;
    private Quaternion rightClosedRot;
    private Quaternion leftOpenRot;
    private Quaternion rightOpenRot;

    private bool doorIsOpening = false;
    private bool cameraIsMoving = false;

    private float timer = 0f;
    private float cameraMoveTimer = 0f;

    void Start()
    {
        // ������]�̕ۑ�
        leftClosedRot = leftDoor.localRotation;
        rightClosedRot = rightDoor.localRotation;

        // Z���ɉ�]�i���E�t�����ɉ�]������j
        leftOpenRot = leftClosedRot * Quaternion.Euler(0, 0, -openAngle);
        rightOpenRot = rightClosedRot * Quaternion.Euler(0, 0, openAngle);

        timer = 0f;
        doorIsOpening = false;
        cameraIsMoving = false;
    }

    void Update()
    {
        // �h�A�J���܂ł̑ҋ@
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
            // �h�A�̊J�A�j���[�V����
            leftDoor.localRotation = Quaternion.Lerp(leftDoor.localRotation, leftOpenRot, Time.deltaTime * doorOpenSpeed);
            rightDoor.localRotation = Quaternion.Lerp(rightDoor.localRotation, rightOpenRot, Time.deltaTime * doorOpenSpeed);

            // �h�A��������x�J������J�����ړ��J�n����
            cameraMoveTimer += Time.deltaTime;
            if (!cameraIsMoving && cameraMoveTimer >= cameraMoveDelay)
            {
                cameraIsMoving = true;
            }
        }

        // �J�����̈ړ�����
        if (cameraIsMoving && targetCamera != null && cameraTargetPosition != null)
        {
            // �ʒu�̕��
            targetCamera.transform.position = Vector3.Lerp(
                targetCamera.transform.position,
                cameraTargetPosition.position,
                Time.deltaTime * cameraMoveSpeed
            );

            // �����̕�ԁi��]�j
            targetCamera.transform.rotation = Quaternion.Lerp(
                targetCamera.transform.rotation,
                cameraTargetPosition.rotation,
                Time.deltaTime * cameraMoveSpeed
            );
        }
    }
}
