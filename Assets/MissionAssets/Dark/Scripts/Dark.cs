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
                audioS.PlayOneShot(audioC[MissionCount]);
                MissionCount++;
                MissionTrigger.Reference.ChangePos(Locations[MissionCount], Objectives[MissionCount]);
            }
            else
            {
                audioS.PlayOneShot(audioC[MissionCount]);
            }
        }
    }
}
