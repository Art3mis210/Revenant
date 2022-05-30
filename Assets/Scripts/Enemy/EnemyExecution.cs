using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExecution : MonoBehaviour
{
    public enum EnemyType
    {
        Human,Zombie
    }
    public EnemyType CurrentEnemyType;
    Zombie ZombieController;
    Enemy EnemyController;
    RagdollManager ragdollManager;
    Animator EnemyAnimator;
    CapsuleCollider EnemyCollider;
    [SerializeField] AudioClip[] Dialogue;
    public DialogueType CurrentDialogue;
    public GameObject[] InterrogationObjects;
    AudioSource audioS;
    public bool Interrogated;
    public enum DialogueType
    {
        Team,Weapon,none
    }
    private void Start()
    {
        EnemyAnimator = transform.GetComponentInParent<Animator>();
        EnemyCollider = transform.GetComponentInParent<CapsuleCollider>();
        ragdollManager= transform.GetComponentInParent<RagdollManager>();
        if (CurrentEnemyType == EnemyType.Human)
        {
            EnemyController = transform.GetComponentInParent<Enemy>();
            audioS=gameObject.AddComponent<AudioSource>();
            audioS.spatialBlend = 1;
        }
        else
            ZombieController = transform.GetComponentInParent<Zombie>();
    }
    public void StartExecution(float Execution)
    {
        ragdollManager.DisableRagdollMode = true;
        if (CurrentEnemyType == EnemyType.Human)
            EnemyController.CurrentEnemyState = Enemy.EnemyState.Execution;
        else
            ZombieController.CurrentState = Zombie.ZombieState.Execution;
        EnemyAnimator.SetFloat("Executions", Execution);
        EnemyAnimator.SetTrigger("StartExecution");
        EnemyTracker.Reference.Count++;
        ragdollManager.Health = 0;
    }
    public void StartInterrogation(float Execution)
    {
        ragdollManager.DisableRagdollMode = true;
        EnemyController.CurrentEnemyState = Enemy.EnemyState.Execution;
        EnemyAnimator.SetInteger("InterrogatePos", 0);
        EnemyAnimator.SetTrigger("Interrogate");
        
    }
    public void InterrogationKill()
    {
        audioS.Pause();
        Destroy(audioS);
        EnemyAnimator.SetInteger("InterrogatePos", 1);
        EnemyTracker.Reference.Count++;
        ragdollManager.Health = 0;
    }
    public void InterrogationRelease()
    {
        EnemyAnimator.SetInteger("InterrogatePos", -1);
        ReduceEnemyCollider(false);
        ragdollManager.DisableRagdollMode = false;
    }
    public void Interrogate()
    {
        Interrogated = true;
        Invoke("PlayDialogue", 0.5f);
    }
    void PlayDialogue()
    {
        if (audioS != null)
        {
            if (CurrentDialogue == EnemyExecution.DialogueType.Team)
            {
                audioS.PlayOneShot(Dialogue[0]);
                foreach (GameObject go in InterrogationObjects)
                {
                    if (go.GetComponent<Outline>() == null)
                        go.AddComponent<Outline>().OutlineColor = Color.red;
                    go.GetComponent<Outline>().OutlineWidth = 10f;
                }
            }
            else if (CurrentDialogue == EnemyExecution.DialogueType.Weapon)
            {
                audioS.PlayOneShot(Dialogue[1]);
                foreach (GameObject go in InterrogationObjects)
                {
                    if (go.GetComponent<Outline>() == null)
                    {
                        go.AddComponent<Outline>().OutlineColor = Color.green;
                    }
                    go.GetComponent<Outline>().OutlineWidth = 10f;
                }
            }
            else
            {
                audioS.PlayOneShot(Dialogue[2]);
            }
        }
    }
    public void ReduceEnemyCollider(bool Status)
    {
        if(Status)
        {
            EnemyCollider.radius = 0.15f;
        }
        else
        {
            EnemyCollider.radius = 0.32f;
        }
    }
}
