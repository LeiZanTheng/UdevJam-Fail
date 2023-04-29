using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAi : MonoBehaviour
{
    NavMeshAgent agent;
    Transform Player;
    [Header("GeneralStats")]
    public float MaxEnemyHP = 100f;
    float CurrentEnemyHP;
    public bool isShooter;
    public bool isFighter;
    public LayerMask Ground;
    public float SightRange;
    public float AttackRange;
    bool PlayerSpotted;
    bool isDead;
    //for patrol
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    public GameObject DieEffect;
    [Header("AnimationAndStuff")]
    public string[] ShootAnimationTrigger;
    public string[] FightAnimationTrigger;
    Animator animator;
    //IsShooter
    [Header("ShooterBuff")]
    public GameObject Bullet;
    public Transform firePoint;
    public float shootForce;
    public float TimeBetweenShots;
    public float ShootDamage;
    bool AllowShoot;
    public bool AnimationHasShootEvent;
    public bool NeedShootingOrientation;
    public Transform ShootingOrientation;
    //isFighter
    [Header("FighterBuff")]
    public float FightDamage;
    public float TimeBtwAttack;
    public bool HasAttackAnimation;
    bool AllowFight;
    bool IsFighting;
    bool StopChasing;
    private void Start() {
        agent = GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        PlayerSpotted = false;
        animator = GetComponent<Animator>();
        CurrentEnemyHP = MaxEnemyHP;
        isDead = false;
        AllowShoot = true;
        AllowFight = true;
        StopChasing = false;
    }
    private void Update() {
        if(Vector3.Distance(Player.position, transform.position) <= SightRange)
        {
            PlayerSpotted = true;
        }
        if(PlayerSpotted)
        {
            Vector3 direction = (Player.position - transform.position).normalized;
		    Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
		    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            //Player Being Spotted
            if(Vector3.Distance(Player.position, transform.position) > AttackRange)
            {
                ChasePlayer();
            }
            if(Vector3.Distance(Player.position, transform.position) <= AttackRange)
            {
                //attacking
                //shooter
                if(isShooter && AllowShoot && AnimationHasShootEvent)
                {
                    
                    animator.SetTrigger(ShootAnimationTrigger[Random.Range(0,ShootAnimationTrigger.Length)]);
                    AllowShoot = false;
                }
                if(isShooter && AllowShoot)
                {
                    Shoot();
                    AllowShoot = false;
                }
                //fighter chase
                if(isFighter && !StopChasing)
                {
                    ChasePlayer();
                }
            }
            //kill enemy
            if(CurrentEnemyHP <= 0 && !isDead)
            {
                isDead = true;
                Destroy(gameObject,0.005f);
                if(NeedShootingOrientation)
                {
                    Instantiate(DieEffect, ShootingOrientation.position, Quaternion.identity);
                }
                if(!NeedShootingOrientation)
                {
                    Instantiate(DieEffect, transform.position, Quaternion.identity);
                }
            }
        }
        if(!PlayerSpotted)
        {
            Patroling();
        }
    }
        private void Patroling()
    {
        if (!walkPointSet || (agent.pathStatus == NavMeshPathStatus.PathPartial)) SearchWalkPoint();
        if (agent.pathStatus == NavMeshPathStatus.PathPartial) Debug.Log("Sus");
        if (walkPointSet)
            agent.SetDestination(walkPoint);
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, Ground))
            walkPointSet = true;
    }
    void ChasePlayer()
    {
        agent.SetDestination(Player.position);
    }
    void Shoot()
    {
        Debug.Log("Bang");
        if(NeedShootingOrientation)
        {
            Vector3 direction = (Player.position - ShootingOrientation.position).normalized;
            GameObject CurrentProjectile = Instantiate(Bullet,firePoint.position, Quaternion.identity);
            CurrentProjectile.GetComponent<Rigidbody>().AddForce(direction * shootForce * Time.fixedDeltaTime, ForceMode.Impulse);
            CurrentProjectile.GetComponent<EnemyBullet>().Damage = ShootDamage;
            Invoke("ResetShoots", TimeBetweenShots);
        }
        else
        {
            Vector3 direction = (Player.position - transform.position).normalized;
            GameObject CurrentProjectile = Instantiate(Bullet,firePoint.position, Quaternion.identity);
            CurrentProjectile.GetComponent<Rigidbody>().AddForce(direction * shootForce * Time.fixedDeltaTime, ForceMode.Impulse);
            CurrentProjectile.GetComponent<EnemyBullet>().Damage = ShootDamage;
            Invoke("ResetShoots", TimeBetweenShots);
        }
    }
    void ResetShoots()
    {
        AllowShoot = true;
    }
    //Fighter
    void Fight()
    {
        IsFighting = true;
        Invoke("ResetFight", TimeBtwAttack);
    }
    void ResetFight()
    {
        AllowFight = true;
    }
    private void OnCollisionStay(Collision other) {
        if(other.gameObject.CompareTag("Player"))
        {
            StopChasing = true;
            //fighter
            if(AllowFight && HasAttackAnimation)
            {   
                AllowFight = false;
                animator.SetTrigger(FightAnimationTrigger[Random.Range(0, FightAnimationTrigger.Length)]);
            }
            if(AllowFight && !HasAttackAnimation)
            {
                AllowFight = false;
                Fight();
            }
        }
        if(IsFighting && other.gameObject.CompareTag("Player"))
        {
            IsFighting = false;
            other.gameObject.GetComponent<PlayerMovement>().GetHurt(FightDamage);
        }
        if(!other.gameObject.CompareTag("Player"))
        {
            StopChasing = false;
        }
        if(IsFighting && !other.gameObject.CompareTag("Player"))
        {
            IsFighting = false;
        }
    }
    public void DamageTheEnemy(float DamageAmount)
    {
        CurrentEnemyHP -= DamageAmount;
    }
}
