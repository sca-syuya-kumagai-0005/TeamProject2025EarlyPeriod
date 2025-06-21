
using UnityEngine;

public class ObjectSpawn : MonoBehaviour
{
    Vector2 position;
    [SerializeField]GameObject obj;
    public int enemyMaxCount = 5;
    int objectCount= 0;

public int ObjectCount
    {
        get { return objectCount; }
        set { objectCount = value; }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i <= enemyMaxCount; i++)
        {
            Instantiate(obj, position, Quaternion.identity);
            objectCount++;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (objectCount <= enemyMaxCount)
        {
            Debug.Log("Ä¶¬");
            objectCount++ ;
            Instantiate(obj, position, Quaternion.identity);
        }
    }
}
