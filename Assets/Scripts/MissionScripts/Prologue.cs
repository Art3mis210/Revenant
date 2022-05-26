using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Prologue : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject VideoCamera;
    public GameObject Phone;
    private AudioSource audioSource;
    public Canvas Controls;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Controls.enabled = false;
    }
    void Update()
    {
        if (VideoCamera.activeInHierarchy)
        {
            if (videoPlayer.frame>0 && !videoPlayer.isPlaying)
            {
                VideoCamera.SetActive(false);
                Controls.enabled = true;
                Invoke("TurnOffPhone", 3f);
                audioSource.enabled = true;
            }
        }
    }
    void TurnOffPhone()
    {
        Phone.SetActive(false);
        
        
    }
}
