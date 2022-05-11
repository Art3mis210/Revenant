using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAlert : MonoBehaviour
{
    public static EnemyAlert Reference
    {
        set;
        get;

    }
    bool AlertActive;
    bool CreateNewAlert;
    public Vector3 EnemyAlertLocation;
    public float EnemyAlertRange;
    private void Start()
    {
        Reference = this;
    }
    public void AlertNearbyEnemies(Vector3 Location, float Radius)
    {
        EnemyAlertLocation = Location;
        EnemyAlertRange = Radius;
        CreateNewAlert = true;
        AlertActive = true;
        StartCoroutine(DestroyAlert());
    }
    IEnumerator DestroyAlert()
    {
        CreateNewAlert = false;
        AlertActive = true;
        yield return new WaitForSeconds(2f);
        if (CreateNewAlert == false)
        {
            AlertActive = false;
        }
    }
    public bool GetActiveAlert(float Radius, Vector3 Location)
    {
        if (AlertActive == true)
        {
            if (Vector3.Distance(EnemyAlertLocation, Location) <= Radius + EnemyAlertRange)
                return true;
        }
        return false;
    }
}
