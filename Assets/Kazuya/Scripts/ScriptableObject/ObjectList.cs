using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "PointList", menuName = "ScriptableObject/PointList")]
public class PointList:ScriptableObject
{
     public List<EnemyData> point = new List<EnemyData>();
    
    public void Clear()
    {
       point.Clear();
    }

    public void Add(EnemyData data)
    {
        point.Add(data);  
    }
}
