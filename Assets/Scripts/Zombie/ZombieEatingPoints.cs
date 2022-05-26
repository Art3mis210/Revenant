using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieEatingPoints : MonoBehaviour
{
    public static ZombieEatingPoints Reference
    {
        get;
        set;
    }
    Transform[] ZombieEatingPointsList;
    void Start()
    {
        Reference = this;
        ZombieEatingPointsList = transform.GetComponentsInChildren<Transform>();
    }

    public Vector3 GetZombieEatingPoint(Vector3 Location)
    {
        float MaxDistance = 1000f;
        Vector3 NearestEatingPoint=ZombieEatingPointsList[1].position;
        foreach(Transform eatingPoint in ZombieEatingPointsList)
        {
            if(eatingPoint!=transform)
            {
                if(Vector3.Distance(eatingPoint.position,Location)<MaxDistance)
                {
                    MaxDistance = Vector3.Distance(eatingPoint.position, Location);
                    NearestEatingPoint = eatingPoint.position;
                }
            }
        }
        return NearestEatingPoint;
    }
}
