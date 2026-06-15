using UnityEngine;

public class SimpleCarController : MonoBehaviour
{
    [Header("Wheel Colliders")]
    public WheelCollider frontLeft;
    public WheelCollider frontRight;
    public WheelCollider rearLeft;
    public WheelCollider rearRight;

    [Header("Wheel Meshes")]
    public Transform frontLeftMesh;
    public Transform frontRightMesh;
    public Transform rearLeftMesh;
    public Transform rearRightMesh;

    [Header("Car Settings")]
    public float motorForce = 1500f;
    public float brakeForce = 3000f;
    public float maxSteerAngle = 30f;

    float horizontalInput;
    float verticalInput;
    float currentBrakeForce;
    bool isBraking;

    void Update()
    {
        // Input
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isBraking = Input.GetKey(KeyCode.Space);
    }

    void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    void HandleMotor()
    {
        // Apply motor force to rear wheels (RWD)
        rearLeft.motorTorque = verticalInput * motorForce;
        rearRight.motorTorque = verticalInput * motorForce;

        // Braking
        currentBrakeForce = isBraking ? brakeForce : 0f;
        ApplyBraking();
    }

    void ApplyBraking()
    {
        frontLeft.brakeTorque = currentBrakeForce;
        frontRight.brakeTorque = currentBrakeForce;
        rearLeft.brakeTorque = currentBrakeForce;
        rearRight.brakeTorque = currentBrakeForce;
    }

    void HandleSteering()
    {
        float steerAngle = maxSteerAngle * horizontalInput;
        frontLeft.steerAngle = steerAngle;
        frontRight.steerAngle = steerAngle;
    }

    void UpdateWheels()
    {
        UpdateSingleWheel(frontLeft, frontLeftMesh);
        UpdateSingleWheel(frontRight, frontRightMesh);
        UpdateSingleWheel(rearLeft, rearLeftMesh);
        UpdateSingleWheel(rearRight, rearRightMesh);
    }

    void UpdateSingleWheel(WheelCollider collider, Transform mesh)
    {
        Vector3 pos;
        Quaternion rot;
        collider.GetWorldPose(out pos, out rot);

        mesh.position = pos;
        mesh.rotation = rot;
    }
}