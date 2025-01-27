using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public Joystick joystick;
    public float accelerationTime = 2.0f;
    public float rotationAccelerationTime = 1.0f;
    public float maxSpeed = 5.0f;
    public float maxRotationSpeed = 200.0f;
    public Transform cameraTransform;
    public float speedExponent = 1;
    public Rigidbody rigidBody;

    [SerializeField] private float currentSpeed = 0.0f;
    private float currentRotationSpeed = 0.0f;
    private float accelerationRate;
    private float rotationAccelerationRate;

    [SerializeField]
    private float slowDownRate = 5.0f; 


    private Vector3 direction;

    public bool inputEnabled = true;

    public float moveRotationXAngle = 20;

    private void Awake()
    {
        accelerationRate = (maxSpeed * speedExponent) / accelerationTime;
        rotationAccelerationRate = maxRotationSpeed / rotationAccelerationTime;
        rigidBody = GetComponent<Rigidbody>();
    }

    private IEnumerator Start()
    {
        yield return new WaitForFixedUpdate();
        if (TryGetComponent(out Rigidbody rigidbody))
            rigidbody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }

    private void Update()
    {
        if (!inputEnabled)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            ResetRigidbody();
        }

        Vector3 forward = cameraTransform.forward;
        forward.y = 0;
        forward.Normalize();
        Vector3 right = cameraTransform.right;
        right.y = 0;
        right.Normalize();

        Vector2 input = new Vector2(joystick.Horizontal, joystick.Vertical);
        if (input.magnitude == 0)
        {
            input.x = Input.GetAxis("Horizontal");
            input.y = Input.GetAxis("Vertical");
        }

        direction = (forward * input.y + right * input.x).normalized;
        
        float rawInputMagnitude =
            Mathf.Sqrt(input.y * input.y + input.x * input.x);
        Movement(rawInputMagnitude);
        Rotation(rawInputMagnitude);

    }

    public void Movement(float rawInputMagnitude)
    {

        float targetSpeed = (maxSpeed * speedExponent) * Mathf.Clamp(rawInputMagnitude, 0, 1);
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, accelerationRate * Time.deltaTime);

        Vector3 move = direction * currentSpeed * Time.deltaTime;
        transform.Translate(move, Space.World);
    }

    public void Rotation(float rawInput)
    {
        float targetRotationSpeed = maxRotationSpeed * Mathf.Clamp(rawInput, 0, 1);
        currentRotationSpeed = Mathf.MoveTowards(currentRotationSpeed, targetRotationSpeed,
            rotationAccelerationRate * Time.deltaTime);
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
                currentRotationSpeed * Time.deltaTime);
        }

        float xRotationAngle = Mathf.Lerp(0, moveRotationXAngle, currentSpeed / (maxSpeed * speedExponent));
        // transform.localRotation = Quaternion.Euler(xRotationAngle,
        //     transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
        transform.localRotation = Quaternion.Euler(0,
            transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
    }

    private void ResetRigidbody()
    {
        if (rigidBody == null)
        {
            rigidBody = GetComponent<Rigidbody>();
            return;
        }

        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
    }

    [Button]
    public void EnableInput()
    {
        ResetRigidbody();
        inputEnabled = true;
    }
}