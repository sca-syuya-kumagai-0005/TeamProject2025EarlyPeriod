using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEditor.Rendering;
using UnityEngine.SceneManagement;

public class NormalEnemy : StayEnemyTemplate
{
    GameObject flashImage;
    
    IEnumerator moveRightCoroutine;
    IEnumerator moveLeftCoroutine;
    IEnumerator coroutine;
    List<IEnumerator> motions=new List<IEnumerator>();
    GameObject[] hideObjects;
    const string hideObjectTag = "HideObject";
    Flash flash;
    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "Result") return;
        hideObjects = GameObject.FindGameObjectsWithTag(hideObjectTag);
        motions.Clear();
        motions.Add(RoundTripCoroutine());
        motions.Add(HIdeCoroutine());   
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
        
        coroutine = motions[Random.Range(0,motions.Count)];
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

    private IEnumerator RoundTripCoroutine()//往復
    {
        while (true)
        {
            Debug.Log("コルーチンは動いています");
            float randomR = Random.Range(0.5f, 10.0f);
            float randomL = randomR * -1;
            float totalTime = Random.Range(0.1f, 2.0f);
            moveRightCoroutine = MoveRightCoroutine(totalTime, randomR);
            StartCoroutine(moveRightCoroutine);
            yield return new WaitForSeconds(totalTime + 1);
            totalTime = Random.Range(0.1f, 3f);
            moveLeftCoroutine = MoveLeftCoroutine(totalTime, randomL);
            StartCoroutine(moveLeftCoroutine);
            yield return new WaitForSeconds(totalTime + 1);
        }
    }

    private IEnumerator HIdeCoroutine()
    {
        int lastObject=-1;//現在隠れているオブジェクトの番号
        while(true)
        {
            while (true)
            {
                int r = Random.Range(0,hideObjects.Length);
                if(r!=lastObject)//rがlastと異なれば抜ける
                {
                    lastObject = r;
                    break;
                }
            }
            float time = Random.Range(0.5f, 2.0f);
            if(lastObject>0)
            {
                StartCoroutine(MoveCoroutine(time, hideObjects[lastObject].transform.position));
            }
         
            float waitTime = Random.Range(1.0f, 3.0f);
            yield return new WaitForSeconds(time+waitTime);

        }
    }

 

   

}