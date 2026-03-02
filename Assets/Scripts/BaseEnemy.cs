using UnityEngine;
using UnityEngine.Events;

public class BaseEnemy : MonoBehaviour,IDamageable
{
    [SerializeField] ProgressBarUI healthBarUI;
    [SerializeField] float knockBackForce;
    int maxHealth = 100;
    Rigidbody rb;
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
    public void Die()
    {
        Debug.Log(transform.name + " died");
        Destroy(gameObject);
    }

    public void TakeDamage(int amount , Vector3 hitPoint)
    {
        UpdateHealth(amount);

        KnockBackObject(hitPoint);

        if (Health <= 0)
        {
            Die();
        }


    }

    private void UpdateHealth(int amount)
    {
        Health -= amount;
        healthBarUI.UpdateUIFillAmount((float)Health / maxHealth);
    }

    private void KnockBackObject(Vector3 hitPoint)
    {
        var knockDirection = transform.position - hitPoint;
        rb.AddForce(knockDirection.normalized * knockBackForce, ForceMode.Impulse);
    }

}
