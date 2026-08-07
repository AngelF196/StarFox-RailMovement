using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.InputSystem;
using System.Threading;
using UnityEngine.Events;

public class ShipActions : MonoBehaviour
{

    [Header("Attacking")]
    public GameObject lazerPrefab;
    public GameObject bombPrefab;

    [Header("G Diffuser")]
    public bool wantsBoost = false;
    public bool wantsBrake = false;

    [Header("Stick Reading")]
    public Vector2 leftStick;
    public Vector2 rightStick;
    public UnityEvent<Vector2> leftStickRead;
    public UnityEvent<Vector2> rightStickRead;

    [Header("References")]
    [SerializeField] private Transform _shipModel;

    [Header("Rolling")]
    [SerializeField] private float _rollBound;
    [SerializeField] private float _rollSpeed;

    private float rollOffset = 0;
    private float currentZ;
    private Coroutine rollCoroutine;

    public enum controlType { ArwingC, ArwingR, Landmaster}

    private void Update()
    {

    }

    // Attacking
    public void Bomb(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Bomb lastBomb = FindObjectOfType<Bomb>();
            if (lastBomb is null)
            {
                Instantiate(bombPrefab, transform.position, Quaternion.LookRotation(transform.forward));
            }
            else lastBomb.Explode();
        }
    }
    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        Instantiate(lazerPrefab, transform.position, Quaternion.LookRotation(transform.forward));
    }

    // Tilting
    public void ZTilt(float axis, int tiltDir, int rollDir)
    {
        float targetZ = 0f;

        // base tilting
        targetZ = -axis * _rollBound;

        // L/R Hard Tilt
        if (tiltDir != 0) targetZ = 90f * -tiltDir;

        currentZ = Mathf.LerpAngle(currentZ, targetZ, _rollSpeed);

        if (rollDir != 0 && rollCoroutine is null)
        {
            rollCoroutine = StartCoroutine(BarrelRoll(rollDir));
            //barrel.Play();

        }

        _shipModel.localEulerAngles = new Vector3(_shipModel.localEulerAngles.x,
        _shipModel.localEulerAngles.y,
        // Build off a Z value that is not continuously added to
        currentZ + rollOffset);
    }
    private IEnumerator BarrelRoll(int rollDir)
    {
        float rollDuration = 0.33f;
        float timer = 0;

        while (timer < 1f)
        {
            timer += Time.deltaTime / rollDuration;
            rollOffset = Mathf.SmoothStep(0, 360 * -rollDir, timer);
            yield return null;
        }
        rollOffset = 0f;
        rollCoroutine = null;
    }

    // Braking and Boosting
    public void BoostHandle(InputAction.CallbackContext context)
    {
        if (context.performed) wantsBoost = true;
        else if (context.canceled) wantsBoost = false;
    }

    public void BrakeHandle(InputAction.CallbackContext context)
    {
        if (context.performed) wantsBrake = true;
        else if (context.canceled) wantsBrake = false;
    }

    // Stick Input
    public void LeftStick(InputAction.CallbackContext context)
    {
        leftStick = context.ReadValue<Vector2>();
        leftStickRead.Invoke(leftStick);
    }
    public void RightStick(InputAction.CallbackContext context)
    {
        rightStick = context.ReadValue<Vector2>();
        rightStickRead.Invoke(rightStick);
    }
}