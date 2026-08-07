using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.InputSystem;

public class UniMovement : MonoBehaviour
{
    private Transform playerModel;
    private Inputs input;
    private float rollOffset = 0;
    private float currentZ;
    private Coroutine rollCoroutine;

    [Header("Public References")]
    public Transform aimTarget;
    public CinemachineDollyCart dolly;
    public Transform cameraParent;

    [Header("Parameters")]
    public float xSpeed = 38;
    public float ySpeed = 38;
    public float xySpeed = 38;
    public float lookSpeed = 340;
    public float forwardSpeed = 6;
    public float horizLeanLimit = 80;
    public float horizLeanTime = 0.1f;
    public float xShipNoseTiltDivisor = 2;
    public float yShipNoseTiltDivisor = 2;

    void Start()
    {
        playerModel = transform.GetChild(0);
        input = GetComponent<Inputs>();
        SetSpeed(forwardSpeed);
    }

    public void LocalMove(Vector2 inputaxis)
    {
        Vector3 camForward = cameraParent.forward;

        // Project forward onto camera plane
        Vector3 targetTransform = Vector3.ProjectOnPlane(transform.forward, camForward);

        transform.position += targetTransform * xySpeed * Time.deltaTime;

        ClampPosition();
    }

    public void ClampPosition()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    public void RotationLook(Vector2 movement)
    {
        aimTarget.parent.position = Vector3.zero;
        aimTarget.localPosition = new Vector3(movement.x / xShipNoseTiltDivisor, movement.y / yShipNoseTiltDivisor, 1);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(aimTarget.position), Mathf.Deg2Rad * lookSpeed * Time.deltaTime);
    }

    public void ZTilt(float axis, int tiltDir, int rollDir)
    {
        float targetZ = 0f;

        // base tilting
        targetZ = -axis * horizLeanLimit;

        // L/R Hard Tilt
        if (tiltDir != 0) targetZ = 90f * -tiltDir;

        currentZ = Mathf.LerpAngle(currentZ, targetZ, horizLeanTime);

        if (rollDir != 0 && rollCoroutine is null)
        {
            rollCoroutine = StartCoroutine(BarrelRoll(rollDir));
            //barrel.Play();

        }

        playerModel.localEulerAngles = new Vector3(playerModel.localEulerAngles.x,
        playerModel.localEulerAngles.y,
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(aimTarget.position, .5f);
        Gizmos.DrawSphere(aimTarget.position, .15f);

    }

    void SetSpeed(float x)
    {
        dolly.m_Speed = x;
    }

    void SetCameraZoom(float zoom, float duration)
    {
        cameraParent.DOLocalMove(new Vector3(0, 0, zoom), duration);
    }

    void DistortionAmount(float x)
    {
        Camera.main.GetComponent<PostProcessVolume>().profile.GetSetting<LensDistortion>().intensity.value = x;
    }

    void FieldOfView(float fov)
    {
        cameraParent.GetComponentInChildren<CinemachineVirtualCamera>().m_Lens.FieldOfView = fov;
    }

    void Chromatic(float x)
    {
        Camera.main.GetComponent<PostProcessVolume>().profile.GetSetting<ChromaticAberration>().intensity.value = x;
    }


}
