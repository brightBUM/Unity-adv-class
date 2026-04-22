using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

enum EnemyState
{
    SPAWN,
    CHASE,
    ATTACK,
    DAMAGE,
    DIE
}

public class BaseEnemy : MonoBehaviour,IDamageable
{
    [SerializeField] ProgressBarUI healthBarUI;
    [SerializeField] float knockBackForce;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform playerTransform;
    [SerializeField] Animator animator;
    [SerializeField] float knockSwitchTime = 0.2f;
    [SerializeField] float timeBWattacks = 0.2f;
    [SerializeField] EnemyState enemyState;
    int maxHealth = 100;
    Rigidbody rb;
    bool knockReady = true;
    bool attackinProgress;
    public int Health 
    { 
        get;
        set;
    }
    private void Start()
    {
        Health = maxHealth;
        rb = GetComponent<Rigidbody>();
    }
    public void Init(Transform playerTransform, Vector3 targetPos)
    {
        StartCoroutine(SpawnToPosition(playerTransform,targetPos));
    }
    IEnumerator SpawnToPosition(Transform playerTransform, Vector3 targetPos)
    {

        float timer = 0f;
        while (timer < 0.75f)
        {
            //Debug.Break();
            transform.position = Vector3.MoveTowards(transform.position, targetPos, 3f * Time.deltaTime);
            timer+= Time.deltaTime;
            yield return null;
        }


        agent.enabled = true;

        //transform.position = new Vector3(transform.position.x, 0f, transform.position.z);

        this.playerTransform = playerTransform;
        enemyState = EnemyState.CHASE;
        
        
        if (agent.enabled && agent.isOnNavMesh)
            agent.SetDestination(playerTransform.position);

        GetComponent<CapsuleCollider>().enabled = true;
    }
    public void Die()
    {
        //Debug.Break();
        if (agent.enabled && agent.isOnNavMesh)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            agent.enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
        }
        healthBarUI.ToggleProgressBar(false);
        enemyState = EnemyState.DIE;

        //play animation , then destroy after death animation
        animator.ResetTrigger("hit");
        animator.SetTrigger("death");
        var randomNum = Random.Range(1, 3);
        //Debug.Log($"death index : {randomNum}");
        animator.SetInteger("deathIndex", randomNum);
    }

    public void TakeDamage(int amount , Vector3 hitPoint)
    {
        if(enemyState==EnemyState.DIE)
            return;

        UpdateHealth(amount);

        if(knockReady)
        {
            knockReady = false;
            enemyState = EnemyState.DAMAGE;

            //STOP movement immediately
            if (agent.enabled && agent.isOnNavMesh)
            {
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
            }

            animator.SetTrigger("hit");
            KnockBackObject(hitPoint);
        }

        //Debug.Break();
        if (Health <= 0)
        {
            Die();
        }


    }
    private void Update()
    {
        if (enemyState == EnemyState.DAMAGE || enemyState == EnemyState.DIE)
            return; 

        if (agent.enabled && agent.isOnNavMesh)
            agent.SetDestination(playerTransform.position);


        switch (enemyState)
        {
            case EnemyState.CHASE:

                //entry - on state initialization
                animator.SetBool("run", true);

                //update
                

                //exit
                agent.updateRotation = true;
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    //attack
                    enemyState = EnemyState.ATTACK;

                }
                break;

            case EnemyState.ATTACK:

                //entry
                

                //update 
                if (!attackinProgress)
                {
                    StartCoroutine(PerformAttack());
                }

                //exit
                //if player not in range 
                if (agent.remainingDistance > agent.stoppingDistance)
                {
                    //attack
                    enemyState = EnemyState.CHASE;

                }
                break;
            case EnemyState.DAMAGE:



                break;
            case EnemyState.DIE:



                break;
        }

        

        
    }

    IEnumerator PerformAttack()
    {
        attackinProgress = true;
        animator.SetTrigger("attack");

        yield return new WaitForSeconds(timeBWattacks);

        attackinProgress = false;

    }
    private void UpdateHealth(int amount)
    {
        Health -= amount;
        healthBarUI.UpdateUIFillAmount((float)Health / maxHealth);
    }

    private void KnockBackObject(Vector3 hitPoint)
    {
        
        var knockDirection = transform.position - playerTransform.position;

        //rb.AddForce(knockDirection.normalized * knockBackForce, ForceMode.Impulse);
        //yield return new WaitForSeconds(knockSwitchTime);

        Vector3 pos = transform.position + knockDirection.normalized * knockBackForce;

        agent.Warp(pos);

    }

    public void OnHitAnimComplete()
    {
        knockReady = true;
        enemyState = EnemyState.CHASE;
    }
    public void OnDeathAnimComplete()
    {
        Destroy(gameObject);
    }
}
