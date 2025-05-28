using UnityEngine;

public class MouseManager : MonoBehaviour
{
    [Header("Material と Mask の参照")]
    [SerializeField] private Material click_Mat;
    [SerializeField] private GameObject maskObj;

    [Header("Alpha 値（Material用）")]
    [SerializeField] private float min_Mat = -0.2f;
    [SerializeField] private float max_Mat = 0.5f;

    [Header("Scale 値（maskObj用）")]
    [SerializeField] private float min_Mask = 0.0f;
    [SerializeField] private float max_Mask = 2.85f;

    [Header("スクロールスピード")]
    [SerializeField] private float scrollSpeed = 5f;

    [Header("補正カーブ（スケール調整用）")]
    [SerializeField] private AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

    private float alpha;

    void Start()
    {
        alpha = click_Mat.GetFloat("_Alpha");
    }

    void Update()
    {
        ZoomProcessing();
    }
    
    void ZoomProcessing()
    {
        float scrollInput = Input.mouseScrollDelta.y;
        float scrollAmount = scrollInput * Time.deltaTime * scrollSpeed;

        // alpha 更新
        alpha = Mathf.Clamp(alpha + scrollAmount, min_Mat, max_Mat);
        click_Mat.SetFloat("_Alpha", alpha);

        // 正規化 (0〜1)
        float t = Mathf.InverseLerp(min_Mat, max_Mat, alpha);

        // カーブ補正（逆転含む）
        float curvedT = scaleCurve.Evaluate(t); // ← カーブで補正（t=0なら1, t=1なら0なら逆転）

        // スケール計算
        float targetScale = Mathf.Lerp(min_Mask, max_Mask, curvedT);

        ApplyScale(targetScale);

        Debug.Log($"Alpha: {alpha:F4}, t: {t:F3}, CurvedT: {curvedT:F3}, Scale: {targetScale:F3}");
    }

    void ApplyScale(float scale)
    {
        maskObj.transform.localScale = new Vector3(scale, scale, scale);
    }
}
