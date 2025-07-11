using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
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

    [Header("スキップボタン")]
    public Button skipButton;

    [Header("サウンド設定")]
    public AudioSource audioSource;
    public AudioClip doorOpenSound;
    public AudioClip doorCloseSound;
    public float doorCloseDelay = 2.5f;

    private Quaternion leftClosedRot;
    private Quaternion rightClosedRot;
    private Quaternion leftOpenRot;
    private Quaternion rightOpenRot;

    private bool doorIsOpening = false;
    private bool cameraIsMoving = false;
    private bool requestedSceneChange = false;

    private float timer = 0f;
    private float cameraMoveTimer = 0f;

    private bool playedOpenSound = false;
    private bool playedCloseSound = false;
    private float closeSoundTimer = 0f;

    void Start()
    {
        leftClosedRot = leftDoor.localRotation;
        rightClosedRot = rightDoor.localRotation;

        leftOpenRot = leftClosedRot * Quaternion.Euler(0, 0, -openAngle);
        rightOpenRot = rightClosedRot * Quaternion.Euler(0, 0, openAngle);

        if (skipButton != null)
        {
            skipButton.onClick.AddListener(SkipByButton);
        }
    }

    void Update()
    {
        // アルファループ
        if (loopAlphaText != null)
        {
            float alpha = Mathf.Lerp(minAlpha, maxAlpha, Mathf.PingPong(Time.time * alphaLoopSpeed, 1f));
            Color color = loopAlphaText.color;
            color.a = alpha;
            loopAlphaText.color = color;
        }

        // ドア開き処理
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

            // 開いた直後に音を再生
            if (!playedOpenSound && audioSource != null && doorOpenSound != null)
            {
                audioSource.PlayOneShot(doorOpenSound);
                playedOpenSound = true;
            }

            // 数秒後に閉まる音を再生
            if (playedOpenSound && !playedCloseSound)
            {
                closeSoundTimer += Time.deltaTime;
                if (closeSoundTimer >= doorCloseDelay)
                {
                    if (audioSource != null && doorCloseSound != null)
                    {
                        audioSource.PlayOneShot(doorCloseSound);
                    }
                    playedCloseSound = true;
                }
            }

            cameraMoveTimer += Time.deltaTime;
            if (!cameraIsMoving && cameraMoveTimer >= cameraMoveDelay)
            {
                cameraIsMoving = true;
            }
        }

        // カメラ移動
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

            // 最終的に次のシーンへ
            if (!requestedSceneChange &&
                Vector3.Distance(targetCamera.transform.position, cameraTargetPosition.position) < 0.1f)
            {
                RequestSceneChangeOnce();
            }
        }
    }

    public void SkipByButton()
    {
        RequestSceneChangeOnce();
    }

    private void RequestSceneChangeOnce()
    {
        if (requestedSceneChange) return;

        requestedSceneChange = true;

        var switcher = Object.FindFirstObjectByType<SceneLoopSwitcher>();
        if (switcher != null)
        {
            SceneLoopSwitcher.RequestSceneChange();
        }
        else
        {
            Debug.LogWarning("SceneLoopSwitcher が存在しません。");
        }
    }
}
