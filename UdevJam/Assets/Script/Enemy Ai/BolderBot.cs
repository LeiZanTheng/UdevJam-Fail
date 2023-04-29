using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class BolderBot : MonoBehaviour
{
    [SerializeField] float ExplosionPower;
    [SerializeField] float ExplosionRadius;
    [SerializeField] float ExplosionDamage;
    [SerializeField] float Speed;
    [SerializeField] GameObject ExplosionEffect;
    [SerializeField] Transform Orientation;
    bool HasAffectedEnemy;
    bool HasAffectedPlayer;
    Transform Player;
    Rigidbody RB;
    void Start() {
        HasAffectedEnemy = false;
        HasAffectedPlayer = false;
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        RB = GetComponent<Rigidbody>();
    }
    void FixedUpdate() {
        FollowPlayer();
    }
    public void Explode()
    {
        Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
        Collider[] ObjectsInRange = Physics.OverlapSphere(transform.position, ExplosionRadius);
        foreach(Collider TargetObject in ObjectsInRange)
        {
            Rigidbody rb = TargetObject.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(ExplosionPower, transform.position, ExplosionRadius);
            }
            if(TargetObject.CompareTag("Player") && !HasAffectedPlayer)
            {
                HasAffectedPlayer = true;
                TargetObject.GetComponent<PlayerMovement>().GetHurt(ExplosionDamage / (Vector3.Distance(transform.position, PlayerMovement.CurPos.position) + 0.001f));
                CameraShaker.Instance.ShakeOnce(100f,10f,0.1f,1f);
            }
            if(TargetObject.CompareTag("Enemy") && !HasAffectedEnemy)
            {
                HasAffectedEnemy = true;
                TargetObject.gameObject.GetComponent<EnemyAi>().DamageTheEnemy(ExplosionDamage / (Vector3.Distance(transform.position, PlayerMovement.CurPos.position) + 0.001f));
            }
        }
        Destroy(gameObject, 0.005f);
    }
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
        {
            Explode();
        }
    }
    void FollowPlayer()
    {
        Orientation.LookAt(Player);
        RB.AddForce(Orientation.forward * Speed);
    }
}
