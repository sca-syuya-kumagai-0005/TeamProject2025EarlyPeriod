using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform leftDoor;
    public Transform rightDoor;

    [Tooltip("ドアを開くZ軸の角度（正の値で開く）")]
    public float openAngle = 90f;

    [Tooltip("開き始めるまでの時間（秒）")]
    public float delayBeforeOpen = 1.0f;

    [Tooltip("開ききるまでの速さ（大きいほど速い）")]
    public float doorOpenSpeed = 2f;

    [Header("カメラ移動設定")]
    public Camera targetCamera;                       // 対象カメラ
    public Transform cameraTargetPosition;            // カメラが移動する目的地（Transform）
    public float cameraMoveDelay = 2.0f;              // ドア開後、カメラ移動を始めるまでの待機時間
    public float cameraMoveSpeed = 1.5f;              // カメラの移動速度

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
        // 初期回転の保存
        leftClosedRot = leftDoor.localRotation;
        rightClosedRot = rightDoor.localRotation;

        // Z軸に回転（左右逆方向に回転させる）
        leftOpenRot = leftClosedRot * Quaternion.Euler(0, 0, -openAngle);
        rightOpenRot = rightClosedRot * Quaternion.Euler(0, 0, openAngle);

        timer = 0f;
        doorIsOpening = false;
        cameraIsMoving = false;
    }

    void Update()
    {
        // ドア開くまでの待機
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
            // ドアの開閉アニメーション
            leftDoor.localRotation = Quaternion.Lerp(leftDoor.localRotation, leftOpenRot, Time.deltaTime * doorOpenSpeed);
            rightDoor.localRotation = Quaternion.Lerp(rightDoor.localRotation, rightOpenRot, Time.deltaTime * doorOpenSpeed);

            // ドアがある程度開いたらカメラ移動開始準備
            cameraMoveTimer += Time.deltaTime;
            if (!cameraIsMoving && cameraMoveTimer >= cameraMoveDelay)
            {
                cameraIsMoving = true;
            }
        }

        // カメラの移動処理
        if (cameraIsMoving && targetCamera != null && cameraTargetPosition != null)
        {
            // 位置の補間
            targetCamera.transform.position = Vector3.Lerp(
                targetCamera.transform.position,
                cameraTargetPosition.position,
                Time.deltaTime * cameraMoveSpeed
            );

            // 向きの補間（回転）
            targetCamera.transform.rotation = Quaternion.Lerp(
                targetCamera.transform.rotation,
                cameraTargetPosition.rotation,
                Time.deltaTime * cameraMoveSpeed
            );
        }
    }
}
