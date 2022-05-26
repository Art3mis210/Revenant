using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class WeaponIK : MonoBehaviour
{
    public float Speed;
    float YPos;
    delegate void MoveIK();
    MoveIK MoveIKPoint;
    private void Start()
    {
        if (PlayerController.Player.CurrentInput == PlayerController.InputType.Mobile)
        {
            MoveIKPoint = MoveIKPointGamepad;
        }
        else
        {
            MoveIKPoint = MoveIKPointKeyboard;
            Speed = 100;
        }
    }
    void Update()
    {
        MoveIKPoint();
    }
    void MoveIKPointGamepad()
    {
        YPos = transform.localPosition.y + (CrossPlatformInputManager.GetAxis("AimUp") / 8) * Speed * Time.deltaTime;
        YPos = Mathf.Clamp(YPos, -0.23f, 5f);
        transform.localPosition = new Vector3(transform.localPosition.x, YPos, transform.localPosition.z);
    }
    void MoveIKPointKeyboard()
    {

        YPos = transform.localPosition.y + (CrossPlatformInputManager.GetAxis("Mouse Y") / 8) * Speed * Time.deltaTime;
        YPos = Mathf.Clamp(YPos, -0.23f, 5f);
        transform.localPosition = new Vector3(transform.localPosition.x, YPos, transform.localPosition.z);
    }

}
