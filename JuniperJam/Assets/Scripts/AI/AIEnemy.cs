using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Animancer;

public class AIEnemy : MonoBehaviour
{
    //components
    private GameObject target;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private AnimancerComponent animancer;
    [SerializeField] private Collider attackTrigger;
    [SerializeField] private Collider bodyCollider;

    //settings
    [SerializeField] private float health;
    [SerializeField] private float moveSpeed;

    [SerializeField] private float attackCooldown;
    private bool playerInRange = false;
    private bool canAttack = true;
    private float a = 0f;

    //animations
    [SerializeField] private AnimationClip walkAnim;
    [SerializeField] private AnimationClip runAnim;
    private AnimationClip moveAnim;
    [SerializeField] private List<AnimationClip> attackAnims;
    [SerializeField] private List<AnimationClip> deathAnims;

    private float pathingUpdateTimer = 0.2f;
    private float t = 0f;

    private bool dead = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAnim = moveSpeed > 3 ? runAnim : walkAnim;
        agent.speed = moveSpeed;

        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        target = GameObject.FindGameObjectWithTag("Player");
        agent.SetDestination(target.transform.position);

        animancer.Play(moveAnim);
    }

    // Update is called once per frame
    void Update()
    {
        if (dead) return;

        t += Time.deltaTime;
        if(t >= pathingUpdateTimer)
        {
            agent.SetDestination(target.transform.position);
            t = 0;
        }

        if(playerInRange)
        {
            if(canAttack)
            {
                Attack();
                canAttack = false;
            }
        }

        if(!canAttack)
        {
            a += Time.deltaTime;
            if (a >= attackCooldown)
            {
                canAttack = true;
                a = 0;
            }
        }
    }

    private void Attack()
    {
        //animancer.Layers[1].Weight = 1;
        AnimationClip currentAttack = attackAnims[Random.Range(0, attackAnims.Count)];
        animancer.Play(currentAttack);
        Invoke("OnAttackFinish", currentAttack.length);
    }

    private void OnAttackFinish()
    {
        animancer.Play(moveAnim, 0.25f);
    }

    public void TryDealDamage()
    {
        if(playerInRange && !dead)
        {
            target.TryGetComponent(out PlayerHealth playerHealth);
            playerHealth.TakeDamage(34f);
        }
    }

    public void TakeDamage(float damage)
    {
        if (dead) return; //if already dead then dont bother
        health -= damage;
        if(health <= 0)
        {
            dead = true;
            agent.enabled = false;

            animancer.Stop();
            AnimationClip deathAnim = deathAnims[Random.Range(0, deathAnims.Count)];
            animancer.Play(deathAnim);
            Invoke("DestroyAfterTime", deathAnim.length);

            attackTrigger.enabled = false;
            bodyCollider.enabled = false;

            WaveSystem.instance.KillZombie();
        }
    }

    public void SetStats(float newHealth, float newSpeed)
    {
        health = newHealth;
        moveSpeed = newSpeed;
    }

    private void DestroyAfterTime()
    {
        Destroy(gameObject, 5.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }
}