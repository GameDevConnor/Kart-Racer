using JetBrains.Annotations;
using NUnit.Framework.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.Windows;
using Input = UnityEngine.Input;
public class Forward : KartInteractionState
{

    public Forward(KartContext kartContext, KartStateMachine.KartState key) : base(kartContext, key)
    {
        KartContext context = kartContext;
    }

    public override void EnterState()
    {
        context.TurnSpeed = 30f;
        context.TimeRate = 1f;
        context.TopSpeed = 30f;

        context.entranceVelocity = context.exitVelocity;

        //Debug.Log(Quaternion.FromToRotation(new Vector3(0f,0.16f,-0.99f), new Vector3(0,-0.02f,-1f)).eulerAngles);
        //Debug.Log(Vector3.Angle(new Vector3(0f, 0.16f, -0.99f), new Vector3(0, -0.02f, -1f)));

    }

    //public override void ExitState()
    //{

    //}

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

        context.playerAngle = Vector3.Angle(machine.transform.TransformDirection(Vector3.down), Vector3.down);

        RaycastHit downwardHit;
        if (Physics.Raycast(machine.transform.position, Vector3.down, out downwardHit, Mathf.Infinity))
        {
            context.downwardHitNormal = downwardHit.normal;
        }


            if (context.forwardDirection.y > 0)
        {
            context.uphill = true;
            context.downhill = false;
        }
        else if (context.forwardDirection.y < 0)
        {
            context.uphill = false;
            context.downhill = true;
        }
        else
        {
            context.uphill = false;
            context.downhill = false;
        }

        if (context.isGrounded == false)
        {
            machine.TransitionToState(KartStateMachine.KartState.Falling);
        }
        else
        {
            context.Deadzone = Time.deltaTime + 0.01f;

            context.ForwardInputTime = Mathf.Clamp(context.ForwardInputTime, 0, 1);
            context.BackwardInputTime = Mathf.Clamp(context.BackwardInputTime, -1, 0);

            float inputX = machine.inputs.x;
            float inputZ = machine.inputs.y;

            if (inputZ > 0)
            {
                context.ForwardInputTime += Time.deltaTime * context.TimeRate;
            }
            else
            {
                context.ForwardInputTime -= Time.deltaTime * context.TimeRate;

                if ((context.ForwardInputTime < context.Deadzone && context.ForwardInputTime > 0) || (context.ForwardInputTime > -context.Deadzone && context.ForwardInputTime < 0))
                {
                    context.ForwardInputTime = 0f;
                }
            }

            if (inputZ < 0)
            {
                context.BackwardInputTime -= Time.deltaTime * context.TimeRate;
            }
            else
            {
                context.BackwardInputTime += Time.deltaTime * context.TimeRate;

                if ((context.BackwardInputTime < context.Deadzone && context.BackwardInputTime > 0) || (context.BackwardInputTime > -context.Deadzone && context.BackwardInputTime < 0))
                {
                    context.BackwardInputTime = 0f;
                }
            }

            context.ForwardDisplacement = context.ForwardInputTime * context.TopSpeed;
            context.BackwardDisplacement = context.BackwardInputTime * context.TopSpeed;

            inputZ = context.ForwardDisplacement + context.BackwardDisplacement;

            if (inputZ > 0)
            {
                if (inputX > 0)
                {
                    context.forwardDirection = Quaternion.AngleAxis(context.TurnSpeed * Time.deltaTime, Vector3.up) * context.forwardDirection;
                    machine.transform.Rotate(0, context.TurnSpeed * Time.deltaTime, 0);
                }

                if (inputX < 0)
                {
                    context.forwardDirection = Quaternion.AngleAxis(-context.TurnSpeed * Time.deltaTime, Vector3.up) * context.forwardDirection;
                    machine.transform.Rotate(0, -context.TurnSpeed * Time.deltaTime, 0);
                }
            }
            else if (inputZ < 0)
            {
                if (inputX > 0)
                {
                    context.forwardDirection = Quaternion.AngleAxis(-context.TurnSpeed * Time.deltaTime, Vector3.up) * context.forwardDirection;
                    machine.transform.Rotate(0, -context.TurnSpeed * Time.deltaTime, 0);
                }

                if (inputX < 0)
                {
                    context.forwardDirection = Quaternion.AngleAxis(context.TurnSpeed * Time.deltaTime, Vector3.up) * context.forwardDirection;
                    machine.transform.Rotate(0, context.TurnSpeed * Time.deltaTime, 0);
                }
            }


            RaycastHit hit;
            RaycastHit hitGravity;
            if (Physics.Raycast(machine.transform.position, machine.transform.TransformDirection(Vector3.down), out hit, (machine.transform.localScale.y * Mathf.Sqrt(2))))
            {
                context.isGrounded = true;
                Quaternion rotationXZ = Quaternion.FromToRotation(machine.transform.TransformDirection(Vector3.up), hit.normal);

                context.angleRotation = hit.normal;

                machine.transform.rotation = rotationXZ * machine.transform.rotation;
                context.forwardDirection = machine.transform.forward;
            
            }
            else
            {
                if ((Physics.Raycast(machine.transform.position, Vector3.down, out hitGravity, (machine.transform.localScale.y * Mathf.Sqrt(2))) && context.CharacterController.velocity.y <= 0f))
                {
                    context.groundAngle = Vector3.Angle(hitGravity.normal, Vector3.down);

                    context.isGrounded = true;
                    Quaternion rotationXZ = Quaternion.FromToRotation(machine.transform.TransformDirection(Vector3.up), hitGravity.normal);
                    
                    machine.transform.rotation = rotationXZ * machine.transform.rotation;
                    context.forwardDirection = rotationXZ * context.forwardDirection;

                    machine.transform.forward = context.forwardDirection;
                }
                else
                {
                    context.isGrounded = false;
                }
            }

            if ((context.playerAngle > context.slideThreshold) && (context.Input.magnitude < context.VelocityThresholdForAngle(context.playerAngle)))
            {
                machine.TransitionToState(KartStateMachine.KartState.Slide);
            }

            if ((context.playerAngle > context.fallThreshold) && (context.Input.magnitude < context.VelocityThresholdForAngle(context.playerAngle)))
            {
                context.isGrounded = false;
            }

            context.Input = context.forwardDirection.normalized * inputZ;

            Vector3 movement = new Vector3(context.Input.x, context.Input.y, context.Input.z);

            context.CharacterController.Move(movement * Time.deltaTime);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
