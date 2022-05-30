using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Industrial : MonoBehaviour
{
    public Vector3 JackLocation;
    bool JackFound;
    [SerializeField] AudioClip[] audioC;
    void Start()
    {
        PlayerController.Player.EnableMovement = false;
        MissionTrigger.Reference.ChangePos(JackLocation, "Rescue Jack");
        Invoke("PlayDialogue", 10f);
    }
    private void Update()
    {
        if(MissionTrigger.Reference.InTrigger)
        {
            if(JackFound==false)
            {
                JackFound = true;
                MissionTrigger.Reference.Fade();
                transform.GetComponent<AudioSource>().PlayOneShot(audioC[1]);
                MissionTrigger.Reference.LoadNewScene("Desert", 20f);
            }
        }
    }
    void PlayDialogue()
    {
        transform.GetComponent<AudioSource>().PlayOneShot(audioC[0]);
    }
}
