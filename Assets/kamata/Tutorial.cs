using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [Header("�؂�ւ���X�v���C�g�摜�i3���j")]
    public Sprite[] tutorialSprites;

    [Header("�\����Image")]
    public Image displayImage;

    [Header("1��������̕\�����ԁi�b�j")]
    public float switchInterval = 3f;

    private int currentIndex = 0;
    private float timer = 0f;
    private bool tutorialEnded = false;

    [Header("�X�L�b�v�{�^��")]
    public Button skipButton;

    [Header("�X�L�b�v�{�^����TextMeshPro�e�L�X�g")]
    public TextMeshProUGUI loopAlphaText;

    [Header("�A���t�@���[�v�ݒ�")]
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
            Debug.LogWarning("skipButton ���ݒ肳��Ă��܂���B");
        }

        if (tutorialSprites == null || tutorialSprites.Length == 0 || displayImage == null)
        {
            Debug.LogError("�X�v���C�g�܂��͕\��Image���ݒ肳��Ă��܂���B");
            tutorialEnded = true;
            return;
        }

        currentIndex = 0;
        displayImage.sprite = tutorialSprites[currentIndex];
    }

    void Update()
    {
        if (tutorialEnded) return;

        // �A���t�@���[�v
        if (loopAlphaText != null)
        {
            float alpha = Mathf.Lerp(minAlpha, maxAlpha, Mathf.PingPong(Time.time * alphaLoopSpeed, 1f));
            Color c = loopAlphaText.color;
            c.a = alpha;
            loopAlphaText.color = c;
        }

        // �X�v���C�g�؂�ւ�����
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
                EndTutorial(); // �Ō�̃X�v���C�g��ɏI������
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
            Debug.LogWarning("SceneLoopSwitcher �����݂��܂���B");
        }
    }
}
