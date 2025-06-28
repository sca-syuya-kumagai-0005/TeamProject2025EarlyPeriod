using UnityEngine;
using System.Collections;

public class StayEnemyTemplate : MonoBehaviour
{
    //このスクリプトは襲ってこないエネミーが共通で使う関数をまとめたスクリプトです


    protected IEnumerator MoveRightCoroutine(float totalTime, float lastPos)//エネミーをtotalTime秒でlastPos(X座標)まで移動する
    {
        while (transform.localPosition.x < lastPos)
        {
            float x = transform.position.x;
            x += Time.deltaTime / totalTime;
            Vector3 pos = transform.position;
            pos.x = x;
            transform.position = pos;
            yield return null;
        }
    }


    protected IEnumerator MoveLeftCoroutine(float totalTime, float lastPos)//左へ移動
    {
        while (transform.localPosition.x > lastPos)
        {
            float x = transform.position.x;
            x -= Time.deltaTime / totalTime ;
            Vector3 pos = transform.position;
            pos.x = x;
            transform.position = pos;
            yield return null;
        }
    }

    protected IEnumerator MoveCoroutine(float totalTime, Vector3 lastPos)
    {
        yield return null;
        Vector3 dir = (lastPos - transform.position).normalized;
        float dist = (lastPos - transform.position).magnitude;
        Debug.Log("dir"+dir);
        float timer = 0.0f;
        while (timer < totalTime)
        {
            timer += Time.deltaTime;
            Vector3 pos = transform.position;
            pos += dir * (dist/totalTime)*Time.deltaTime ;
            transform.position = pos;
            yield return null;
        }

    }


    protected IEnumerator ExitCoroutine(GameObject enemy,float time)//フラッシュされたら逃げる
    {
        SpriteRenderer sr = enemy.GetComponent<SpriteRenderer>();
        float alpha = 1;
        while(alpha>0)
        {
            alpha -= Time.deltaTime/time;
            Color c = sr.color;
            sr.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }
        Destroy(enemy);
        yield return null;
    }



}
