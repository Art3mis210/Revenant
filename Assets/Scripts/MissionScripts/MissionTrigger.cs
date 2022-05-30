using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MissionTrigger : MonoBehaviour
{
    public GameObject BlackScreenFade;
    BoxCollider boxC;
    Image BlackScreenAlpha;
    public bool DisableTriggerChange;
    public static MissionTrigger Reference
    {
        get;
        set;
    }
    private void Awake()
    {
        Reference = this;
        BlackScreenAlpha = BlackScreenFade.GetComponent<Image>();
        boxC = GetComponent<BoxCollider>();
    }
    public bool InTrigger;
    public void OnTriggerEnter(Collider other)
    {
        if (!DisableTriggerChange)
        {
            if (other.gameObject == PlayerController.Player.gameObject)
            {
                InTrigger = true;
                boxC.enabled = false;
            }
        }
    }
    public void ChangePos(Vector3 pos,string Objective)
    {
        transform.position = pos;
        ObjectiveMarker.Reference.ChangeTarget(gameObject, Objective);
        InTrigger = false;
        boxC.enabled = true;
    }
    public void Fade()
    {
        BlackScreenFade.GetComponent<Animator>().Rebind();
        BlackScreenFade.SetActive(true);
        PlayerController.Player.GetComponent<BulletHit>().EnableBulletHit = false;
    }
    public void UnFade()
    {
        if(BlackScreenFade.activeInHierarchy)
        {
            BlackScreenFade.GetComponent<Animator>().SetBool("Fade", false);
            PlayerController.Player.GetComponent<BulletHit>().EnableBulletHit = true;
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
    public void FadeThenUnfade()
    {
        Fade();
        Invoke("UnFade", 5f);
    }
}
