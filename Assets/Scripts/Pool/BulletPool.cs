using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public GameObject Prefab;
    public int Count;
    private Bullet[] BulletList;
    public static BulletPool Reference
    {
        get;
        set;
    }
    void Awake()
    {
        Reference = this;
        BulletList = new Bullet[Count];
        for(int i=0;i<Count;i++)
        {
            BulletList[i] = Instantiate(Prefab, transform).GetComponent<Bullet>();
            BulletList[i].gameObject.SetActive(false);
        }
    }
    public Bullet GetBulletFromPool()
    {
        foreach(Bullet b in BulletList)
        {
            if (!b.transform.gameObject.activeInHierarchy)
                return b;
        }
        return null;
    }
}
