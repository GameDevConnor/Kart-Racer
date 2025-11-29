using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
public class KartStateMachine : StateMachine<KartStateMachine.KartState>
{
    public float topSpeed;
    public Vector3 input;
    public float displacement;
    public float timeRate;
    public float deadzone;

    public float forwardDisplacement;
    public float backwardDisplacement;

    public float forwardInputTime = 0;
    public float backwardInputTime = 0;

    public float turnLimit;
    public float turnSpeed;
    public CharacterController characterController;
    public float gravity = 10f;

    public KartContext kartContext;

    public Vector2 inputs;

    public enum KartState
    {
        Idle,
        Forward,
        Backward,
        Falling
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        kartContext = new KartContext(characterController, topSpeed, input, displacement, timeRate, deadzone, forwardDisplacement, backwardDisplacement, forwardInputTime, backwardInputTime, turnLimit, turnSpeed, transform.forward);

        InitializeStates();
    }

    private void InitializeStates()
    {
        States.Add(KartState.Idle, new Idle(kartContext, KartState.Idle));
        States.Add(KartState.Forward, new Forward(kartContext, KartState.Forward));
        States.Add(KartState.Backward, new Backward(kartContext, KartState.Backward));
        States.Add(KartState.Falling, new Falling(kartContext, KartState.Falling));

        CurrentState = States[KartState.Falling];
    }

    public void GetInputValue(InputAction.CallbackContext inputActionCallbackContext)
    {
        inputs = inputActionCallbackContext.ReadValue<Vector2>();
    }
}
