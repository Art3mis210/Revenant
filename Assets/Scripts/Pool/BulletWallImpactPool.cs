using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletWallImpactPool : MonoBehaviour
{
    public GameObject Prefab;
    public int Count;
    private GameObject[] BulletWallImpactList;
    public static BulletWallImpactPool Reference
    {
        get;
        set;
    }
    void Awake()
    {
        Reference = this;
        BulletWallImpactList = new GameObject[Count];
        for (int i = 0; i < Count; i++)
        {
            BulletWallImpactList[i] = Instantiate(Prefab, transform);
            BulletWallImpactList[i].gameObject.SetActive(false);
        }
    }
    public GameObject GetBulletWallImpactFromPool()
    {
        foreach (GameObject go in BulletWallImpactList)
        {
            if (!go.activeInHierarchy)
                return go;
        }
        return null;
    }
}
