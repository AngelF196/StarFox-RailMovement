using UnityEngine;
using Cinemachine;

public class RangeCam : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform lookTarget;

    [Header("Look Ahead")]
    [SerializeField] private float lookAheadDistance = 5f;
    [SerializeField] private float lookAheadSmoothing = 5f;

    [Header("Turn Camera Offset")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float turnCameraOffset = 3f;
    [SerializeField] private float turnCameraSmoothing = 5f;

    [SerializeField] private AllRange _ship;

    private float currentLookAhead;
    private float currentCameraOffset;

    private CinemachineTransposer transposer;
    private Vector3 baseFollowOffset;

    private void Start()
    {
        _ship = FindFirstObjectByType<AllRange>();

        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();

        if (transposer != null)
            baseFollowOffset = transposer.m_FollowOffset;
    }

    private void LateUpdate()
    {
        if (player == null || lookTarget == null || _ship == null)
            return;

        float turnAmount = Mathf.Clamp(
            _ship.yawVelocity / _ship.turnSpeed,
            -1f,
            1f
        );

        // Recreate the direction the ship's nose is pointing.
        // This ignores barrel roll.
        Quaternion noseRotation =
            Quaternion.AngleAxis(_ship.yaw, Vector3.up) *
            Quaternion.AngleAxis(_ship.pitch, Vector3.right);

        Vector3 noseDirection =
            noseRotation * Vector3.forward;

        // Horizontal right direction based ONLY on yaw.
        Vector3 rightDirection =
            Quaternion.AngleAxis(_ship.yaw, Vector3.up)
            * Vector3.right;

        // -------------------------
        // LOOK TARGET
        // -------------------------

        float targetLookAhead =
            turnAmount * lookAheadDistance;

        currentLookAhead = Mathf.Lerp(
            currentLookAhead,
            targetLookAhead,
            lookAheadSmoothing * Time.deltaTime
        );

        lookTarget.position =
            player.position +
            noseDirection * lookAheadDistance +
            rightDirection * currentLookAhead;

        // -------------------------
        // CAMERA POSITION
        // -------------------------

        float targetCameraOffset =
            turnAmount * turnCameraOffset;

        currentCameraOffset = Mathf.Lerp(
            currentCameraOffset,
            targetCameraOffset,
            turnCameraSmoothing * Time.deltaTime
        );

        if (transposer != null)
        {
            Vector3 followOffset = baseFollowOffset;

            // Move the camera sideways relative to
            // the ship's current horizontal heading.
            followOffset.x += currentCameraOffset;

            transposer.m_FollowOffset = followOffset;
        }
    }
}
