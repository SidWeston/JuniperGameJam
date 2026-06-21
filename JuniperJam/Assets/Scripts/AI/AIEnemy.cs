using UnityEngine;
using UnityEngine.AI;
using Animancer;

public class AIEnemy : MonoBehaviour
{
    //components
    private GameObject target;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private AnimancerComponent animancer;

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
    [SerializeField] private AnimationClip attackAnim;

    private float pathingUpdateTimer = 0.2f;
    private float t = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        agent.SetDestination(target.transform.position);

        animancer.Play(walkAnim);
    }

    // Update is called once per frame
    void Update()
    {
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
            else
            {
                a += Time.deltaTime;
                if(a >= attackCooldown)
                {
                    canAttack = true;
                    a = 0;
                }
            }
        }
    }

    private void Attack()
    {
        animancer.Layers[1].Stop();
        animancer.Layers[1].Play(attackAnim);
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