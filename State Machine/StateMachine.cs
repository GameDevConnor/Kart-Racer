using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class StateMachine<EState>: MonoBehaviour where EState : Enum
{
    // This class is abstract because from here we can extend an unlimited number of concrete classes from this one

    protected Dictionary<EState, BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>();

    public BaseState<EState> CurrentState;

    protected bool IsTransitioningState = false;

    void Start()
    {
        CurrentState.EnterState();
    }

    void Update()
    {
        EState nextStateKey = CurrentState.GetNextState();
        if (!IsTransitioningState && nextStateKey.Equals(CurrentState.StateKey))
        {
            CurrentState.UpdateState();
        }
        else if (!IsTransitioningState)
        {
            TransitionToState(nextStateKey);
        }
    }

    public void TransitionToState(EState key)
    {
        IsTransitioningState = true;
        CurrentState.ExitState();
        CurrentState = States[key];
        CurrentState.EnterState();
        IsTransitioningState = false;

    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        
    }
}
