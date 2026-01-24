using Unity.Cinemachine;
using UnityEngine;

public class Slide : KartInteractionState
{
    public float maxSlideSpeed;
    public Slide(KartContext kartContext, KartStateMachine.KartState key) : base(kartContext, key)
    {
        KartContext context = kartContext;
    }
    public override void EnterState()
    {
        context.entranceVelocity = context.exitVelocity;
        maxSlideSpeed = context.entranceVelocity.y;
    }

    public override KartStateMachine.KartState GetNextState()
    {
        return StateKey;
    }

    public override void OnTriggerEnter(Collider other)
    {

    }

    public override void OnTriggerExit(Collider other)
    {

    }

    public override void OnTriggerStay(Collider other)
    {

    }

    public override void UpdateState()
    {
        maxSlideSpeed += context.gravity * Time.deltaTime;

        context.playerAngle = Vector3.Angle(machine.transform.TransformDirection(Vector3.down), Vector3.down);

        if (context.playerAngle < context.slideThreshold)
        {
            machine.TransitionToState(KartStateMachine.KartState.Forward);
        }

        if (context.isGrounded == false)
        {
            machine.TransitionToState(KartStateMachine.KartState.Falling);
        }

        RaycastHit hit;
        RaycastHit hitGravity;
        if (Physics.Raycast(machine.transform.position, machine.transform.TransformDirection(Vector3.down), out hit, (machine.transform.localScale.y/* / 2 + 0.1f*/)))
        {
            context.isGrounded = true;
            Quaternion rotationXZ = Quaternion.FromToRotation(machine.transform.TransformDirection(Vector3.up), hit.normal);
            machine.transform.rotation = rotationXZ * machine.transform.rotation;

            context.forwardDirection = rotationXZ * context.forwardDirection;

            Vector3 slideDirection = Vector3.ProjectOnPlane(Vector3.down, hit.normal).normalized * maxSlideSpeed;

            context.CharacterController.Move(slideDirection * Time.deltaTime);
        }
        else 
        {
            if ((Physics.Raycast(machine.transform.position, Vector3.down, out hitGravity, (machine.transform.localScale.y/* / 2 + 0.1f*/)) && context.CharacterController.velocity.y <= 0f))
            {
                context.isGrounded = true;
                Quaternion rotationXZ = Quaternion.FromToRotation(Vector3.up, hitGravity.normal);
                machine.transform.rotation = rotationXZ;

                context.forwardDirection = rotationXZ * context.forwardDirection;
            }
            else
            {
                context.isGrounded = false;

                RaycastHit hitInfinity;
                if ((Physics.Raycast(machine.transform.position, Vector3.down, out hitInfinity, Mathf.Infinity)))
                {
                    Quaternion rotationXZ = Quaternion.FromToRotation(Vector3.up, hitInfinity.normal);
                    machine.transform.rotation = rotationXZ;

                    context.forwardDirection = machine.transform.TransformDirection(Vector3.forward);
                    context.forwardDirection = rotationXZ * context.forwardDirection;
                }
            }
        }
    }
}
