using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class Walljump : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float groundCastRadius;
    [SerializeField] Transform groundTransform;
    [SerializeField] LayerMask groundMask;
    [SerializeField] LayerMask wallMask;
    public float jumpHeight = 8f;
    public float gravity = -30f;

    public float fallMultiplier = 2f;     // faster fall
    public float lowJumpMultiplier = 1f;  // optional

    Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, 0);

        rb.MovePosition(rb.position + move * Time.deltaTime * speed);

        bool isGrounded = Physics.CheckSphere(groundTransform.position, groundCastRadius, groundMask);
        
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            float jumpVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpVelocity, rb.linearVelocity.z);
        }
    }

    private void FixedUpdate()
    {
        // Falling
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * gravity * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        // Going up
        else if (rb.linearVelocity.y > 0)
        {
            rb.linearVelocity += Vector3.up * gravity * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundTransform.position, groundCastRadius);
    }
}
