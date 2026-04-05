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
        cMovement.HorizontalLean(movementAxis.x);

        if (inputs.Arwing.Boost.WasPressedThisFrame()) cMovement.Boost(true);

        if (inputs.Arwing.Boost.WasReleasedThisFrame()) cMovement.Boost(false);

        if (inputs.Arwing.Break.WasPressedThisFrame()) cMovement.Break(true);

        if (inputs.Arwing.Break.WasReleasedThisFrame()) cMovement.Break(false);

        if (inputs.Arwing.Lazer.WasPressedThisFrame()) attack.Shoot();

        if (inputs.Arwing.Bomb.WasPressedThisFrame()) attack.Bomb();

        if (inputs.Arwing.LeftTilt.WasPressedThisFrame() || inputs.Arwing.RightTilt.WasPressedThisFrame())
        {
            int dir = inputs.Arwing.LeftTilt.WasPressedThisFrame() ? -1 : 1;
            cMovement.QuickSpin(dir);
        }

    }
}
