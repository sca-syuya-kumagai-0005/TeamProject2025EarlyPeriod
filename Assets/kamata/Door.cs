using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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
    public Camera targetCamera;
    public Transform cameraTargetPosition;
    public float cameraMoveDelay = 2.0f;
    public float cameraMoveSpeed = 1.5f;

    [Header("シーン切り替え設定")]
    [Tooltip("このZ座標をカメラが超えたらシーンを切り替えます")]
    public float cameraTriggerZ = 5.0f;

    [Tooltip("遷移先のシーン名（必ずBuild Settingsに追加してください）")]
    public string nextSceneName;

    [Header("テキストメッシュプロ設定")]
    [Tooltip("アルファをループさせるTextMeshProオブジェクト")]
    public TextMeshProUGUI loopAlphaText; // UGUI版
    // public TextMeshPro loopAlphaText; // 3D Text版にしたい場合はこちらに変更

    [Tooltip("アルファのループ速度")]
    public float alphaLoopSpeed = 2.0f;

    [Tooltip("最小アルファ値")]
    public float minAlpha = 0.2f;

    [Tooltip("最大アルファ値")]
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
        // ▼ 左クリックで即シーン遷移
        if (!sceneSwitched && Input.GetMouseButtonDown(0))
        {
            TrySwitchScene();
        }

        // ▼ アルファ値ループ（TextMeshPro）
        if (loopAlphaText != null)
        {
            float alpha = Mathf.Lerp(minAlpha, maxAlpha, Mathf.PingPong(Time.time * alphaLoopSpeed, 1f));
            Color currentColor = loopAlphaText.color;
            currentColor.a = alpha;
            loopAlphaText.color = currentColor;
        }

        // ドアの開き処理
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

        // カメラの移動とZ軸チェック
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

    // シーン切り替え処理（共通化）
    void TrySwitchScene()
    {
        sceneSwitched = true;
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("シーン名が設定されていません！");
        }
    }
}
