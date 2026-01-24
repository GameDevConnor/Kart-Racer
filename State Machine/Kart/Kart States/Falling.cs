using UnityEngine;

public class Falling : KartInteractionState
{

    float fallSpeed;
    Vector3 opposite;

    float minimumXSpeed;
    float minimumZSpeed;

    public Falling(KartContext kartContext, KartStateMachine.KartState key) : base(kartContext, key)
    {
        KartContext context = kartContext;
    }

    public override void EnterState()
    {
        context.entranceVelocity = context.exitVelocity;

        minimumXSpeed = context.entranceVelocity.x * (1.0f / 2.0f);
        minimumZSpeed = context.entranceVelocity.z * (1.0f / 2.0f);

        fallSpeed = context.entranceVelocity.y;

        context.TurnSpeed = 30f;
        context.TimeRate = 1f;
        context.TopSpeed = 30f;
        context.airResistance = 5f;

        opposite = new Vector3(-context.entranceVelocity.x, 0, -context.entranceVelocity.z).normalized;

        context.minimumAirSpeed = (context.TopSpeed) * (2.0f / 3.0f);
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
        RaycastHit hit;
        RaycastHit hitGravity;
        if (Physics.Raycast(machine.transform.position, machine.transform.TransformDirection(Vector3.down), out hit, (machine.transform.localScale.y * Mathf.Sqrt(2))) && !((context.playerAngle > context.slideThreshold) && (context.Input.magnitude < context.VelocityThresholdForAngle(context.playerAngle))))
        {
            context.isGrounded = true;
        }
        else
        {
            if ((Physics.Raycast(machine.transform.position, Vector3.down, out hitGravity, (machine.transform.localScale.y * Mathf.Sqrt(2))) && context.CharacterController.velocity.y <= 0f))
            {
                context.isGrounded = true;
                Quaternion rotationXZ = Quaternion.FromToRotation(Vector3.up, hitGravity.normal);
                machine.transform.rotation = rotationXZ * machine.transform.rotation;

                context.forwardDirection = rotationXZ * context.forwardDirection;
            }
            else
            {
                context.isGrounded = false;
            }
        }

        if (context.isGrounded)
        {
            machine.TransitionToState(KartStateMachine.KartState.Forward);
        }
        else
        {

            if (context.CharacterController.velocity.y == 0)
            {
                RaycastHit hitInfinity;
                if ((Physics.Raycast(machine.transform.position, Vector3.down, out hitInfinity, Mathf.Infinity)))
                {
                    Quaternion rotationXZ = Quaternion.FromToRotation(Vector3.up, hitInfinity.normal);
                    machine.transform.rotation = rotationXZ * machine.transform.rotation;

                    context.forwardDirection = rotationXZ * context.forwardDirection;
                }
            }


            fallSpeed -= context.gravity * Time.deltaTime;

            if (context.entranceVelocity.z > minimumZSpeed)
            {
                context.entranceVelocity.z += opposite.z * context.airResistance * Time.deltaTime;
            }

            if (context.entranceVelocity.x > minimumXSpeed)
            {
                context.entranceVelocity.x += opposite.x * context.airResistance * Time.deltaTime;
            }

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
