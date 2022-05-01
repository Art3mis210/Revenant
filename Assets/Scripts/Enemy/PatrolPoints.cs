using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPoints : MonoBehaviour
{
    public Transform[] PatrolPointList;
    void Start()
    {
        PatrolPointList = new Transform[transform.childCount];
        PatrolPointList = transform.GetComponentsInChildren<Transform>();
    }
    public Transform GetNextPatrolPoint(ref int CurrentWaypoint,bool Dir)
    {
        if(Dir)
            CurrentWaypoint += 1;
        else
        {
            CurrentWaypoint -= 1;
            if (CurrentWaypoint < 0)
                CurrentWaypoint = transform.childCount - 1;
        }
        CurrentWaypoint = CurrentWaypoint % (transform.childCount);
        return PatrolPointList[CurrentWaypoint];
    }

}
