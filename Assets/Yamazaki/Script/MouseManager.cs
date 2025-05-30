using UnityEngine;

public class MouseManager : MonoBehaviour
{
    [Header("マウスクリック時にフラッシュ表示する SpriteRenderer")]
    [SerializeField] private SpriteRenderer cameraFlash;

    [Header("マテリアルとマスク")]
    [SerializeField] private Material clickMat;
    [SerializeField] private GameObject maskObj;

    [Header("Material の Alpha 範囲")]
    [SerializeField] private float minMat = -0.2f;
    [SerializeField] private float maxMat = 0.5f;

    [Header("マスクの Scale 範囲")]
    [SerializeField] private float minMask = 0f;
    [SerializeField] private float maxMask = 2.85f;

    [Header("マウスホイール感度")]
    [SerializeField] private float scrollSpeed = 5f;

    [Header("スケール補正カーブ")]
    [SerializeField] private AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

    [Header("カメラと出力先")]
    [SerializeField] private Camera targetCamera;
    [SerializeField] private RenderTexture renderTexture;
    [SerializeField] private SpriteRenderer targetSpriteRenderer;

    [Header("撮影写真の親オブジェクト")]
    [SerializeField] private GameObject snapshot_Obj;

    [Header("タイトルの親オブジェクト")]
    [SerializeField] private GameObject titleObj;

    private float alpha;
    private bool canMouseClick = true;
    Animation animationScript;

    void Start()
    {
        alpha = clickMat.GetFloat("_Alpha");
        cameraFlash.enabled = false;
        snapshot_Obj.SetActive(false);
        animationScript = GetComponent<Animation>();
    }

    void Update()
    {
        HandleMouseClick();
        HandleZoom();
    }

    void HandleMouseClick()
    {
        if (Input.GetMouseButtonDown(0) && canMouseClick)
        {
            TakeSnapshot();
            canMouseClick = false;
            cameraFlash.enabled = true;
            cameraFlash.color = Color.white;

            
        }

        // フラッシュ演出
        if (cameraFlash.enabled)
        {
            cameraFlash.color = Color.Lerp(cameraFlash.color, Color.clear, Time.deltaTime * 5f);

            // αが十分小さくなったらフラッシュ終了
            if (cameraFlash.color.a <= 0.01f)
            {
                cameraFlash.enabled = false;
                //canMouseClick = true;
                snapshot_Obj.SetActive(true);
                Debug.Log("ここからアニメーションの入れる感じ");
                animationScript.GetSetProperty = 2;
                titleObj.SetActive(false);
            }
        }
    }

    void HandleZoom()
    {
        float scroll = Input.mouseScrollDelta.y * Time.deltaTime * scrollSpeed;
        alpha = Mathf.Clamp(alpha + scroll, minMat, maxMat);
        clickMat.SetFloat("_Alpha", alpha);

        float t = Mathf.InverseLerp(minMat, maxMat, alpha);
        float curvedT = scaleCurve.Evaluate(t);
        float scale = Mathf.Lerp(minMask, maxMask, curvedT);

        maskObj.transform.localScale = Vector3.one * scale;
    }

    void TakeSnapshot()
    {
        if (targetCamera == null || renderTexture == null || targetSpriteRenderer == null)
        {
            Debug.LogError("設定が不完全です");
            return;
        }

        Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);

        var prevRT = RenderTexture.active;
        RenderTexture.active = renderTexture;

        targetCamera.targetTexture = renderTexture;
        targetCamera.Render();

        tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tex.Apply();

        targetCamera.targetTexture = null;
        RenderTexture.active = prevRT;

        Sprite snapshot = Sprite.Create(
            tex,
            new Rect(0, 0, tex.width, tex.height),
            new Vector2(0.5f, 0.5f),
            100f, // pixelsPerUnit
            0,
            SpriteMeshType.FullRect
        );

        targetSpriteRenderer.sprite = snapshot;
        targetSpriteRenderer.enabled = true;
        targetSpriteRenderer.color = Color.white;
        //Textureの解像度と見合う値に設定する
       

        Debug.Log("Snapshot set to SpriteRenderer: " + snapshot.name);
    }


}
