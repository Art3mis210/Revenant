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
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (VideoCamera.activeInHierarchy)
        {
            if (videoPlayer.frame>0 && !videoPlayer.isPlaying)
            {
                VideoCamera.SetActive(false);
                Invoke("TurnOffPhone", 10f);
                audioSource.enabled = true;
            }
        }
    }
    void TurnOffPhone()
    {
        Phone.SetActive(false);
        
    }
}
