using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Explodable : MonoBehaviour,IDamageable
{
    [SerializeField] float explosionRadius = 5f;
    [SerializeField] GameObject explosionVFX;
    [SerializeField] LayerMask damageLayer;
    [SerializeField] GameObject debugCircle;
    [SerializeField] bool debugRadius;
    bool triggered;
    public int Health { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void Die()
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(int amount, Vector3 hitPoint)
    {
        if (triggered)
            return;

        triggered = true;

        StartCoroutine(DelayedExplosion());
    }

    IEnumerator DelayedExplosion()
    {
        yield return new WaitForSeconds(0.25f);

        Instantiate(explosionVFX, transform.position, Quaternion.identity);
        var colliders = Physics.OverlapSphere(transform.position, explosionRadius, damageLayer);
        //Debug.Log(transform.name);
        foreach (var collider in colliders)
        {
            if (collider.transform == this.transform)
                continue;

            //Debug.Log(collider.name);
            var damageables = collider.GetComponents<IDamageable>();
            foreach(var damageable in damageables)
            {
                    damageable.TakeDamage(100, transform.position);
            }
            
        }
    }

    private void OnDrawGizmos()
    {
        if (debugCircle == null) return;

        debugCircle.SetActive(debugRadius ? true : false);
    }

}
