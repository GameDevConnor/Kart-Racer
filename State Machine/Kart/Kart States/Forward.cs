using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;
using Input = UnityEngine.Input;
using UnityEngine.InputSystem;
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
        isGrounded = context.CharacterController.isGrounded;

        if (!isGrounded)
        {
            machine.TransitionToState(KartStateMachine.KartState.Falling);
        }
        else
        {
            context.Deadzone = Time.deltaTime + 0.01f;

            context.ForwardInputTime = Mathf.Clamp(context.ForwardInputTime, 0, 1);
            context.BackwardInputTime = Mathf.Clamp(context.BackwardInputTime, -1, 0);

            float inputX = Input.GetAxis("Horizontal");
            float inputZ = Input.GetAxis("Vertical");

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

            context.Input = context.forwardDirection.normalized * inputZ;

            Vector3 movement;

            if (Mathf.Abs(context.CharacterController.velocity.y) > 0.001f)
            {
                movement = new Vector3(context.Input.x, context.CharacterController.velocity.y, context.Input.z);
            }
            else
            {
                movement = new Vector3(context.Input.x, -context.gravity, context.Input.z);
            }

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
