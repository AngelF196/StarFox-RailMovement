//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using DG.Tweening;
//using Cinemachine;
//using UnityEngine.Rendering.PostProcessing;
//using UnityEngine.InputSystem;

//public class Corridor : MonoBehaviour
//{
//    private Transform playerModel;
//    private Inputs input;
//    private float rollOffset = 0;
//    private float currentZ;
//    private Coroutine rollCoroutine;

//    [Header("Parameters")]
//    public float xSpeed = 38;
//    public float ySpeed = 38;
//    public float xySpeed = 38;
//    public float lookSpeed = 340;
//    public float forwardSpeed = 6;
//    public float horizLeanLimit = 80;
//    public float horizLeanTime = 0.1f;
//    public float xShipNoseTiltDivisor = 2;
//    public float yShipNoseTiltDivisor = 2;

//    [Space]

//    [Header("Public References")]
//    public Transform aimTarget;
//    public CinemachineDollyCart dolly;
//    public Transform cameraParent;

//    private ShipActions shipActions;

//    void Start()
//    {
//        playerModel = transform.GetChild(0);
//        input = GetComponent<Inputs>();
//        SetSpeed(forwardSpeed);
//    }

//    private void OnEnable()
//    {
//        shipActions.leftStickRead.AddListener(LeftStick);
//        shipActions.rightStickRead.AddListener(RightStick);

//    }

//    private void OnDisable()
//    {
//        shipActions.leftStickRead.RemoveListener(LeftStick);
//        shipActions.rightStickRead.RemoveListener(RightStick);
//    }

//    private void LeftStick(Vector2 leftStick)
//    {

//    }

//    private void RightStick(Vector2 rightStick)
//    {

//    }

//    public void LocalMove(Vector2 inputaxis)
//    {
//        Vector3 camForward = cameraParent.forward;
//        // Project forward onto camera plane
//        Vector3 targetTransform = Vector3.ProjectOnPlane(transform.forward, camForward);
//        transform.position += targetTransform * xySpeed * Time.deltaTime;
//        ClampPosition();
//    }

//    public void ClampPosition()
//    {
//        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
//        pos.x = Mathf.Clamp01(pos.x);
//        pos.y = Mathf.Clamp01(pos.y);
//        transform.position = Camera.main.ViewportToWorldPoint(pos);
//    }

//    public void RotationLook(Vector2 movement)
//    {
//        aimTarget.parent.position = Vector3.zero;
//        aimTarget.localPosition = new Vector3(movement.x/xShipNoseTiltDivisor, movement.y/yShipNoseTiltDivisor, 1);
//        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(aimTarget.position), Mathf.Deg2Rad * lookSpeed * Time.deltaTime);
//    }

//    private void OnDrawGizmos()
//    {
//        Gizmos.color = Color.blue;
//        Gizmos.DrawWireSphere(aimTarget.position, .5f);
//        Gizmos.DrawSphere(aimTarget.position, .15f);
//    }

//    void SetSpeed(float x)
//    {
//        dolly.m_Speed = x;
//    }
//}
