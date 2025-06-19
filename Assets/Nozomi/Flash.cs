using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Flash : MonoBehaviour
{
    bool flashOn;
    float flashTimer;
    float imageAlpha = 1;
    float coolTime = 4f;
    [SerializeField] Image Image;
    bool getFirst;
    bool flashCool;
    // [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] HitManager hitManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //spriteRenderer = Image.GetComponent<SpriteRenderer>();
        flashTimer = 120.0f;
        coolTime = 240.0f;
        flashOn = false;
        Image.gameObject.SetActive(false);
        getFirst = true;
        flashCool = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (hitManager.Mode == HitManager.modeChange.flashMode)
        {

            if (Input.GetMouseButton(0))
            {
                if (getFirst)
                {
                    getFirst = false;
                    Image.gameObject.SetActive(true);
                    StartCoroutine(TestCoroutine());
                }

            }

        }
        if (flashOn)
        {
            imageAlpha -= Time.deltaTime;
            Image.color = new Color(1, 1, 1, imageAlpha);
        }
    }
    IEnumerator TestCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        flashOn = true;
        yield return new WaitForSeconds(3.0f);
        Image.gameObject.SetActive(false);
        Debug.Log("クールタイム終了（フラッシュ）");
        flashOn = false;
        imageAlpha = 1; Image.color = new Color(1, 1, 1, imageAlpha);
        getFirst = true;

    }
}
