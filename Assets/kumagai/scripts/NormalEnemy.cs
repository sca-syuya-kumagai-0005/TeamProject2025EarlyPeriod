using System.Collections;
using UnityEngine;

public class NormalEnemy : StayEnemyTemplate
{
    GameObject flashImage;
    IEnumerator coroutine;
    IEnumerator moveRightCoroutine;
    IEnumerator moveLeftCoroutine;
    Flash flash;
    private void Awake()
    {
        GameObject obj = GameObject.Find("SpawnManager").gameObject;
        if (obj != null)
        {
            flashImage = obj.GetComponent<SpawnManager>().FlashImage;
            flash = obj.GetComponent<SpawnManager>().Flash.GetComponent<Flash>();
        }
        

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coroutine=ExectionCoroutine();
        StartCoroutine(coroutine);
        
    }

    private void Update()
    {
       
        if(flash.Flashing)
        {
            Debug.Log("コルーチンを停止します");
            if(coroutine!=null) StopCoroutine(coroutine) ;
            if(moveRightCoroutine!=null)StopCoroutine(moveRightCoroutine);
            if(moveLeftCoroutine!=null)StopCoroutine(moveLeftCoroutine);
            StartCoroutine(ExitCoroutine(this.gameObject, 1f));
        }
    }
    protected override IEnumerator ExectionCoroutine()
     {
        while(true)
        {
            Debug.Log("コルーチンは動いています");
            float randomR = Random.Range(0.5f, 10.0f);
            float randomL = randomR * -1;
            float totalTime = Random.Range(0.1f, 2.0f);
            moveRightCoroutine = MoveRightCoroutine(totalTime, randomR);
            StartCoroutine(moveRightCoroutine);
            yield return new WaitForSeconds(totalTime+1);
            totalTime = Random.Range(0.1f, 3f);
            moveLeftCoroutine = MoveLeftCoroutine(totalTime, randomL);
            StartCoroutine(moveLeftCoroutine);
            yield return new WaitForSeconds(totalTime+1);
        }
     }
}