using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dark : MonoBehaviour
{
    public Vector3[] Locations;
    public string[] Objectives;
    public AudioClip[] audioC;
    AudioSource audioS;
    int MissionCount;
    void Start()
    {
        MissionCount = 0;
        MissionTrigger.Reference.ChangePos(Locations[0], Objectives[0]);
        audioS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(MissionTrigger.Reference.InTrigger)
        {
            if (MissionCount < Locations.Length-1)
            {
                MissionTrigger.Reference.FadeThenUnfade();
                Invoke("PlayAudio", 5f);
                MissionCount++;
                MissionTrigger.Reference.ChangePos(Locations[MissionCount], Objectives[MissionCount]);
            }
            else
            {
                
                audioS.PlayOneShot(audioC[MissionCount]);
                MissionTrigger.Reference.Fade();
                MissionTrigger.Reference.LoadNewScene("Cliff", 5f);
                MissionCount = -1;
            }
        }
    }
    void PlayAudio()
    {
        audioS.PlayOneShot(audioC[MissionCount-1]);
    }
}
