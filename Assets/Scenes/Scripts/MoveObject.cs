using UnityEngine;

public class MoveObject : MonoBehaviour
{
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;
    [SerializeField] float speed = 2f;
    [SerializeField] float minValue = 0.2f;
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        //Vector3 direction = pointA.position - pointB.position;
        //pointB.position += direction.normalized * speed * Time.deltaTime;

        //pointB.position = Vector2.Lerp(pointB.position, pointA.position, speed * Time.deltaTime);

        transform.LookAt(pointA);

        float distance = Vector3.Distance(pointB.position, pointA.position);
        //if distance to target > minValue
        if(distance>=minValue)
        {
            //walk
            animator.SetBool("run",true);
            pointB.position = Vector3.MoveTowards(pointB.position, pointA.position, speed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("run",false);
        }
        
    }
}
