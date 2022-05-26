using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFleshImpactPool : MonoBehaviour
{
    public GameObject Prefab;
    public int Count;
    private GameObject[] BulletFleshImpactList;
    public static BulletFleshImpactPool Reference
    {
        get;
        set;
    }
    void Awake()
    {
        Reference = this;
        BulletFleshImpactList = new GameObject[Count];
        for (int i = 0; i < Count; i++)
        {
            BulletFleshImpactList[i] = Instantiate(Prefab, transform);
            BulletFleshImpactList[i].gameObject.SetActive(false);
        }
    }
    public GameObject GetBulletFleshImpactFromPool()
    {
        foreach (GameObject go in BulletFleshImpactList)
        {
            if (!go.activeInHierarchy)
                return go;
        }
        return null;
    }
}


