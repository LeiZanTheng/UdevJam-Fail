using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissle : MonoBehaviour
{
    Rigidbody rb;
    Transform Player;
    [SerializeField] float Speed;
    [SerializeField] float steerSpeed = 10f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        var rotation = Quaternion.LookRotation(Player.position - transform.position);
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, steerSpeed * Time.deltaTime));
    }
    private void FixedUpdate() {
        rb.velocity = transform.forward * Speed;
    }
}
