using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class WeaponIK : MonoBehaviour
{
    public float Speed;
    float YPos;
    void Update()
    {
     //   if (!CameraLocker.instance.CameraLock)
        {
            YPos = transform.localPosition.y + (CrossPlatformInputManager.GetAxis("AimUp") / 8)*Speed*Time.deltaTime;
            YPos = Mathf.Clamp(YPos, -0.23f, 5f);
            transform.localPosition = new Vector3(transform.localPosition.x, YPos, transform.localPosition.z);
        }
    }
}
