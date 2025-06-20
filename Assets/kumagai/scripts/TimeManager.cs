using UnityEngine;
using UnityEngine.SceneManagement;
public class TimeManager : MonoBehaviour
{
    [SerializeField] Sprite[] sprits;
    [SerializeField] float MaxTimer;
    SpriteRenderer sr;
    float timer;
    [SerializeField]int nowSprite ;
    const string result = "Result";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nowSprite=0;
        timer=0;
        sr = GetComponent<SpriteRenderer>();    
    }

    // Update is called once per frame
    void Update()
    {
        timer+=Time.deltaTime;
        if(timer>MaxTimer/sprits.Length*(nowSprite+1))
        {
            nowSprite++;
        }
        if(timer>MaxTimer)
        {
            SceneManager.LoadScene(result);
        }
        sr.sprite = sprits[nowSprite];
    }
}
