using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class KatanaController : MonoBehaviour
{
    public string[] AttackTrigger;
    public GameObject Katana;
    [SerializeField] bool CanAttack = true;
    [SerializeField] float AttackCoolDown = 1f;
    public AudioClip KatanaSlashSound;
    AudioSource KatanaSoundEffect;
    public float Damage = 60f;
    private void Start() {
        KatanaSoundEffect = GetComponent<AudioSource>();
        CanAttack = true;
    }
    void Update()
    {
        KatanaSoundEffect.pitch = Time.timeScale;
        if(Input.GetMouseButtonDown(0))
        {
            if(CanAttack)
            {
                Attack();
            }
        }
    }
    void Attack()
    {
        //Attack Generator
        int RandomAttack = Random.Range(0,AttackTrigger.Length);
        //
        CanAttack = false;
        Animator Animation = Katana.GetComponent<Animator>();
        Animation.SetTrigger(AttackTrigger[RandomAttack]);
        KatanaSoundEffect.PlayOneShot(KatanaSlashSound);
        CameraShaker.Instance.ShakeOnce(6f, 1f, 0.4f, 0.2f);
        Invoke("ResetAttack", AttackCoolDown);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyAi>().DamageTheEnemy(Damage);
        }
        if(other.name == "HomingMissleShooter")
        {
            other.gameObject.GetComponent<MissleShooter>().Explode();
        }
    }
    void ResetAttack()
    {
        CanAttack = true;
    }
}
