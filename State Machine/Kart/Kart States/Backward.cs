using Unity.VisualScripting;
using UnityEngine;

public class Backward : KartInteractionState
{

    public Backward(KartContext kartContext, KartStateMachine.KartState key) : base(kartContext, key)
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
                machine.transform.Rotate(0, context.TurnSpeed * Time.deltaTime, 0, Space.Self);
            }

            if (inputX < 0)
            {
                machine.transform.Rotate(0, -context.TurnSpeed * Time.deltaTime, 0, Space.Self);
            }
        }
        else if (inputZ < 0)
        {
            if (inputX > 0)
            {
                machine.transform.Rotate(0, -context.TurnSpeed * Time.deltaTime, 0, Space.Self);
            }

            if (inputX < 0)
            {
                machine.transform.Rotate(0, context.TurnSpeed * Time.deltaTime, 0, Space.Self);
            }
        }


        context.Input = machine.transform.forward.normalized * inputZ;

        context.CharacterController.Move(context.Input * Time.deltaTime);
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
