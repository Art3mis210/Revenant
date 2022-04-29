using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseManager : MonoBehaviour
{
    public static NoiseManager Noise
    {
        get;
        set;
    }
    private void Start()
    {
        Noise = this;
    }
    public Vector3 NoiseLocation;
    bool NoiseActive;
    bool AddNewNoise;
    public void CreateNoise(Vector3 Location)
    {
        AddNewNoise = true;
        NoiseLocation = Location;
        StartCoroutine(DestroyNoise());
    }
    IEnumerator DestroyNoise()
    {
        NoiseActive = true;
        AddNewNoise = false;
        yield return new WaitForSeconds(2f);
        if(AddNewNoise==false)
        {
            NoiseActive = false;
        }
    }
    public bool GetActiveNoise(float Radius,Vector3 Location)
    {
        if (NoiseActive == true)
        {
            if (Vector3.Distance(NoiseLocation, Location) <= Radius)
                return true;
        }
        return false;
    }
}
