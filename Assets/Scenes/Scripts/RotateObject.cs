using UnityEngine;
using System;
using System.Collections;
public class RotateObject : MonoBehaviour
{
  
    [SerializeField] Transform target;
    [SerializeField] GameObject prefab;
    [SerializeField] bool spawnActive = true;
    float timeBWSpawns =0.5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Vector2 direction = mouseWorldPos - transform.position;
        //float angleinRadians = Mathf.Atan2(direction.y, direction.x);
        //float angleinDegrees = angleinRadians * Mathf.Rad2Deg;

        //transform.rotation = Quaternion.Euler(0,0,angleinDegrees);

        StartCoroutine(CoroutineFunction());

    }

    IEnumerator CoroutineFunction()
    {
        while (spawnActive)
        {
            yield return new WaitForSeconds(timeBWSpawns);

            var randomPoint = new Vector3(UnityEngine.Random.Range(-5.0f, 5.0f), 
                                          UnityEngine.Random.Range(-5.0f, 5.0f), 
                                          0f);

            Instantiate(prefab, randomPoint, Quaternion.identity);

            timeBWSpawns = UnityEngine.Random.Range(0.5f, 1.0f);
            Debug.Log(timeBWSpawns);
        }
    }
}
