using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class CameraMask : MonoBehaviour
{
    [SerializeField] GameObject mask;
    [SerializeField] float lensSpeed;
    private GameObject backGround;
    private SpriteRenderer backGroundSpriteRenderer;
    [SerializeField]private GameObject[]enemies;
    private SpriteRenderer[] enemiesSpriteRenderer;
    [SerializeField]private GameObject photo;
    [SerializeField]private GameObject album;
    [SerializeField]private GameObject lens;
    private string enemyTag="Enemy";
    private string backGroundTag="BackGround";
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        backGround=GameObject.Find(backGroundTag).gameObject;
        enemies=(GameObject.FindGameObjectsWithTag(enemyTag));
        for(int i=0; i<enemies.Length; i++)
        {
            Debug.Log(enemies[i]);
        }
       
        mask.transform.position = new Vector3(0, 0, 0);
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            backGround.SetActive(false);
            lens.SetActive(false);
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].SetActive(false);
            }
            photo.transform.position-=transform.position;
            this.gameObject.SetActive(false);
        }
        //ここまではデバッグ用コード
        if (photo.transform.childCount != 0)
        {
            return;
        }
        mask.transform.position+=Input.mousePositionDelta*Time.deltaTime*lensSpeed;
        if(Input.GetMouseButton(1))
        {
            mask.transform.position = new Vector3(0, 0, 0);
        }
        if(Input.GetMouseButton(0))
        {
            GameObject tmpThis=Instantiate(this.gameObject,transform.position,Quaternion.identity,photo.transform);
            tmpThis.GetComponent<CameraMask>().enabled = false;
            GameObject tmpBackGround=Instantiate(backGround,backGround.transform.position,Quaternion.identity,photo.transform);
            SpriteRenderer sr= tmpBackGround.GetComponent<SpriteRenderer>();
            sr.maskInteraction=SpriteMaskInteraction.VisibleInsideMask;
            for(int i=0;i<enemies.Length;i++)
            {
                GameObject photoEnemy;
                photoEnemy=Instantiate(enemies[i], enemies[i].transform.position, Quaternion.identity,photo.transform);
                sr=photoEnemy.GetComponent<SpriteRenderer>();
                sr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            }
        }

       
      
    }
}
