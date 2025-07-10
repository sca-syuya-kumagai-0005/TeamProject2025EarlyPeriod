using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Flash : MonoBehaviour
{
    bool flashOn;
    float flashTimer;
    float imageAlpha = 1;
    float coolTime = 3f;
    float effectTime = 1f;
    [SerializeField] Image Image;
    bool getFirst;
    bool flashCoolUp;
    bool flashing;
    public bool Flashing { get { return flashing; } }
    public bool FlashCoolUp { get { return flashCoolUp; } }
    // [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] HitManager hitManager;
    private GameObject flashEffect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //spriteRenderer = Image.GetComponent<SpriteRenderer>();
        flashTimer = 120.0f;
        //coolTime = 240.0f;
        flashOn = false;
        Image.gameObject.SetActive(false);
        getFirst = true;
        flashCoolUp = true;
        flashEffect = GameObject.Find("FlashEffect");
        flashEffect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        flashing = false;
        if (hitManager.Mode == HitManager.modeChange.flashMode)
        {

            if (Input.GetMouseButton(0))
            {
                if (getFirst)
                {
                    getFirst = false;
                    flashing = true;
                    flashCoolUp = false;
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
        flashOn = true;
        yield return new WaitForSeconds(coolTime);
        Image.gameObject.SetActive(false);
        Debug.Log("クールタイム終了（フラッシュ）");
        yield  return StartCoroutine(effectCoroutine());
        flashCoolUp = true;
        flashOn = false;
        imageAlpha = 1; Image.color = new Color(1, 1, 1, imageAlpha);
        getFirst = true;

    }

    IEnumerator effectCoroutine()
    {
        flashEffect.SetActive(true);
        yield return new WaitForSeconds(effectTime);
        flashEffect.SetActive(false);
    }
}
