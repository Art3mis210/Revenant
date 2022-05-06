using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBite : MonoBehaviour
{
    [SerializeField] float TimeToBite;
    [SerializeField] float TimeInTrigger;
    private Animator ZombieAnimator;
    private CapsuleCollider EnemyCollider;

    Transform Player;
    bool Biting;
    private void Start()
    {
        ZombieAnimator = GetComponentInParent<Animator>();
        EnemyCollider = GetComponentInParent<CapsuleCollider>();
    }

    void OnTriggerStay(Collider other)
    {
        if (!Biting)
        {
            TimeInTrigger += Time.deltaTime;
            if (TimeInTrigger >= TimeToBite)
            {
                EnemyCollider.radius = 0.15f;
                Biting = true;
                if (Player == null)
                    Player = other.transform;
                PlayerController.Player.EnableMovement = false;
                PlayerController.Player.transform.GetComponent<PlayerWeapon>().DisableAim();
                StartCoroutine(MovePlayerTowardsTrigger(1.25f));
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        TimeInTrigger = 0;
    }
    IEnumerator MovePlayerTowardsTrigger(float Duration)
    {
        float t = 0;
        ZombieAnimator.SetTrigger("ZombieBite");
        Vector3 MovePosition = new Vector3(transform.position.x, Player.position.y, transform.position.z);
        while (t < Duration)
        {
            Player.position = Vector3.Lerp(Player.position, MovePosition, t / Duration);
            Player.rotation = Quaternion.Slerp(Player.rotation, Quaternion.Euler(Player.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Player.rotation.eulerAngles.z), t / Duration); ;
            yield return null;
            t += Time.deltaTime;
        }
        Player.GetComponent<Animator>().SetTrigger("ZombieBite");
        
    }

}
