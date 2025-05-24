using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class CameraMask :SoundPlayer
{
    [SerializeField] GameObject mask;
    [SerializeField] float lensSpeed;
    private GameObject backGround;
    private SpriteRenderer backGroundSpriteRenderer;
    private SpriteRenderer[] enemiesSpriteRenderer;
    [SerializeField]private GameObject photo;
    [SerializeField]private GameObject photoStorage;
    [SerializeField]private GameObject lens;
    private string enemyTag="Enemy";
    private string backGroundTag="BackGround";
    [SerializeField] private GameObject hit;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        backGround=GameObject.Find(backGroundTag).gameObject;
      
        mask.transform.position = new Vector3(0, 0, 0);
        //Cursor.visible = false;
     

    }

    // Update is called once per frame
    void Update()
    {
        //if(photo.transform.childCount!=0)
        //{
        //    return;
        //}
        PointerMove();
        MakePhoto();
    }


    void PointerMove()
    {
       transform.position=hit.transform.position;
    }


    void MakePhoto()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            GameObject[] enemies;
            photo = Instantiate(photo, photoStorage.transform);
            photo.name = "photo";
            photo.SetActive(false);
            enemies = (GameObject.FindGameObjectsWithTag(enemyTag));
            GameObject tmpThis = Instantiate(this.gameObject, transform.position, Quaternion.identity, photo.transform);
            //tmpThis.GetComponent<CameraMask>().enabled = false;
            GameObject tmpBackGround = Instantiate(backGround, backGround.transform.position, Quaternion.identity, photo.transform);
            SpriteRenderer sr = tmpBackGround.GetComponent<SpriteRenderer>();
            sr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            for (int i = 0; i < enemies.Length; i++)
            {
                GameObject photoEnemy;
                photoEnemy = Instantiate(enemies[i], enemies[i].transform.position, Quaternion.identity, photo.transform);
                sr = photoEnemy.GetComponent<SpriteRenderer>();
                sr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            }
            SavePhoto();
            DebugFunction();
        }
      
    }

    void SavePhoto()
    {
        
        //GameObject obj=Instantiate(photo,new Vector3(0,0,0), Quaternion.identity, photoStorage.transform);
       
    }

   
    void DebugFunction()
    {
       // backGround.SetActive(false);
        //lens.SetActive(false);
        //for (int i = 0; i < enemies.Length; i++)
        //{
        //    enemies[i].SetActive(false);
        //}
        photo.transform.position -= transform.position;
       // this.gameObject.SetActive(false);
    }
}