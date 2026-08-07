using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AllRange : MonoBehaviour
{

    public Transform cameraParent;

    private Vector2 _leftStick;
    private Vector2 _rightStick;
    private Rigidbody rb;
    private ShipActions shipActions;

    [Header("Movement")]
    [SerializeField] public float turnSpeed = 60f;
    [SerializeField] private float turnAcceleration = 180f;
    [SerializeField] private float turnDeceleration = 240f;
    public float pitchSpeed = 70f;
    public float pitchBound;
    public float pitchLevelingSpeed;
    public float forwardSpeed = 12f;

    public float yaw;
    public float pitch;
    public float yawVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Start from the ship's current rotation
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;

        // Convert 0-360 Euler angle into -180 to 180
        if (pitch > 180f)
            pitch -= 360f;
    }


    public void LeftStickHandle(Vector2 leftStick)
    {
        _leftStick = leftStick;
    }

    public void RightStickHandle(Vector2 rightStick)
    {
        _rightStick = rightStick;
    }

    private void HandleMovementRotation()
    {
        // Calculate the desired yaw speed based on stick input
        float targetYawVelocity = _leftStick.x * turnSpeed;

        // Accelerate toward the target turn speed
        float acceleration = Mathf.Abs(_leftStick.x) > 0.01f
            ? turnAcceleration
            : turnDeceleration;

        yawVelocity = Mathf.MoveTowards(
            yawVelocity,
            targetYawVelocity,
            acceleration * Time.deltaTime
        );

        // Apply yaw
        yaw += yawVelocity * Time.deltaTime;


        // Up/down pitches the ship
        if (Mathf.Abs(_leftStick.y) > 0)
        {
            pitch += _leftStick.y * pitchSpeed * Time.deltaTime;
        }
        else pitch = Mathf.MoveTowards(pitch, 0f, pitchLevelingSpeed * Time.deltaTime);

        // Keep the ship upright
        pitch = Mathf.Clamp(pitch, -pitchBound, pitchBound);

        // Apply yaw and pitch
        transform.rotation =
            Quaternion.AngleAxis(yaw, Vector3.up) *
            Quaternion.AngleAxis(pitch, Vector3.right);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.forward * forwardSpeed;
        HandleMovementRotation();
    }
}
