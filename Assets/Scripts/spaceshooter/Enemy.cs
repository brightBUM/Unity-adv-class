using System.Collections;
using System.Threading;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject destroyVFX;
    [SerializeField] float vfxDestroyDelay;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] float coinLifeTime =2f;
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            //destroy both
            ObjectPoolManager.Instance.Despawn(collision.gameObject, 0);

            //spawn a collectible
            GameObject coinObject = ObjectPoolManager.Instance.Spawn(1, transform.position, Quaternion.identity);
            ObjectPoolManager.Instance.Despawn(coinObject, coinLifeTime);

            //spawn & destroy vfx
            GameObject vfxObject = ObjectPoolManager.Instance.Spawn(2, transform.position, Quaternion.identity);
            ObjectPoolManager.Instance.Despawn(vfxObject, vfxDestroyDelay);

            WaveSpawner.instance.RemoveEnemy(this);

            ObjectPoolManager.Instance.Despawn(this.gameObject, 0);

        }
    }

    

    
}
