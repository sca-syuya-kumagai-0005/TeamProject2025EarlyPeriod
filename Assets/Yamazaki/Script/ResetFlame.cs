using UnityEngine;

public class ResetFlame : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Material _mat;

    void OnEnable()
    {
        _mat = GetComponent<Renderer>().material;
        _mat.SetFloat("_TotalTime", Time.time); // 現在時刻を送って差分を0に
    }
}
