using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class EnemyBullet : MonoBehaviour
{
    [HideInInspector]public float Damage;
    public float MaxDisFromPlayer = 100f;
    [Header("ExplosiveBullet")]
    public bool CanExplode;
    public float Radius;
    public float ExplosionForce;
    public GameObject ExplosionEffect; 
    bool HasExploded;
    bool HasAffectPlayer;
    private void Start() {
        HasExploded = false;
        HasAffectPlayer = false;
    }
    private void Update() {
        if(Vector3.Distance(transform.position, PlayerMovement.CurPos.position) >= MaxDisFromPlayer)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player" && !CanExplode)
        {
            other.gameObject.GetComponent<PlayerMovement>().GetHurt(Damage);
            Destroy(gameObject, 0.005f);
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
                if(ObjectInRange.gameObject.tag == "Player" && !HasAffectPlayer)
                {
                    HasAffectPlayer = true;
                    ObjectInRange.gameObject.GetComponent<PlayerMovement>().GetHurt(Damage / (Vector3.Distance(transform.position, PlayerMovement.CurPos.position) + 0.001f));
                    CameraShaker.Instance.ShakeOnce(100f,10f,0.1f,1f);
                }
            }
            Destroy(gameObject, 0.005f);
        }
    }
}
