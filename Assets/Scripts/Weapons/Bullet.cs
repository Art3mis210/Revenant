using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public LayerMask BodyMask;
    RaycastHit hit;
    RaycastHit BodyHit;
    Rigidbody BulletRigidbody;
    private void Start()
    {
        BulletRigidbody = GetComponent<Rigidbody>();
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
                    BodyHit.transform.gameObject.GetComponent<Rigidbody>().AddExplosionForce(50, BodyHit.point, 50f, 70f, ForceMode.Impulse);
                }
            }
            else if (hit.transform.gameObject.layer == BodyMask)
            {
                hit.transform.gameObject.GetComponent<Rigidbody>().AddExplosionForce(50, hit.point, 50f, 70f, ForceMode.Impulse);
            }
            gameObject.SetActive(false);
        }
       
    }
    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
