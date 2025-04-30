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
    [SerializeField]private GameObject photoStorage;
    [SerializeField]private GameObject lens;
    private string enemyTag="Enemy";
    private string backGroundTag="BackGround";
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        backGround=GameObject.Find(backGroundTag).gameObject;
        enemies=(GameObject.FindGameObjectsWithTag(enemyTag));
        mask.transform.position = new Vector3(0, 0, 0);
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(photo.transform.childCount!=0)
        {
            return;
        }
        PointerMove();
        MakePhoto();
    }

    /// <summary>
    /// カーソルの動き　デバッグ用なので仮実装
    /// </summary>
    void PointerMove()
    {
        mask.transform.position += Input.mousePositionDelta * Time.deltaTime * lensSpeed;
        if (Input.GetMouseButton(1))
        {
            mask.transform.position = new Vector3(0, 0, 0);
        }
    }

    /// <summary>
    /// 写真の切り抜き
    /// </summary>
    void MakePhoto()
    {
        if (Input.GetMouseButton(0))
        {
            GameObject tmpThis = Instantiate(this.gameObject, transform.position, Quaternion.identity, photo.transform);
            tmpThis.GetComponent<CameraMask>().enabled = false;
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

    /// <summary>
    /// 作成した写真オブジェクトをPhotoStorageにコピー
    /// </summary>
    void SavePhoto()
    {
        GameObject obj=Instantiate(photo,new Vector3(0,0,0), Quaternion.identity, photoStorage.transform);
        obj.name="Stage1";
        photo.SetActive(false);
    }

    //デバッグ用　背景とかもろもろのActiveをfalseにして見やすくするための関数　
    void DebugFunction()
    {
        backGround.SetActive(false);
        lens.SetActive(false);
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].SetActive(false);
        }
        photo.transform.position -= transform.position;
        this.gameObject.SetActive(false);
    }
}