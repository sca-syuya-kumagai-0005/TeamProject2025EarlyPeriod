using UnityEngine;

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
    [SerializeField] private GameObject enemy;
    private string enemyTag="Enemy";
    private string backGroundTag="BackGround";
    [SerializeField] private GameObject hit;
    private const float time=3.0f;
    [SerializeField] float timer;
    [SerializeField]GameObject photoSheet;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(GameObject.Find("PhotoStorage")!=null)
        {
            Destroy(photoSheet);
        }
          photoSheet = Instantiate(photoStorage,new Vector3(0,0,0), Quaternion.identity);
          photoSheet.name= "PhotoStorage";
          DontDestroyOnLoad(photoSheet);   
        backGround=GameObject.Find(backGroundTag).gameObject;
        timer = 0;
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
        timer -= Time.deltaTime;
        
        PointerMove();
        MakePhoto();
    }


    void PointerMove()
    {
       transform.position=hit.transform.position;
    }


    public void MakePhoto()
    {
        if(timer>0)
        {
            return;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            GameObject p = Instantiate(photo,new Vector3(0,0,0),Quaternion.identity,photoSheet.transform);
            p.name = "photo";
            p.SetActive(false);
            GameObject tmpThis = Instantiate(this.gameObject, transform.position, Quaternion.identity, p.transform);
            tmpThis.GetComponent<CameraMask>().enabled = false;
            GameObject hitLens = Instantiate(hit.gameObject,hit.transform.position, Quaternion.identity, p.transform);
            HitManager hitManager = hitLens.GetComponent<HitManager>();
            hitManager.enabled = false;
            HitMove hitMove=hitLens.GetComponent<HitMove>();
            hitMove.enabled = false;    
            GameObject tmpBackGround = Instantiate(backGround, backGround.transform.position, Quaternion.identity, p.transform);
            SpriteRenderer sr = tmpBackGround.GetComponent<SpriteRenderer>();
            sr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            GameObject enemies=Instantiate(enemy,new Vector3(0,0,0),Quaternion.identity, p.transform);
            for (int i = 0; i < enemies.transform.childCount; i++)
            {
                sr = enemies.transform.GetChild(i).GetComponent<SpriteRenderer>();
                sr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            }
            SavePhoto();
            DebugFunction();
            timer = time;
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