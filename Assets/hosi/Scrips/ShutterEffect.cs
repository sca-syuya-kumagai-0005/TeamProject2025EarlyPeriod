using UnityEngine;

public class ShutterEffect : MonoBehaviour
{
    //�e�V���b�^�[UI   
    public RectTransform top;
    public RectTransform bottom;
    public RectTransform left;
    public RectTransform right;

    //�V���b�^�[�̏����ʒu�i�J������Ԃ̈ʒu�j
    private Vector2 topStart;
    private Vector2 bottomStart;
    private Vector2 leftStart;
    private Vector2 rightStart;

    //�V���b�^�[�̖ڕW�ʒu�i������Ԃ̈ʒu�j
    public Vector2 topTarget = Vector2.zero;
    public Vector2 bottomTarget = Vector2.zero;
    public Vector2 leftTarget = Vector2.zero;
    public Vector2 rightTarget = Vector2.zero;

    public float duration = 1.0f; //�A�j���[�V�����̏��v����
    private bool isAnimating = false; //�A�j���[�V���������ǂ���

    void Start()
    {
        //�J�n���Ɋe�p�l���̏����ʒu��ۑ�
        topStart = top.anchoredPosition;
        bottomStart = bottom.anchoredPosition;
        leftStart = left.anchoredPosition;
        rightStart = right.anchoredPosition;

        SetActiveAll(false); //������Ԃł̓V���b�^�[�͔�\��
    }

    //�G�t�F�N�g�̊J�n
    public void TriggerEffect()
    {
        if (!isAnimating)
            StartCoroutine(MoveToTargets());
    }

    //�ڕW�ʒu�Ɉړ�
    private System.Collections.IEnumerator MoveToTargets()
    {
        isAnimating = true;
        SetActiveAll(true);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            //�J�n�ʒu����ڕW�ʒu�֐��`���
            top.anchoredPosition = Vector2.Lerp(topStart, topTarget, t);
            bottom.anchoredPosition = Vector2.Lerp(bottomStart, bottomTarget, t);
            left.anchoredPosition = Vector2.Lerp(leftStart, leftTarget, t);
            right.anchoredPosition = Vector2.Lerp(rightStart, rightTarget, t);

            yield return null;
        }

        SetActiveAll(false); //��\��
        ResetPositions();    //�ʒu�����Z�b�g
        isAnimating = false;
    }

    //�S�ẴV���b�^�[�p�l���̕\���E��\����؂�ւ���
    private void SetActiveAll(bool active)
    {
        top.gameObject.SetActive(active);
        bottom.gameObject.SetActive(active);
        left.gameObject.SetActive(active);
        right.gameObject.SetActive(active);
    }

    //�p�l���̈ʒu���J�n�ʒu�ɖ߂�
    private void ResetPositions()
    {
        top.anchoredPosition = topStart;
        bottom.anchoredPosition = bottomStart;
        left.anchoredPosition = leftStart;
        right.anchoredPosition = rightStart;
    }
}
