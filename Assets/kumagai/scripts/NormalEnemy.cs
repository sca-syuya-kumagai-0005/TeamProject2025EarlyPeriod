using System.Collections;
using UnityEngine;

public class NormalEnemy : MonoBehaviour
{
    float nextMoveTime=0.0f;
    const float nextMoveTimeMax=5f;
    const float nextMoveTimeMin=3.0f;
    
    const float half=0.5f;
    enum State
    {
        HIDE = 0, //何もしない
        MOVE,//顔出し
        RUN,//フラッシュされたら
        EXIT,//写真を撮られたら
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(MoveCoroutine());  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator MoveCoroutine()
    {
        float r = Random.Range(nextMoveTimeMin, nextMoveTimeMax);
        //yield return new WaitForSeconds(r);
        
        StartCoroutine(MoveRightCoroutine());
        yield return null;
    }


    IEnumerator MoveRightCoroutine()
    {
        float totalTime = 5.0f;
        while (transform.localPosition.x < 0.5f)
        {
            float x = transform.position.x;
            x += Time.deltaTime / totalTime / half;
            Vector3 pos = transform.position;
            pos.x = x;
            transform.position = pos;
            Debug.Log(1);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        totalTime=0.5f;
        while(transform.localPosition.x>0)
        {
            float x = transform.position.x;
            x -= Time.deltaTime / totalTime / half;
            Vector3 pos = transform.position;
            pos.x = x;
            transform.position = pos;
            Debug.Log(1);
            yield return null;
        }
        totalTime=0.5f;
        while (transform.localPosition.x < 1f)
        {
            float x = transform.position.x;
            x += Time.deltaTime / totalTime / half;
            Vector3 pos = transform.position;
            pos.x = x;
            transform.position = pos;
            Debug.Log(1);
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        totalTime=0.1f;
        while (transform.localPosition.x > 0)
        {
            float x = transform.position.x;
            x -= Time.deltaTime / totalTime / half;
            Vector3 pos = transform.position;
            pos.x = x;
            transform.position = pos;
            Debug.Log(1);
            yield return null;
        }
    }
}
