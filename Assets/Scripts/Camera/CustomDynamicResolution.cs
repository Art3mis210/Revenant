using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomDynamicResolution : MonoBehaviour
{
    public float DesiredFPS;
    public float MaxScaleFactor = 1f;
    public float MinScaleFactor = 0.1f;
    [Range(0.1f,1f)]
    public float CurrentScale=1f;
    public FPS CurrentFPS;
    // Update is called once per frame
    void Update()
    {
        if(Mathf.Round(CurrentFPS.count)<DesiredFPS)
        {
            if (CurrentScale > MinScaleFactor)
            {
                ScalableBufferManager.ResizeBuffers(CurrentScale - 0.1f, CurrentScale - 0.1f);
            }
        }
        else
        {
            if (CurrentScale < MaxScaleFactor)
            {
                ScalableBufferManager.ResizeBuffers(CurrentScale + 0.1f, CurrentScale + 0.1f);
            }
        }
    }
}
