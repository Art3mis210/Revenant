using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTracker : MonoBehaviour
{
    public int Count;
    public static EnemyTracker Reference
    {
        get;
        set;
    }
    private void Awake()
    {
        Reference = this;
    }
}
