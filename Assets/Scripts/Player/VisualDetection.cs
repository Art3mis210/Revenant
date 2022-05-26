using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualDetection : MonoBehaviour
{
    public static VisualDetection DetectPlayer
    {
        get;
        set;
    }
    public GameObject HitMeterParent;
    HitMeter[] hitMeter;
    private void Start()
    {
        DetectPlayer = this;
        hitMeter = HitMeterParent.GetComponentsInChildren<HitMeter>();
    }
    private void Update()
    {
        HitMeterParent.transform.rotation = Quaternion.Euler(HitMeterParent.transform.rotation.eulerAngles.x, HitMeterParent.transform.rotation.eulerAngles.y, PlayerController.Player.PlayerCamera.transform.rotation.eulerAngles.y-transform.rotation.eulerAngles.y);
    }
    public bool PlayerDetection(Vector3 Location)
    {
        if (Mathf.Abs(Vector3.Angle(Location-transform.position, transform.forward)) <= 45)
        {
            hitMeter[0].HitDetection();
            return hitMeter[0].DetectionFull();
        }
        else if (Mathf.Abs(Vector3.Angle(Location-transform.position, transform.forward)) <= 90 && Mathf.Abs(Vector3.Angle(Location - transform.position, transform.right)) <= 90)
        {
            hitMeter[1].HitDetection();
            return hitMeter[1].DetectionFull();
        }
        else if (Mathf.Abs(Vector3.Angle(Location - transform.position, transform.right)) <= 45)
        {
            hitMeter[2].HitDetection();
            return hitMeter[2].DetectionFull();
        }
        else if (Mathf.Abs(Vector3.Angle(Location - transform.position, -transform.forward)) <= 90 && Mathf.Abs(Vector3.Angle(Location - transform.position, transform.right)) <= 90)
        {
            hitMeter[3].HitDetection();
            return hitMeter[3].DetectionFull();
        }
        else if (Mathf.Abs(Vector3.Angle(Location - transform.position, -transform.forward)) <= 45)
        {
            hitMeter[4].HitDetection();
            return hitMeter[4].DetectionFull();
        }
        else if (Mathf.Abs(Vector3.Angle(Location - transform.position, -transform.forward)) <= 90 && Mathf.Abs(Vector3.Angle(Location - transform.position, -transform.right)) <= 90)
        {
            hitMeter[5].HitDetection();
            return hitMeter[5].DetectionFull();
        }
        else if (Mathf.Abs(Vector3.Angle(Location - transform.position, -transform.right)) <= 45)
        {
            hitMeter[6].HitDetection();
            return hitMeter[6].DetectionFull();
        }
        else if (Mathf.Abs(Vector3.Angle(Location - transform.position, transform.forward)) <= 90 && Mathf.Abs(Vector3.Angle(Location - transform.position, -transform.right)) <= 90)
        {
            hitMeter[7].HitDetection();
            return hitMeter[7].DetectionFull();
        }
        return true;
    }

}
