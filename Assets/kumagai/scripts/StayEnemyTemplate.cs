using UnityEngine;
using System.Collections;

public class StayEnemyTemplate : MonoBehaviour
{
    //このスクリプトは襲ってこないエネミーが共通で使う関数をまとめたスクリプトです
    protected virtual IEnumerator ExectionCoroutine()
    {
        yield return null;
    }

    protected IEnumerator MoveRightCoroutine(float totalTime, float lastPos)
    {
        while (transform.localPosition.x < lastPos)
        {
            float x = transform.position.x;
            x += Time.deltaTime / totalTime;
            Vector3 pos = transform.position;
            pos.x = x;
            transform.position = pos;
            Debug.Log(1);
            yield return null;
        }
    }


    protected IEnumerator MoveLeftCoroutine(float totalTime, float lastPos)
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


    protected IEnumerator ExitCoroutine(GameObject enemy,float time)
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
