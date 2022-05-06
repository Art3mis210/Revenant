using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraLocker : MonoBehaviour
{
    public CinemachineFreeLook PlayerCamera;
    public bool CameraLock;
    public static CameraLocker instance
    {
        get;
        set;
    }
    private void Start()
    {
        LockCamera();
        CameraLock = true;
        instance = this;
    }
    public void LockCamera()
    {
        CameraLock = true;
        PlayerCamera.m_YAxis.m_MaxSpeed = 0;
        PlayerCamera.m_XAxis.m_MaxSpeed = 0;
    }
    public void UnLockCamera()
    {
        CameraLock = false;
        PlayerCamera.m_YAxis.m_MaxSpeed = 2;
        PlayerCamera.m_XAxis.m_MaxSpeed = 300;
    }
}
