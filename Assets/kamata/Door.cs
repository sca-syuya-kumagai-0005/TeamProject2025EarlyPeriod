using TMPro;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("ドア設定")]
    public Transform leftDoor;
    public Transform rightDoor;
    public float openAngle = 90f;
    public float delayBeforeOpen = 1.0f;
    public float doorOpenSpeed = 2f;

    [Header("カメラ移動設定")]
    public Camera targetCamera;
    public Transform cameraTargetPosition;
    public float cameraMoveDelay = 2.0f;
    public float cameraMoveSpeed = 1.5f;

    [Header("テキストメッシュプロ設定")]
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
        // ▼ 左クリックで即シーン切り替え（Doorスキップ）
        if (!requestedSceneChange && Input.GetMouseButtonDown(0))
        {
            requestedSceneChange = true;
            RequestSceneChange();
            return;
        }

        // ▼ TextMeshPro アルファループ（フェード演出）
        if (loopAlphaText != null)
        {
            float alpha = Mathf.Lerp(minAlpha, maxAlpha, Mathf.PingPong(Time.time * alphaLoopSpeed, 1f));
            Color color = loopAlphaText.color;
            color.a = alpha;
            loopAlphaText.color = color;
        }

        // ▼ ドア開き処理
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

        // ▼ カメラ移動処理とシーン遷移判定
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
            Debug.LogWarning("SceneLoopSwitcher が見つかりません。");
        }
    }
}
