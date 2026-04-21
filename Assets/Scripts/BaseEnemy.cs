using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

enum EnemyState
{
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
    public void Init(Transform playerTransform)
    {
        this.playerTransform = playerTransform;
        enemyState = EnemyState.CHASE;

        if (agent.enabled && agent.isOnNavMesh)
            agent.SetDestination(playerTransform.position);
    }
    public void Die()
    {
        //play animation , then destroy after death animation
        Destroy(gameObject);
    }

    public void TakeDamage(int amount , Vector3 hitPoint)
    {
        UpdateHealth(amount);

        /*if(knockReady) */StartCoroutine(KnockBackObject(hitPoint));

        if (Health <= 0)
        {
            Die();
        }


    }
    private void Update()
    {
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

    private IEnumerator KnockBackObject(Vector3 hitPoint)
    {
        knockReady = false;
        var knockDirection = transform.position - playerTransform.position;

        //rb.AddForce(knockDirection.normalized * knockBackForce, ForceMode.Impulse);
        //yield return new WaitForSeconds(knockSwitchTime);

        Vector3 pos = transform.position + knockDirection.normalized * knockBackForce;

        agent.Warp(pos);

        yield return new WaitForSeconds(2f);
        knockReady = true;

    }

}
