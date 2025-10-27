using UnityEngine;

public class Control : MonoBehaviour
{
    public CharacterController characterController;
    public float topSpeed;
    public Vector3 input;
    public float displacement;
    public float timeRate;
    public float deadzone;

    public float forwardDisplacement;
    public float backwardDisplacement;

    public float forwardInputTime = 0;
    public float backwardInputTime = 0;

    public float turnLimit;
    public float turnSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        deadzone = Time.deltaTime + 0.01f;

        forwardInputTime = Mathf.Clamp(forwardInputTime, 0, 1);
        backwardInputTime = Mathf.Clamp(backwardInputTime, -1, 0);

        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        if (inputZ > 0)
        {
            forwardInputTime += Time.deltaTime * timeRate;
        }
        else
        {
            forwardInputTime -= Time.deltaTime * timeRate;

            if ((forwardInputTime < deadzone && forwardInputTime > 0) || (forwardInputTime > -deadzone && forwardInputTime < 0))
            {
                forwardInputTime = 0f;
            }
        }

        if (inputZ < 0)
        {
            backwardInputTime -= Time.deltaTime * timeRate;
        }
        else
        {
            backwardInputTime += Time.deltaTime * timeRate;

            if ((backwardInputTime < deadzone && backwardInputTime > 0) || (backwardInputTime > -deadzone && backwardInputTime < 0))
            {
                backwardInputTime = 0f;
            }
        }

        forwardDisplacement = forwardInputTime * topSpeed;
        backwardDisplacement = backwardInputTime * topSpeed;

        inputZ = forwardDisplacement + backwardDisplacement;

        input = new Vector3(inputX, 0f, inputZ);

        characterController.Move(input * Time.deltaTime);

    }
}
