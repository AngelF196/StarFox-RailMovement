using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RangeCam : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform lookTarget;

    [Header("Look Ahead")]
    [SerializeField] private float lookAheadDistance = 5f;
    [SerializeField] private float lookAheadSmoothing = 5f;
    [SerializeField] private AllRange _ship;

    private float currentLookAhead;

    private void Start()
    {
        _ship = FindFirstObjectByType<AllRange>();
    }

    private void LateUpdate()
    {
        if (player == null || lookTarget == null)
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

        // The ship's roll is ignored here. We only want the
        // horizontal direction associated with its yaw.
        Vector3 rightDirection =
            Quaternion.AngleAxis(_ship.yaw, Vector3.up)
            * Vector3.right;

        // How far the camera pulls sideways based on turn rate.
        float targetLookAhead =
            turnAmount * lookAheadDistance;

        currentLookAhead = Mathf.Lerp(
            currentLookAhead,
            targetLookAhead,
            lookAheadSmoothing * Time.deltaTime
        );

        // Always look ahead of the ship's nose,
        // then offset sideways based on the turn.
        lookTarget.position =
            player.position +
            noseDirection * lookAheadDistance +
            rightDirection * currentLookAhead;
    }
}

