using UnityEngine;
using UnityEngine.SceneManagement;
public class TimeManager : MonoBehaviour
{
    [SerializeField] Sprite[] sprits;
    [SerializeField] float waitTimer;
    public float WaitTimer {  get { return waitTimer; }  }
    SpriteRenderer sr;
    float timer;
    public float Timer { get { return timer; } }
    int nowSprite ;
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
        if(timer>waitTimer/sprits.Length*(nowSprite+1))
        {
            nowSprite++;
        }
        if(sprits.Length>nowSprite)
        { 
            sr.sprite = sprits[nowSprite];
        }
    }
}
