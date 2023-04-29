using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleShooter : MonoBehaviour
{
    [SerializeField] GameObject Missle;
    [SerializeField] float TimeBtwLaunch = 10f;
    [SerializeField] GameObject BoomEffect;
    [SerializeField] Transform ShootPoint;
    [SerializeField] float AttackDis = 30f;
    bool AllowLaunch;
    Transform player;
    private void Start() {
        AllowLaunch = true;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    public void Explode()
    {
        Instantiate(BoomEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    void Launch()
    {
        AllowLaunch = false;
        Instantiate(Missle, ShootPoint.position, Quaternion.identity);
        Invoke("ResetLaunch", TimeBtwLaunch);
    }
    void ResetLaunch()
    {
        AllowLaunch = true;
    }
    private void Update() {
        if(Vector3.Distance(transform.position, player.position) <= AttackDis && AllowLaunch)
        {
            Launch();
        }
    }
}
