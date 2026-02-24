using UnityEngine;
using UnityEngine.Events;

public class BaseEnemy : MonoBehaviour,IDamageable
{
    public UnityEvent exampleEvent;
    public UnityAction exampleAction;
    public int Health 
    { 
        get;
        set;
    }
    private void Start()
    {
        Health = 100;
    }
    public void Die()
    {
        Debug.Log(transform.name + " died");
        Destroy(gameObject);
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
        if(Health<=0)
        {
            Die();
        }
    }

    
}
