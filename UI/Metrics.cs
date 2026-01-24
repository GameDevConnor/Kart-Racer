using UnityEngine;
using TMPro;

public class Metrics : MonoBehaviour
{
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI angleText;
    public KartContext kartContext;
    public TextMeshProUGUI thresholdText;
    public TextMeshProUGUI stateText;
    public TextMeshProUGUI inputText;
    public TextMeshProUGUI hitNormalText;
    public TextMeshProUGUI upText;
    public TextMeshProUGUI downwardHitNormalText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        kartContext = GetComponent<KartStateMachine>().kartContext;
    }

    // Update is called once per frame
    void Update()
    {
        speedText.text = "Speed: " + kartContext.CharacterController.velocity + ", " + kartContext.CharacterController.velocity.magnitude;
        angleText.text = "Angle: " + kartContext.playerAngle;
        thresholdText.text = "Threshold: " + kartContext.VelocityThresholdForAngle(kartContext.playerAngle);
        stateText.text = "State: " + GetComponent<KartStateMachine>().CurrentState;
        inputText.text = "Input " + kartContext.input + ", " + kartContext.input.magnitude;
        hitNormalText.text = "Hit Normal: " + kartContext.angleRotation;
        upText.text = "Up: " + transform.TransformDirection(Vector3.up);
        downwardHitNormalText.text = "Downward Hit Normal: " + kartContext.downwardHitNormal;
    }
}
