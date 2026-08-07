using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Inputs : MonoBehaviour
{
    public PlayerInputs inputs;
    public Attacking attack;
    public Corridor cMovement;

    [SerializeField] private bool boosting;
    [SerializeField] private bool breaking;

    [SerializeField] private bool invertY = true;
    private Vector2 movementAxis;


    void Start()
    {
        inputs = new PlayerInputs();
        inputs.Enable();
    }

    void Update()
    {
        movementAxis = inputs.Arwing.Move.ReadValue<Vector2>();
        if (invertY) movementAxis.y = movementAxis.y * -1;

        cMovement.LocalMove(movementAxis);
        cMovement.RotationLook(movementAxis);

        if (!breaking) 
        {
            if (inputs.Arwing.Boost.WasPressedThisFrame()) 
            {
                cMovement.Boost(true);
                boosting = true;
            }
            if (inputs.Arwing.Boost.WasReleasedThisFrame())
            {
                cMovement.Boost(false); 
                boosting = false;
            }
        }

        if (!boosting)
        {
            if (inputs.Arwing.Break.WasPressedThisFrame())
            {
                cMovement.Break(true);
                breaking = true;
            }
            if (inputs.Arwing.Break.WasReleasedThisFrame())
            {
                cMovement.Break(false);
                breaking = false;
            }
        }

        if (inputs.Arwing.Lazer.WasPressedThisFrame()) attack.Shoot();
        if (inputs.Arwing.Bomb.WasPressedThisFrame()) attack.Bomb();


        TiltCheck(movementAxis.x);
    }

    public void TiltCheck(float xAxis)
    {
        int rollDir = 0;
        int tiltDir = 0;
        tiltDir = inputs.Arwing.LeftTilt.IsPressed() ? -1 : 0;
        tiltDir += inputs.Arwing.RightTilt.IsPressed() ? 1 : 0;

        if (inputs.Arwing.LeftRoll.WasPerformedThisFrame() || inputs.Arwing.RightRoll.WasPerformedThisFrame())
        {
            rollDir = inputs.Arwing.LeftRoll.WasPerformedThisFrame() ? -1 : 1;
        }

        cMovement.ZTilt(xAxis, tiltDir, rollDir);
    }
}
