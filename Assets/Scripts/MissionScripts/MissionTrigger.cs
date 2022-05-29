using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MissionTrigger : MonoBehaviour
{
    public GameObject BlackScreenFade;
    Image BlackScreenAlpha;
    public static MissionTrigger Reference
    {
        get;
        set;
    }
    private void Start()
    {
        Reference = this;
        BlackScreenAlpha = BlackScreenFade.GetComponent<Image>();
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
    public void LoadNewScene(string SceneName,float Duration)
    {
        StartCoroutine(LoadSceneAfter(SceneName,Duration));
    }
    IEnumerator LoadSceneAfter(string SceneName,float Duration)
    {
        yield return new WaitForSeconds(Duration);
        SceneManager.LoadScene(SceneName);
        
    }
}
