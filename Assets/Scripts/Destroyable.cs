using UnityEngine;

public class Destroyable : MonoBehaviour,IDamageable
{
    [SerializeField] private GameObject originalObject;
    [SerializeField] private GameObject brokenObject;
    bool triggered;
    public int Health { get; set ; }

    public void Die()
    {
        
    }

    public void TakeDamage(int amount, Vector3 hitPoint)
    {
        if (triggered)
            return;

        brokenObject.SetActive(true);
        originalObject.SetActive(false);
        triggered = true;
    }

   
}
