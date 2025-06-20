using UnityEngine;
using System.Collections;

public class StayEnemyTemplate : MonoBehaviour
{
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
        while (transform.localPosition.x < lastPos)
        {
            float x = transform.position.x;
            x += Time.deltaTime / totalTime ;
            Vector3 pos = transform.position;
            pos.x = x;
            transform.position = pos;
            yield return null;
        }
    }


}
