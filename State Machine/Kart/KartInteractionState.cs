using UnityEngine;

public abstract class KartInteractionState : BaseState<KartStateMachine.KartState>
{
    protected KartContext context;

    protected KartStateMachine machine = GameObject.FindFirstObjectByType<KartStateMachine>();

    public bool isGrounded;

    public float xRotation;
    public float yRotation;
    public float zRotation;

    public KartInteractionState(KartContext context, KartStateMachine.KartState stateKey): base(stateKey)
    {
        this.context = context;
    }

    public override void ExitState()
    {
        context.exitVelocity = context.CharacterController.velocity;
    }
}
