using UnityEngine;

public class Destroyable : MonoBehaviour,IDamageable
{
    [SerializeField] private GameObject originalObject;
    [SerializeField] private GameObject brokenObject;

    public int Health { get; set ; }

    public void Die()
    {
        
    }

    public void TakeDamage(int amount, Vector3 hitPoint)
    {
        brokenObject.SetActive(true);
        originalObject.SetActive(false);
        this.enabled = false;
    }

   
}
