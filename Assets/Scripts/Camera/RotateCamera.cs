using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityStandardAssets.CrossPlatformInput;

public class RotateCamera : MonoBehaviour
{
    CinemachineFreeLook PlayerCamera;
    public bool Mobile;
    void Start()
    {
        PlayerCamera = GetComponent<CinemachineFreeLook>();
        #if MOBILE_INPUT
            Mobile = true;
        #endif

    }
    void Update()
    {
        if (Mobile)
        {
            PlayerCamera.m_XAxis.m_InputAxisValue = CrossPlatformInputManager.GetAxis("AimSide");
            PlayerCamera.m_YAxis.m_InputAxisValue = CrossPlatformInputManager.GetAxis("AimUp");
        }
        else
        {
            PlayerCamera.m_XAxis.m_InputAxisValue = Input.GetAxis("Mouse X");
            PlayerCamera.m_YAxis.m_InputAxisValue = Input.GetAxis("Mouse Y");
        }
    }
}
