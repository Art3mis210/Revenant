using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest : MonoBehaviour
{

    public Vector3[] Locations;
    public string[] Objectives;
    int MissionCount = 0;

    bool update;
    void Start()
    {
        MissionTrigger.Reference.ChangePos(Locations[MissionCount], Objectives[MissionCount]);
        MissionTrigger.Reference.DisableTriggerChange = true;
        PlayerPrefs.SetInt("Mission", 5);
    }
    void Update()
    {
        if (MissionCount == 0)
        {
            if (EnemyTracker.Reference.Count == 10)
            {
                MissionCount++;
                MissionTrigger.Reference.DisableTriggerChange = false;
                PlayerDataSaver.SaveGame(PlayerInventory.InventoryReference);
                MissionTrigger.Reference.Fade();
                MissionTrigger.Reference.LoadNewScene("Menu", 10f);

            }
        }
    }
}
