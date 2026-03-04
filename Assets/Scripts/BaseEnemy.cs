using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class BaseEnemy : MonoBehaviour,IDamageable
{
    [SerializeField] ProgressBarUI healthBarUI;
    [SerializeField] float knockBackForce;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] float knockSwitchTime = 0.2f;
    int maxHealth = 100;
    Rigidbody rb;
    Transform playerTransform;
    bool knockReady = true;
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
    }
    public void Die()
    {
        Debug.Log(transform.name + " died");
        Destroy(gameObject);
    }

    public void TakeDamage(int amount , Vector3 hitPoint)
    {
        UpdateHealth(amount);

        if(knockReady) StartCoroutine(KnockBackObject(hitPoint));

        if (Health <= 0)
        {
            Die();
        }


    }
    private void Update()
    {
        if(agent.enabled && agent.isOnNavMesh)
            agent.SetDestination(playerTransform.position);
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
