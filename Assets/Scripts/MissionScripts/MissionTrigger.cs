using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionTrigger : MonoBehaviour
{
    public GameObject BlackScreenFade;
    public static MissionTrigger Reference
    {
        get;
        set;
    }
    private void Start()
    {
        Reference = this;
    }
    public bool InTrigger;
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject==PlayerController.Player.gameObject)
        {
            InTrigger = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject == PlayerController.Player.gameObject)
        {
            InTrigger = false;
        }
    }
    public void ChangePos(Vector3 pos,string Objective)
    {
        transform.position = pos;
        ObjectiveMarker.Reference.ChangeTarget(gameObject, Objective);
    }
    public void Fade()
    {
        BlackScreenFade.GetComponent<Animator>().Rebind();
        BlackScreenFade.SetActive(true);
    }
    public void UnFade()
    {
        if(BlackScreenFade.activeInHierarchy)
        {
            BlackScreenFade.GetComponent<Animator>().SetBool("Fade", false);
        }
    }
}
