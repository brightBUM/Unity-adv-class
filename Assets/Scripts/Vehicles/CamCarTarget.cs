using UnityEngine;

public class CamCarTarget : MonoBehaviour
{
    public Transform car;
    public Rigidbody rb;
    public float followHeight = 6f;
    [Header("Mouse Look")]
    public float mouseSensitivity = 3f;
    public float pitchMin = -20f;
    public float pitchMax = 60f;

    [Header("Auto Align")]
    public float autoAlignSpeed = 5f;
    public float velocityThreshold = 2f;

    float yaw;
    float pitch;

    float lastInputTime;
    public float autoAlignDelay = 1.0f; // seconds after input stops

    void Start()
    {
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
    }
    private void FixedUpdate()
    {
        
    }
    Vector3 velocityRef;

    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        bool hasInput = Mathf.Abs(mouseX) > 0.01f || Mathf.Abs(mouseY) > 0.01f;

        if (hasInput)
        {
            // 🟢 FREE LOOK
            yaw += mouseX * mouseSensitivity * 100f * Time.deltaTime;
            pitch -= mouseY * mouseSensitivity * 100f * Time.deltaTime;

            pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

            lastInputTime = Time.time;
        }
        else if (Time.time - lastInputTime > autoAlignDelay)
        {
            // 🔵 AUTO ALIGN TO VELOCITY
            if (rb.linearVelocity.magnitude > velocityThreshold)
            {
                Vector3 dir = rb.linearVelocity.normalized;

                Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
                Vector3 euler = targetRot.eulerAngles;

                yaw = Mathf.LerpAngle(yaw, euler.y, Time.deltaTime * autoAlignSpeed);
            }
        }

        // Apply rotation
        transform.rotation = Quaternion.Euler(pitch, yaw, 0);

        Vector3 velocityDir = rb.linearVelocity.magnitude > 1f
           ? rb.linearVelocity.normalized
           : car.forward;

        Vector3 targetPos = car.position - velocityDir * 6f + Vector3.up * followHeight;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref velocityRef,
            0.1f
        );
    }
}