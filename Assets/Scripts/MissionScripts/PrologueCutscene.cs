using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueCutscene : MonoBehaviour
{
    [SerializeField] GameObject CutsceneCamera;
    [SerializeField] GameObject CutsceneObjects;
    [SerializeField] GameObject[] NonCutsceneObjects;
    [SerializeField] GameObject Traffic;
    public GameObject Title;
    Animator anim;
    void Start()
    {
        CutsceneCamera.SetActive(true);
        anim = GetComponent<Animator>();
        anim.enabled = true;
        GetComponent<AudioSource>().enabled = true;
        Traffic.SetActive(false);
        TurnNonCutsceneObjectsOff();
        CutsceneObjects.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TurnNonCutsceneObjectsOff()
    {
        foreach(GameObject go in NonCutsceneObjects)
        {
            go.SetActive(false);
        }
    }
    public void BlackScreenFade()
    {
        //MissionTrigger.Reference.Fade();
        //MissionTrigger.Reference.LoadNewScene("Dark", 5f);
    }
    public void EnableTitle()
    {
        Title.SetActive(true);
        MissionTrigger.Reference.LoadNewScene("Dark", 10f);
    }
}
