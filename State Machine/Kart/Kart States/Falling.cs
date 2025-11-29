using UnityEngine;

public class Falling : KartInteractionState
{

    float fallSpeed;
    Vector3 opposite;

    public Falling(KartContext kartContext, KartStateMachine.KartState key) : base(kartContext, key)
    {
        KartContext context = kartContext;
    }

    public override void EnterState()
    {
        Debug.Log("Falling");

        context.entranceVelocity = context.exitVelocity;

        fallSpeed = context.entranceVelocity.y;

        context.TurnSpeed = 30f;
        context.TimeRate = 1f;
        context.TopSpeed = 30f;
        context.airResistance = 5f;

        opposite = new Vector3(-context.entranceVelocity.x, 0, -context.entranceVelocity.z).normalized;
        context.minimumAirSpeed = (context.TopSpeed) * (2 / 3);
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

        context.entranceVelocity.x = Mathf.Clamp(context.entranceVelocity.x, context.minimumAirSpeed, context.TopSpeed);
        context.entranceVelocity.z = Mathf.Clamp(context.entranceVelocity.z, context.minimumAirSpeed, context.TopSpeed);

        if (isGrounded)
        {
            machine.TransitionToState(KartStateMachine.KartState.Forward);
        }
        else
        {
            fallSpeed -= context.gravity * Time.deltaTime;

            context.entranceVelocity += (opposite * context.airResistance) * Time.deltaTime;

            context.Input = new Vector3(context.entranceVelocity.x, fallSpeed, context.entranceVelocity.z);

            context.CharacterController.Move(context.Input * Time.deltaTime);

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
