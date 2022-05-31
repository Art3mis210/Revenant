using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cliff : MonoBehaviour
{
    public Vector3[] Locations;
    public string[] Objectives;
    public AudioClip[] audioC;
    public GameObject FPSCamera;
    int MissionCount;
    AudioSource audioS;
    bool update;
    void Start()
    {
        MissionCount = 0;
        MissionTrigger.Reference.ChangePos(Locations[MissionCount],Objectives[MissionCount]);
        MissionTrigger.Reference.DisableTriggerChange = true;
        audioS = GetComponent<AudioSource>();
        PlayerPrefs.SetInt("Mission", 2);
    }
    void Update()
    {
        if (MissionCount == 0)
        {
            if (EnemyTracker.Reference.Count == 6)
            {
                audioS.PlayOneShot(audioC[MissionCount]);
                MissionCount++;
                MissionTrigger.Reference.DisableTriggerChange = false;
                MissionTrigger.Reference.ChangePos(Locations[MissionCount], Objectives[MissionCount]);
               
            }
        }
        else if(MissionCount==1)
        {
            if(MissionTrigger.Reference.InTrigger && !update)
            {
                update = true;
                MissionTrigger.Reference.FadeThenUnfade();
                PlayerController.Player.EnableMovement=false;
                PlayerWeapon.playerWeapon.DisableCurrentWeapon();
                Invoke("EnableFPSCamera",4f);
            }
        }
    }
    void EnableFPSCamera()
    {
        FPSCamera.SetActive(true);
        PlayerController.Player.EnableMovement = false;
        audioS.PlayOneShot(audioC[MissionCount]);
        Invoke("KnockPlayer", 6f);
    }
    void KnockPlayer()
    {
        PlayerController.Player.transform.GetComponent<Animator>().SetTrigger("KnockedDown");
        MissionCount++;
        audioS.PlayOneShot(audioC[MissionCount]);
        MissionCount++;
        MissionTrigger.Reference.Fade();
        PlayerDataSaver.SaveGame(PlayerInventory.InventoryReference);
        Invoke("LoadNextScene", 3f);
    }
    void LoadNextScene()
    {
        audioS.PlayOneShot(audioC[MissionCount]);
        MissionTrigger.Reference.LoadNewScene("Industrial", 10f);
    }
}
