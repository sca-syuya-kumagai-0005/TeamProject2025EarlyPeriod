using UnityEngine;

public class CameraMask :SoundPlayer
{
    [SerializeField] GameObject mask;
    [SerializeField] float lensSpeed;
    private GameObject backGround;
    [SerializeField]private GameObject photo;
    [SerializeField]private GameObject photoStorage;
    [SerializeField]private GameObject lens;
    [SerializeField] private GameObject enemy;
    private const string enemyTag="Enemy";
    private const string backGroundTag="BackGround";
    private const string photoStorageTag = "PhotoStorage";
    [SerializeField] private GameObject hit;
    private const float time=3.0f;
    [SerializeField] float timer;
    [SerializeField]GameObject photoSheet;
    GameObject flashImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnEnable()
    {
        flashImage = GameObject.Find("imageObject").gameObject;
        Debug.Log(flashImage.name);
        if (flashImage != null)
        {
            flashImage.SetActive(false);
        }
    }
    void Start()
    {
       
        if (GameObject.Find(photoStorageTag)!= null)
        {
            GameObject obj = GameObject.Find(photoStorageTag).gameObject;
            Destroy(obj);
        }
          photoSheet = Instantiate(photoStorage,new Vector3(0,0,0), Quaternion.identity);
          photoSheet.name= photoStorageTag;
          DontDestroyOnLoad(photoSheet);   
        backGround=GameObject.Find(backGroundTag).gameObject;
        timer = 0;
        mask.transform.position = new Vector3(0, 0, 0);
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if(flashImage.activeSelf)
        {
            return;
        }
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
            if(hitManager!=null)hitManager.enabled = false;
            HitMove hitMove=hitLens.GetComponent<HitMove>();
            if(hitMove!=null)hitMove.enabled = false;    

            GameObject tmpBackGround = Instantiate(backGround, backGround.transform.position, Quaternion.identity, p.transform);
            SpriteRenderer sr = tmpBackGround.GetComponent<SpriteRenderer>();
            if(sr!=null)sr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            GameObject enemies=Instantiate(enemy,new Vector3(0,0,0),Quaternion.identity, p.transform);
            
            for (int i = 0; i < enemies.transform.childCount; i++)
            {
                GameObject obj = enemies.transform.GetChild(i).gameObject;
                sr = obj.GetComponent<SpriteRenderer>();
                sr.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                EnemySpriteAnimator enemyAnim = obj.GetComponent<EnemySpriteAnimator>();
                if(enemyAnim!=null)enemyAnim.enabled = false;
                HitCheakBibiri hitBibiri = obj.GetComponent<HitCheakBibiri>();
                if(hitBibiri!=null)hitBibiri.enabled = false;
                HitCheakOdokasi hitOdokasi = obj.GetComponent<HitCheakOdokasi>();
                if( hitOdokasi!=null)hitOdokasi.enabled = false;
                NormalEnemy nomEnemy = obj.GetComponent<NormalEnemy>(); 
                if(nomEnemy!=null) nomEnemy.enabled = false;    
               
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