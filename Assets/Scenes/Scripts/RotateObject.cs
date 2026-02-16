using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] Vector3 axis;
    [SerializeField] float amount;
    [SerializeField] Transform target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(axis, amount);
        transform.LookAt(target);
    }
}
