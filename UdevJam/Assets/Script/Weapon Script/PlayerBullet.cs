using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [HideInInspector] public float Damage;
    public float MaxDisFromPlayer = 100f;
    [Header("ExplosiveBullet")]
    public bool CanExplode;
    public float Radius;
    public float ExplosionForce;
    public GameObject ExplosionEffect; 
    bool HasExploded;
    bool HasAffectedEnemy;
    private void Start() {
        HasExploded = false;
        HasAffectedEnemy = false;
    }
    private void Update() {
        if(Vector3.Distance(transform.position, PlayerMovement.CurPos.position) >= MaxDisFromPlayer)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyAi>().DamageTheEnemy(Damage);
            Destroy(gameObject, 0.005f);
        }
        if(other.name == " HomingMissleLauncher")
        {
            other.gameObject.GetComponent<MissleShooter>().Explode();
        }
        if(CanExplode && !HasExploded)
        {
            HasExploded = true;
            Collider[] colliders = Physics.OverlapSphere(transform.position, Radius);
            Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
            foreach(Collider ObjectInRange in colliders)
            {
                Rigidbody rb = ObjectInRange.GetComponent<Rigidbody>();
                if(rb != null)
                {
                    rb.AddExplosionForce(ExplosionForce * Time.fixedDeltaTime * 50f, transform.position, Radius);
                    Debug.Log("Boom");
                }
                if(ObjectInRange.gameObject.tag == "Enemy" && !HasAffectedEnemy)
                {
                    HasAffectedEnemy = true;
                    ObjectInRange.gameObject.GetComponent<EnemyAi>().DamageTheEnemy(Damage / (Vector3.Distance(transform.position, PlayerMovement.CurPos.position) + 0.001f));
                }
                if(other.name == "BolderRobot")
                {
                    other.gameObject.GetComponent<BolderBot>().Explode();
                }
                if(other.name == " HomingMissleLauncher")
                {
                    other.gameObject.GetComponent<MissleShooter>().Explode();
                }
            }
            Destroy(gameObject, 0.005f);
        }
        if(other.name == "BolderRobot")
        {
            other.gameObject.GetComponent<BolderBot>().Explode();
        }
    }
}
