using Unity.VisualScripting;
using UnityEngine;

public class KartContext
{

    // This is where I put shared values. AKA, things I want to access in every class

    public Vector3 entranceVelocity;
    public Vector3 exitVelocity;

    private CharacterController characterController;
    private float topSpeed;
    public Vector3 input;
    private float displacement;
    private float timeRate;
    private float deadzone;

    private float forwardDisplacement;
    private float backwardDisplacement;
    private float leftDisplacement;
    private float rightDisplacement;

    private float forwardInputTime = 0;
    private float backwardInputTime = 0;
    private float leftTotal = 0;
    private float rightTotal = 0;

    private float turnLimit;
    private float turnSpeed;

    public float gravity = 10f;

    public float airResistance;
    public float minimumAirSpeed;

    public bool isGrounded;

    public bool gravityThreshold;

    public bool uphill;
    public bool downhill;
    public float slideThreshold = 45f;
    public float fallThreshold = 90f;
    public float playerAngle;
    public float groundAngle;
    public Vector3 downwardHitNormal;

    public CharacterController CharacterController { get => characterController; set => characterController = value; }
    public float TopSpeed { get => topSpeed; set => topSpeed = value; }
    public Vector3 Input { get => input; set => input = value; }
    public float Displacement { get => displacement; set => displacement = value; }
    public float TimeRate { get => timeRate; set => timeRate = value; }
    public float Deadzone { get => deadzone; set => deadzone = value; }
    public float ForwardDisplacement { get => forwardDisplacement; set => forwardDisplacement = value; }
    public float BackwardDisplacement { get => backwardDisplacement; set => backwardDisplacement = value; }
    public float LeftDisplacement { get => leftDisplacement; set => leftDisplacement = value; }
    public float RightDisplacement { get => rightDisplacement; set => rightDisplacement = value; }
    public float ForwardInputTime { get => forwardInputTime; set => forwardInputTime = value; }
    public float BackwardInputTime { get => backwardInputTime; set => backwardInputTime = value; }
    public float LeftTotal { get => leftTotal; set => leftTotal = value; }
    public float RightTotal { get => rightTotal; set => rightTotal = value; }
    public float TurnLimit { get => turnLimit; set => turnLimit = value; }
    public float TurnSpeed { get => turnSpeed; set => turnSpeed = value; }

    public Vector3 forwardDirection;
    public Quaternion kartRotation;

    public Vector3 angleRotation;

    public KartContext(CharacterController characterController, float topSpeed, Vector3 input, float displacement, float timeRate, float deadzone, float forwardDisplacement, float backwardDisplacement, float forwardInputTime, float backwardInputTime, float turnLimit, float turnSpeed, Vector3 forwardDirection)
    {
        this.CharacterController = characterController;
        this.TopSpeed = topSpeed;
        this.Input = input;
        this.Displacement = displacement;
        this.TimeRate = timeRate;
        this.Deadzone = deadzone;
        this.ForwardDisplacement = forwardDisplacement;
        this.BackwardDisplacement = backwardDisplacement;
        this.ForwardInputTime = forwardInputTime;
        this.BackwardInputTime = backwardInputTime;
        this.TurnLimit = turnLimit;
        this.TurnSpeed = turnSpeed;
        this.forwardDirection = forwardDirection;
        this.kartRotation = Quaternion.identity;
    }


    public float VelocityThresholdForAngle(float angle)
    {
        return ((1.0f/5.0f) * angle) - (topSpeed / 2.0f);
        //return 0f;
    }

    //public float VelocityMultiplierForAngle(float angle)
    //{
        
    //}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
