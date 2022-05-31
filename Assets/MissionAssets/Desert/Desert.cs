using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desert : MonoBehaviour
{
    public Vector3[] Locations;
    public string[] Objectives;
    public AudioClip[] audioC;
    int MissionCount;
    AudioSource audioS;
    public GameObject Jack;
    bool update;
    void Start()
    {
        MissionCount = 0;
        MissionTrigger.Reference.ChangePos(Locations[MissionCount], Objectives[MissionCount]);
        MissionTrigger.Reference.DisableTriggerChange = true;
        audioS = GetComponent<AudioSource>();
        PlayerPrefs.SetInt("Mission", 4);
    }
    void Update()
    {
        if (MissionCount == 0)
        {
            if (EnemyTracker.Reference.Count == 10)
            {
                MissionCount++;
                MissionTrigger.Reference.DisableTriggerChange = false;
                MissionTrigger.Reference.ChangePos(Locations[MissionCount], Objectives[MissionCount]);
                Jack.GetComponent<Animator>().SetBool("Die",true);

            }
        }
        else if (MissionCount == 1)
        {
            if (MissionTrigger.Reference.InTrigger && !update)
            {
                update = true;
                audioS.PlayOneShot(audioC[MissionCount]);
                MissionCount++;
                PlayerDataSaver.SaveGame(PlayerInventory.InventoryReference);
                MissionTrigger.Reference.Fade();
                MissionTrigger.Reference.LoadNewScene("Forest", 10f);
            }
        }
    }
}
