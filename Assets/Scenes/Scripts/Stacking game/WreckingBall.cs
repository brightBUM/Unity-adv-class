using UnityEngine;


public class WreckingBall : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform pivotPoint;

    [Header("Swing Settings")]
    [SerializeField] float initialPushForce = 5f;   // Starting impulse
    [SerializeField] float motorForce = 50f;         // HingeJoint motor force
    [SerializeField] float motorSpeed = 60f;         // Degrees per second target speed
    [SerializeField] float maxSwingAngle = 55f;      // Degrees each side before reversing

    [Header("Knock")]
    [SerializeField] float knockBounceForce = 3f;
    [SerializeField] LineRenderer lineRenderer;

    Rigidbody rb;
    HingeJoint hinge;
    float swingDir = 1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        hinge = GetComponent<HingeJoint>();

        if (hinge == null)
        {
            Debug.LogError("WreckingBall: No HingeJoint found! Please add one in the Inspector.");
            return;
        }

        // Set hinge limits
        JointLimits limits = hinge.limits;
        limits.min = -maxSwingAngle;
        limits.max = maxSwingAngle;
        limits.bounciness = 0.1f;
        hinge.limits = limits;
        hinge.useLimits = true;

        // Enable motor to drive the swing
        SetMotor(swingDir);
        hinge.useMotor = true;

        // Initial push to get it going
        rb.AddForce(transform.right * initialPushForce, ForceMode.Impulse);
        lineRenderer.SetPosition(0, pivotPoint.position);

    }

    private void Update()
    {
        if (hinge == null || pivotPoint == null) return;

        float angle = hinge.angle;

        // Reverse motor when approaching limits
        if (angle >= maxSwingAngle - 2f && swingDir > 0)
        {
            swingDir = -1f;
            SetMotor(swingDir);
        }
        else if (angle <= -maxSwingAngle + 2f && swingDir < 0)
        {
            swingDir = 1f;
            SetMotor(swingDir);
        }
        lineRenderer.SetPosition(1, rb.position);
;    }

    void SetMotor(float direction)
    {
        JointMotor motor = hinge.motor;
        motor.targetVelocity = motorSpeed * direction;
        motor.force = motorForce;
        motor.freeSpin = false;
        hinge.motor = motor;
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayerStack player = collision.gameObject.GetComponent<PlayerStack>();

        if (player != null)
        {
            player.KnockStack();

            // Slight bounce back on hit for juice
            //Vector3 bounceDir = (transform.position - collision.contacts[0].point).normalized;
            //rb.AddForce(bounceDir * knockBounceForce, ForceMode.Impulse);
        }
    }

    public void SetSwinging(bool state)
    {
        hinge.useMotor = state;
        rb.isKinematic = !state;
    }
}