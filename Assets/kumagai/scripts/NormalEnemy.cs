using System.Collections;
using UnityEditor.Animations;
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
        flashImage = GameObject.Find("FlashImage").gameObject;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(ExectionCoroutine ()) ;
    }

     protected override IEnumerator ExectionCoroutine()
     {
        StartCoroutine(MoveRightCoroutine(3.5f,1.5f));
        yield return new WaitForSeconds(5.0f);
        StartCoroutine(MoveLeftCoroutine(0.2f,0.0f));
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(MoveRightCoroutine(0.2f,2.0f));
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(MoveLeftCoroutine(0.1f,0.0f));


        yield return null;
     }


}