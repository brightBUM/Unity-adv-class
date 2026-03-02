using UnityEngine;

public interface IDamageable
{
    public int Health { get; set; }
    public void TakeDamage(int amount,Vector3 hitPoint);
    public void Die();
}
