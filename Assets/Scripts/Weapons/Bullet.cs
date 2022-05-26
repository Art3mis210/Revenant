using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public LayerMask BodyMask;
    RaycastHit hit;
    RaycastHit BodyHit;
    Rigidbody BulletRigidbody;
    TrailRenderer bulletTrail;
    public float TimeToTurnOff=3f;
    private void Start()
    {
        BulletRigidbody = GetComponent<Rigidbody>();
        bulletTrail = GetComponent<TrailRenderer>();
    }
    void OnEnable()
    {
        if (bulletTrail != null)
            bulletTrail.enabled = true;
        StartCoroutine(TurnOff());
        if(BulletRigidbody!=null)
            BulletRigidbody.isKinematic = false;
    }
    private void OnDisable()
    {
        if (BulletRigidbody != null)
            BulletRigidbody.isKinematic = true;
        StopAllCoroutines();
    }
    IEnumerator TurnOff()
    {
        yield return new WaitForSeconds(TimeToTurnOff);
        BulletRigidbody.velocity = Vector3.zero;
        BulletRigidbody.angularVelocity = Vector3.zero;
        bulletTrail.enabled = false;
        gameObject.SetActive(false);
    }
    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position,transform.forward,out hit,5f))
        {
            if (hit.transform.gameObject.layer != 7)
            {
                if (hit.transform.gameObject.GetComponent<RagdollManager>() != null)
                {
                    Debug.DrawRay(hit.point, transform.forward, Color.red, 5f);
                    hit.transform.gameObject.GetComponent<RagdollManager>().EnableRagdoll(true);
                    if (Physics.Raycast(hit.point, transform.forward, out BodyHit, 4f, BodyMask))
                    {
                        Debug.Log(BodyHit.transform.gameObject.name);
                        if (BodyHit.transform.GetComponent<BulletDamage>() != null)
                            BodyHit.transform.GetComponent<BulletDamage>().ModifyHealth();
                        BodyHit.transform.gameObject.GetComponent<Rigidbody>().AddExplosionForce(20, BodyHit.point, 50f, 70f, ForceMode.Impulse);
                        GameObject bEffect = BulletFleshImpactPool.Reference.GetBulletFleshImpactFromPool(); //Instantiate(BulletFleshImpactPool.Reference.GetBulletFleshImpactFromPool(), hit.point, Quaternion.LookRotation(-hit.normal));
                        bEffect.transform.position = BodyHit.point;
                        bEffect.transform.rotation = Quaternion.LookRotation(-BodyHit.normal);
                        bEffect.transform.parent = BodyHit.transform;
                        bEffect.SetActive(true);

                    }
                }
                else if (Physics.Raycast(hit.point, transform.forward, out BodyHit, 4f, BodyMask))
                {
                    if (BodyHit.transform.GetComponent<BulletDamage>() != null)
                        BodyHit.transform.GetComponent<BulletDamage>().ModifyHealth();
                    BodyHit.transform.gameObject.GetComponent<Rigidbody>().AddExplosionForce(20, BodyHit.point, 50f, 70f, ForceMode.Impulse);
                    GameObject bEffect = BulletFleshImpactPool.Reference.GetBulletFleshImpactFromPool(); //Instantiate(BulletFleshImpactPool.Reference.GetBulletFleshImpactFromPool(), hit.point, Quaternion.LookRotation(-hit.normal));
                    bEffect.transform.position = BodyHit.point;
                    bEffect.transform.rotation = Quaternion.LookRotation(-BodyHit.normal);
                    bEffect.transform.parent = BodyHit.transform;
                    bEffect.SetActive(true);

                }
                else if (hit.transform.gameObject.layer == 6 || hit.transform.gameObject.layer == 3)
                {
                    GameObject bEffect = BulletWallImpactPool.Reference.GetBulletWallImpactFromPool(); //Instantiate(BulletFleshImpactPool.Reference.GetBulletFleshImpactFromPool(), hit.point, Quaternion.LookRotation(-hit.normal));
                    bEffect.transform.position = hit.point;
                    bEffect.transform.rotation = Quaternion.LookRotation(-hit.normal);
                    bEffect.transform.parent = hit.transform;
                    bEffect.SetActive(true);
                }
                else if (hit.transform.tag == "Player")
                {
                    hit.transform.GetComponent<BulletHit>().BulletHitReaction(hit.point, transform.forward);
                    /* if (hit.transform.GetComponent<BulletHit>().EnableBulletHit)
                     {
                         GameObject bEffect = BulletFleshImpactPool.Reference.GetBulletFleshImpactFromPool(); //Instantiate(BulletFleshImpactPool.Reference.GetBulletFleshImpactFromPool(), hit.point, Quaternion.LookRotation(-hit.normal));
                         bEffect.transform.position = hit.point + 0.1f * transform.forward;
                         bEffect.transform.rotation = Quaternion.LookRotation(-hit.normal);
                         bEffect.transform.parent = hit.transform;
                         bEffect.SetActive(true);
                     }*/
                }

                BulletRigidbody.velocity = Vector3.zero;
                BulletRigidbody.angularVelocity = Vector3.zero;
                transform.GetComponent<TrailRenderer>().enabled = false;
                gameObject.SetActive(false);
            }
            
        }
       
    }
}
