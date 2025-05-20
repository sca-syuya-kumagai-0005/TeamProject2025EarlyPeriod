using UnityEngine;

public class ShutterEffect : MonoBehaviour
{
    //各シャッターUI   
    public RectTransform top;
    public RectTransform bottom;
    public RectTransform left;
    public RectTransform right;

    //シャッターの初期位置（開いた状態の位置）
    private Vector2 topStart;
    private Vector2 bottomStart;
    private Vector2 leftStart;
    private Vector2 rightStart;

    //シャッターの目標位置（閉じた状態の位置）
    public Vector2 topTarget = Vector2.zero;
    public Vector2 bottomTarget = Vector2.zero;
    public Vector2 leftTarget = Vector2.zero;
    public Vector2 rightTarget = Vector2.zero;

    public float duration = 1.0f; //アニメーションの所要時間
    private bool isAnimating = false; //アニメーション中かどうか

    void Start()
    {
        //開始時に各パネルの初期位置を保存
        topStart = top.anchoredPosition;
        bottomStart = bottom.anchoredPosition;
        leftStart = left.anchoredPosition;
        rightStart = right.anchoredPosition;

        SetActiveAll(false); //初期状態ではシャッターは非表示
    }

    //エフェクトの開始
    public void TriggerEffect()
    {
        if (!isAnimating)
            StartCoroutine(MoveToTargets());
    }

    //目標位置に移動
    private System.Collections.IEnumerator MoveToTargets()
    {
        isAnimating = true;
        SetActiveAll(true);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            //開始位置から目標位置へ線形補間
            top.anchoredPosition = Vector2.Lerp(topStart, topTarget, t);
            bottom.anchoredPosition = Vector2.Lerp(bottomStart, bottomTarget, t);
            left.anchoredPosition = Vector2.Lerp(leftStart, leftTarget, t);
            right.anchoredPosition = Vector2.Lerp(rightStart, rightTarget, t);

            yield return null;
        }

        SetActiveAll(false); //非表示
        ResetPositions();    //位置をリセット
        isAnimating = false;
    }

    //全てのシャッターパネルの表示・非表示を切り替える
    private void SetActiveAll(bool active)
    {
        top.gameObject.SetActive(active);
        bottom.gameObject.SetActive(active);
        left.gameObject.SetActive(active);
        right.gameObject.SetActive(active);
    }

    //パネルの位置を開始位置に戻す
    private void ResetPositions()
    {
        top.anchoredPosition = topStart;
        bottom.anchoredPosition = bottomStart;
        left.anchoredPosition = leftStart;
        right.anchoredPosition = rightStart;
    }
}
