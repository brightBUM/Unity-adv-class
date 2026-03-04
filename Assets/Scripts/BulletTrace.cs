using System.Collections;
using UnityEngine;

public class BulletTrace : MonoBehaviour
{
    [SerializeField] float travelSpeed;
    [SerializeField] GameObject bulletHitVFX;
    Vector3 targetPoint;
    Vector3 origin;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public void Init(Vector3 origin,Vector3 target,bool hasTarget)
    {
        this.origin = origin;
        this.targetPoint = target;
        StartCoroutine(UpdateBulletTrace(hasTarget));
    }
   
    IEnumerator UpdateBulletTrace(bool hasTarget)
    {
        transform.position = origin;
        while(Vector3.Distance(transform.position,targetPoint)>0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPoint, travelSpeed*Time.deltaTime);
            transform.LookAt(targetPoint);
            yield return null;
        }

        //spawn hit effect
        if (hasTarget)
        {
            var spawnedVFX = Instantiate(bulletHitVFX, targetPoint, Quaternion.identity);
            spawnedVFX.transform.localScale = Vector3.one * 0.5f; 
            Destroy(spawnedVFX,2f);
        }
        //destroy self
        Destroy(gameObject);
    }
}
