using Unity.VisualScripting;
using UnityEngine;

public class Explodable : MonoBehaviour,IDamageable
{
    [SerializeField] float explosionRadius = 5f;
    [SerializeField] GameObject explosionVFX;
    public int Health { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void Die()
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(int amount, Vector3 hitPoint)
    {
        Instantiate(explosionVFX,transform.position,Quaternion.identity);
        var colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var collider in colliders)
        {
            collider.GetComponent<IDamageable>().TakeDamage(100,transform.position);
            //collider.GetComponent<Rigidbody>().add
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

}
