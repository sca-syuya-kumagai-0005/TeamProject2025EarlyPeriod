using System.Collections;
using UnityEngine;

public class NormalEnemy : StayEnemyTemplate
{
    float nextMoveTime=0.0f;
    const float nextMoveTimeMax=15.0f;
    const float nextMoveTimeMin=7.0f;
    const float half=0.5f;
    GameObject flashImage;
    enum State
    {
        HIDE = 0, //何もしない
        MOVE,//移動中
        OPEN,//顔出し中
        RUN,//フラッシュされたら
        EXIT,//写真を撮られたら
    }

    State state=State.HIDE;

    private void Awake()
    {
        GameObject obj = GameObject.Find("SpawnManager").gameObject;
        if(obj != null )  flashImage = obj.GetComponent<SpawnManager>().FlashImage; 
        

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(ExectionCoroutine ()) ;
    }

     protected override IEnumerator ExectionCoroutine()
     {
        while(true)
        {
            float randomR = Random.Range(0.5f, 10.0f);
            float randomL = randomR * -1;
            float totalTime = Random.Range(0.1f, 2.0f);

            StartCoroutine(MoveRightCoroutine(totalTime, randomR));
            yield return new WaitForSeconds(totalTime+1);
            totalTime = Random.Range(0.1f, 3f);
            StartCoroutine(MoveLeftCoroutine(totalTime, randomL));
            yield return new WaitForSeconds(totalTime+1);
        }
     }
}