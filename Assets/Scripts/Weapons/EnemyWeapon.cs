using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public float BulletSpeed;
    Enemy EnemyController;
    [SerializeField] Vector3 NonAimPos;
    [SerializeField] Vector3 NonAimRot;

    [SerializeField] Vector3 AimPos;
    [SerializeField] Vector3 AimRot;

    public Transform Muzzle;
    public ParticleSystem MuzzleFlash;
    int Damage = 1;
    private void Start()
    {
        EnemyController = transform.GetComponentInParent<Enemy>();
    }
    public void EnableAimPos(bool Status)
    {
        if(Status)
        {
            transform.localPosition = AimPos;
            transform.localRotation = Quaternion.Euler(AimRot);
        }
        else
        {
            transform.localPosition = NonAimPos;
            transform.localRotation = Quaternion.Euler(NonAimRot);
        }
    }
    public void Shoot()
    {
        Bullet newBullet = BulletPool.Reference.GetBulletFromPool();
        newBullet.transform.position = Muzzle.position;
        newBullet.transform.rotation = Muzzle.rotation;
        newBullet.transform.gameObject.SetActive(true);
        newBullet.Damage = Damage;
        newBullet.GetComponent<Rigidbody>().AddForce(BulletSpeed * Muzzle.transform.forward);
        MuzzleFlash.Play();
    }

}
