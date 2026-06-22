using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    TrailRenderer bombtrail;
    private void Awake()
    {
        bombtrail = GetComponent<TrailRenderer>();

    }
   
    private void OnEnable()
    {
        bombtrail.Clear();
    }
    
    private void OnDisable()
    {
        bombtrail.Clear();

    }
}
