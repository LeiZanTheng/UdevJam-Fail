using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneScript : MonoBehaviour
{
    Transform Target;
    Rigidbody rb;
    [SerializeField] Transform GunTip;
    [SerializeField] Transform Orientation;
    [SerializeField] GameObject Bullet;
    [SerializeField] float TimeBetweenShots;
    [SerializeField] float FlyingSpeed;
    [SerializeField] float AttackRange;
    [SerializeField] float ShootForce;
    [SerializeField] float ShootDamage;
    [SerializeField] GameObject ExplosionEffect;
    bool AllowShoot;

    private void Start() {
        Target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        AllowShoot = true;
    }
    private void Update() {
        Vector3 direction = (Target.position - transform.position).normalized;
        //orientation look
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
		Orientation.rotation = Quaternion.Slerp(Orientation.rotation, lookRotation, Time.deltaTime * 5f);
        //drone look
		Quaternion lookRot = Quaternion.LookRotation(new Vector3(direction.x,0,direction.z));
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);
        //GunTipLook
        GunTip.LookAt(Target);
        if(Vector3.Distance(transform.position, Target.position) <= AttackRange && AllowShoot)
        {
            AllowShoot = false;
            GameObject CurrentBullet = Instantiate(Bullet, GunTip.position, Quaternion.identity);
            CurrentBullet.GetComponent<Rigidbody>().AddForce(GunTip.forward * ShootForce * Time.fixedDeltaTime, ForceMode.Impulse);
            CurrentBullet.GetComponent<EnemyBullet>().Damage = ShootDamage;
            Invoke("ResetShot", TimeBetweenShots);
        }
    }
    private void FixedUpdate() {
        if(Vector3.Distance(transform.position, Target.position) > AttackRange)
        {
            rb.velocity = Orientation.forward * FlyingSpeed;
        }
        else
        {
            rb.velocity = Orientation.forward * 0;
        }
    }
    private void OnTriggerEnter(Collider other) {
        if(other.name != "DroneBullet(Clone)")
        {
            Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
            Destroy(gameObject, 0.005f);
        }
    }
    void ResetShot()
    {
        AllowShoot = true;
    }
}
