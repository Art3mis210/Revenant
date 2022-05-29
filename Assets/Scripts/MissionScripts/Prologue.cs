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
    [SerializeField] AudioClip[] Dialogue;
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
                MissionTrigger.Reference.ChangePos(new Vector3(-98.3899994f, 1.34461617f, -112.940002f),"Return To Home");
                audioSource.PlayOneShot(Dialogue[0]);
            }
        }
        else
        {
            if(MissionTrigger.Reference.InTrigger)
            {
                PlayerController.Player.EnableMovement = false;
                Debug.Log("Mission Complete");
                MissionTrigger.Reference.Fade();
                audioSource.PlayOneShot(Dialogue[1]);
                PlayerPrefs.SetInt("Mission", 1);
                this.enabled = false;
            }
        }
    }
    void TurnOffPhone()
    {
        Phone.SetActive(false);
        
        
    }
}
