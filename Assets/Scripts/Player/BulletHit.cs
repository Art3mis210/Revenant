using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHit : MonoBehaviour
{
    Animator PlayerAnimator;
    public int FrontBulletHit;
    public bool EnableBulletHit;
    public float Health;
    private void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
        EnableBulletHit = true;
    }
    public void BulletHitReaction(Vector3 hitPoint,Vector3 hitForward)
    {
        Health--;
        if (EnableBulletHit)
        {
            if (!PlayerController.Player.InCover && PlayerController.Player.EnableMovement==true)
            {
                if (Mathf.Abs(Vector3.Angle(hitPoint-hitForward, transform.forward)) <= 45)
                {
                    FrontBulletHit++;
                    if (FrontBulletHit > 10)
                    {
                        FrontBulletHit = 0;
                        if(Health>0)
                            PlayerAnimator.SetTrigger("BulletHit");
                        EnableBulletHit = false;
                    }
                }
            }
        }
    }
}
