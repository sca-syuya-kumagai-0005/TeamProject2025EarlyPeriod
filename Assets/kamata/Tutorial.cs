using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [Header("切り替えるスプライト画像（3枚）")]
    public Sprite[] tutorialSprites;

    [Header("表示先Image")]
    public Image displayImage;

    [Header("1枚あたりの表示時間（秒）")]
    public float switchInterval = 3f;

    private int currentIndex = 0;
    private float timer = 0f;
    private bool tutorialEnded = false;

    [Header("スキップボタン")]
    public Button skipButton;

    [Header("スキップボタンのTextMeshProテキスト")]
    public TextMeshProUGUI loopAlphaText;

    [Header("アルファループ設定")]
    public float alphaLoopSpeed = 2.0f;
    public float minAlpha = 0.2f;
    public float maxAlpha = 1.0f;

    void Start()
    {
        if (skipButton != null)
        {
            skipButton.onClick.AddListener(OnSkipTutorial);
        }
        else
        {
            Debug.LogWarning("skipButton が設定されていません。");
        }

        if (tutorialSprites == null || tutorialSprites.Length == 0 || displayImage == null)
        {
            Debug.LogError("スプライトまたは表示Imageが設定されていません。");
            tutorialEnded = true;
            return;
        }

        currentIndex = 0;
        displayImage.sprite = tutorialSprites[currentIndex];
    }

    void Update()
    {
        if (tutorialEnded) return;

        // アルファループ
        if (loopAlphaText != null)
        {
            float alpha = Mathf.Lerp(minAlpha, maxAlpha, Mathf.PingPong(Time.time * alphaLoopSpeed, 1f));
            Color c = loopAlphaText.color;
            c.a = alpha;
            loopAlphaText.color = c;
        }

        // スプライト切り替え処理
        timer += Time.deltaTime;

        if (timer >= switchInterval)
        {
            timer = 0f;
            currentIndex++;

            if (currentIndex < tutorialSprites.Length)
            {
                displayImage.sprite = tutorialSprites[currentIndex];
            }
            else
            {
                EndTutorial(); // 最後のスプライト後に終了処理
            }
        }
    }

    public void OnSkipTutorial()
    {
        EndTutorial();
    }

    private void EndTutorial()
    {
        if (tutorialEnded) return;
        tutorialEnded = true;

        if (SceneLoopSwitcher.InstanceExists())
        {
            SceneLoopSwitcher.EndTutorialAndProceed();
        }
        else
        {
            Debug.LogWarning("SceneLoopSwitcher が存在しません。");
        }
    }
}
