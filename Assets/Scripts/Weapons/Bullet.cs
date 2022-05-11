using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public LayerMask BodyMask;
    RaycastHit hit;
    RaycastHit BodyHit;
    Rigidbody BulletRigidbody;
    public GameObject BulletEffect;
    public GameObject BulletWallEffect;
    public float TimeToTurnOff=3f;
    float ActiveTime;
    private void Start()
    {
        BulletRigidbody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (ActiveTime <= TimeToTurnOff)
            ActiveTime += Time.deltaTime;
        else
        {
            ActiveTime = 0f;
            BulletRigidbody.velocity = Vector3.zero;
            BulletRigidbody.angularVelocity = Vector3.zero;
            gameObject.SetActive(false);
        }
    }
    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position,transform.forward,out hit,5f))
        {
            Debug.Log(hit.transform.gameObject.name);
            if (hit.transform.gameObject.GetComponent<RagdollManager>() != null)
            {
                Debug.DrawRay(hit.point,transform.forward, Color.red, 5f);
                hit.transform.gameObject.GetComponent<RagdollManager>().EnableRagdoll(true);
                if(Physics.Raycast(hit.point,transform.forward,out BodyHit,4f,BodyMask))
                {
                    Debug.Log(BodyHit.transform.gameObject.name);
                    if(BodyHit.transform.GetComponent<BulletDamage>()!=null)
                        BodyHit.transform.GetComponent<BulletDamage>().ModifyHealth();
                    BodyHit.transform.gameObject.GetComponent<Rigidbody>().AddExplosionForce(20, BodyHit.point, 50f, 70f, ForceMode.Impulse);
                    GameObject bEffect = Instantiate(BulletEffect,BodyHit.point,Quaternion.LookRotation(-BodyHit.normal));
                    bEffect.transform.parent = BodyHit.transform;

                }
            }
            else if (Physics.Raycast(hit.point, transform.forward, out BodyHit, 4f, BodyMask))
            {
                Debug.Log(BodyHit.transform.gameObject.name);
                if (BodyHit.transform.GetComponent<BulletDamage>() != null)
                    BodyHit.transform.GetComponent<BulletDamage>().ModifyHealth();
                BodyHit.transform.gameObject.GetComponent<Rigidbody>().AddExplosionForce(20, BodyHit.point, 50f, 70f, ForceMode.Impulse);
                GameObject bEffect = Instantiate(BulletEffect, BodyHit.point, Quaternion.LookRotation(-BodyHit.normal));
                bEffect.transform.parent = BodyHit.transform;

            }
            else if(hit.transform.gameObject.layer==6 || hit.transform.gameObject.layer == 3)
            {
                GameObject bEffect = Instantiate(BulletWallEffect, hit.point, Quaternion.LookRotation(-hit.normal));
                bEffect.transform.parent = BodyHit.transform;
            }
            BulletRigidbody.velocity = Vector3.zero;
            BulletRigidbody.angularVelocity = Vector3.zero;
            gameObject.SetActive(false);
            
        }
       
    }
    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
