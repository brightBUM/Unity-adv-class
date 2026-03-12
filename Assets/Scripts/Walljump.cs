using UnityEngine;

public class Walljump : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float rotSpeed = 5f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float wallJumpForce = 5f;
    [SerializeField] float groundCastDistance = 5f;
    [SerializeField] float wallCastDistance = 5f;
    [SerializeField] float gravity = -20f;
    //[SerializeField] float fallMultiplier = 2f;
    [SerializeField] Transform groundTransform;
    [SerializeField] Vector3 groundBoxsize;
    [SerializeField] LayerMask groundMask;
    [SerializeField] LayerMask wallMask;
    
    Rigidbody rb;
    Animator animator;
    Vector3 targetRotation;

    bool isGrounded;
    bool inWallRange;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Physics.gravity = new Vector3(0, gravity, 0);

    }

    private void Update()
    {
        Movement();
        LayerCheckAndJump();
    }
    private void LayerCheckAndJump()
    {
        //groundCheck

        isGrounded = Physics.CheckBox(groundTransform.position, 
                     groundBoxsize * 0.5f, Quaternion.identity, groundMask);

        //raycast in transform right
        var rayDirection = (transform.position + transform.right) - transform.position;
        inWallRange = Physics.Raycast(transform.position,
                                      rayDirection,
                                      wallCastDistance, wallMask);
        //jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

            }
            else if(inWallRange)
            {
                Vector3 jumpDirection;
                //diagonal jump
                if (transform.rotation.y>=180)
                {
                    //left   
                    jumpDirection = Vector3.right + Vector3.up;
                }
                else
                {
                    //right
                    jumpDirection = -Vector3.right + Vector3.up;

                }

                rb.linearVelocity = jumpDirection.normalized * wallJumpForce;
            }

        }

        //grounded - vertical
        //grounded & wall in range- vertical
        //not grounded & wall in range - diagonall jump 

        //rb.gravityScale = rb.linearVelocity.y < 0 ? fallMultiplier : 1f;
    }
    
    private void Movement()
    {
        // move left-right
        float xInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);

        Quaternion targetQuaternion;
        //flip the direction
        
        if(xInput!=0f)
            targetRotation = new Vector3(0f, xInput < 0 ? 180 : 0f, 0f);

        targetQuaternion = Quaternion.Euler(targetRotation);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetQuaternion, rotSpeed * Time.deltaTime);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        
        Gizmos.DrawCube(groundTransform.position, groundBoxsize);

        Gizmos.color = inWallRange ? Color.green : Color.red;

        Gizmos.DrawLine(transform.position, 
            transform.position + transform.right * wallCastDistance);
    }
}
