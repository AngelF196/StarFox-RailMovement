using Cinemachine;
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

    [SerializeField] private float pitchSpeed = 70f;
    [SerializeField] private float pitchBound = 60f;
    [SerializeField] private float pitchLevelingSpeed = 120f;

    [SerializeField] private float forwardSpeed = 12f;

    [Header("Banking")]
    [SerializeField] private float maxBank = 55f;
    [SerializeField] private float bankSpeed = 6f;

    [Header("Camera Banking")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float cameraMaxBank = 15f;
    [SerializeField] private float cameraBankSpeed = 5f;
    private float cameraBank;

    public float yaw;
    public float pitch;
    public float yawVelocity;

    private float bank;

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
        // YAW/TURNING
        float targetYawVelocity = _leftStick.x * turnSpeed;

        float acceleration = Mathf.Abs(_leftStick.x) > 0.01f
            ? turnAcceleration
            : turnDeceleration;

        yawVelocity = Mathf.MoveTowards(yawVelocity, targetYawVelocity,
            acceleration * Time.deltaTime);

        /*
         * The amount we're currently turning determines
         * how much the Arwing banks.
         *
         * This means the bank follows the actual movement
         * rather than simply following the stick.
         */
        float turnAmount = Mathf.Clamp(
            yawVelocity / turnSpeed,
            -1f,
            1f
        );
        float targetBank = -turnAmount * maxBank;
        bank = Mathf.MoveTowards(bank, targetBank, 
            bankSpeed * maxBank * Time.deltaTime);


        /*
         * Make banking contribute to the turn.
         *
         * At maximum bank, the ship gets additional
         * turning power.
         */
        float bankInfluence = Mathf.Abs(bank) / maxBank;
        float effectiveYawVelocity =
            yawVelocity * (1f + bankInfluence);

        yaw += effectiveYawVelocity * Time.deltaTime;


        // PITCH
        if (Mathf.Abs(_leftStick.y) > 0.01f)
        {
            pitch += _leftStick.y * pitchSpeed * Time.deltaTime;
        }
        else
        {
            pitch = Mathf.MoveTowards(pitch, 0f, pitchLevelingSpeed * Time.deltaTime
            );
        }
        pitch = Mathf.Clamp(pitch, -pitchBound, pitchBound);

        // FINAL SHIP ROTATION
        transform.rotation =
            Quaternion.Euler(pitch, yaw, transform.eulerAngles.z);
    }


    private void HandleCamera()
    {
        float turnAmount = Mathf.Clamp(
            yawVelocity / turnSpeed,
            -1f,
            1f
        );

        float targetCameraBank = -turnAmount * cameraMaxBank;

        cameraBank = Mathf.MoveTowards(
            cameraBank,
            targetCameraBank,
            cameraBankSpeed * Time.deltaTime
        );

        virtualCamera.m_Lens.Dutch = cameraBank;
    }


    void Update()
    {
        HandleMovementRotation();
        HandleCamera();
    }

    void FixedUpdate()
    {
        rb.velocity = transform.forward * forwardSpeed;
    }
}