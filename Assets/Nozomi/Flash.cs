using UnityEngine;

public class Flash : MonoBehaviour
{
    bool modeChange;
    bool flashOn;
    float flashTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        modeChange = false;
        flashTimer = 0.0f;
        flashOn = false;
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            modeChange = false;
        }
        if (Input.GetKey(KeyCode.D))
        {
            modeChange = true;
        }
        if (modeChange)
        {
            //フラッシュのテスト（失敗）　デバッグログでチェック
            /*if (Input.GetMouseButton(0))
            {
                this.gameObject.SetActive(true);
                flashOn = true;
            }*/
        }
        if (flashOn)
        {
            flashTimer++;
            if (flashTimer >= Time.deltaTime)
            {
                this .gameObject.SetActive(false);
                flashOn=false;
                flashTimer=0;
            }
        }
    }
}
