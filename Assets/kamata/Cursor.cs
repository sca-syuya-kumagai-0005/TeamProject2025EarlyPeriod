using UnityEngine;

public class Cursor : MonoBehaviour
{
    [Header("起動時にカーソルを非表示にする")]
    public bool hideOnStart = true;

    void Start()
    {
        if (hideOnStart)
        {
            UnityEngine.Cursor.visible = false; // 見た目だけ非表示
        }
    }

    void OnDestroy()
    {
        // シーンが切り替わってこのオブジェクトが破棄されたら、カーソル表示を復元
        UnityEngine.Cursor.visible = true;
    }
}
