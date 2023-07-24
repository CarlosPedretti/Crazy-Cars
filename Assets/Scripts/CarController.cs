using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{

    //public Vector2 moveDirection;  // = Vector2.zero;
    private Vector2 moveDirection;  // = Vector2.zero;

    private Vector2 inputVector;  // = Vector2.zero;
    private float inputBraking;
    private float inputAcceleration;


    private PlayerInput playerInput;

    private float horizontalInput;
    private float verticalInput;
    private float currentSteerAngle;
    private float currentbreakForce;

    private float currentAcceleration;

    private bool isBreaking;
    private bool isAccelerating;

    [SerializeField] float downForce = 0;

    [SerializeField] private float maxForardSpeed;
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float forwardAcceleration;
    [SerializeField] private float stoppingAceleration;
    [SerializeField] private float maxSpeed;


    [SerializeField] private float motorForce;
    [SerializeField] private float brakeForce;
    [SerializeField] private float maxSteerAngle;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheeTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    private Rigidbody rb;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
    }

    public void SetInputVector(Vector2 direction)
    {
        inputVector = direction;
    }
    public void SetInputBrakes(float brakes)
    {
        inputBraking = brakes;
    }
    public void SetInputAcceleration(float acceleration)
    {
        inputAcceleration = acceleration;
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        ApplyBreaking();

        rb.AddRelativeForce(Vector3.down * downForce, ForceMode.Force);
    }


    public void GetInput()
    {
        //moveDirection = playerInput.actions["Movement"].ReadValue<Vector2>();
        //Debug.Log("Recibiendo Input:" + moveDirection);
        moveDirection = inputVector;
        //isBreaking = playerInput.actions["Brakes"].ReadValue<float>() == 1;
        //isAccelerating = playerInput.actions["Acceleration"].ReadValue<float>() == 1;
        isBreaking = inputBraking == 1.0f;
        isAccelerating = inputAcceleration == 1.0f;

    }

    private void HandleMotor()
    {
        currentAcceleration = isAccelerating ? forwardSpeed : 0f;



        rearLeftWheelCollider.motorTorque = moveDirection.y * currentAcceleration;
        rearRightWheelCollider.motorTorque = moveDirection.y * currentAcceleration;

        currentbreakForce = isBreaking ? brakeForce : 0f;
        if (isBreaking)
        {
            //Debug.Log("APPLY BREAKING()");
            ApplyBreaking();
        }

        //Forward
        if (moveDirection.y >= 0) 
        {
            if (forwardSpeed < maxForardSpeed) 
            {
                forwardSpeed += Time.deltaTime * forwardAcceleration; 
            }
            else 
            {
                forwardSpeed = maxForardSpeed;
            }
        }

        //Stopping
        if (moveDirection.y == 0) 
        {
            if (forwardSpeed > 0) 
            {
                forwardSpeed -= Time.deltaTime * stoppingAceleration;

                if (forwardSpeed < 0) 
                {
                    forwardSpeed = 0; 
                }
                Breaking();
            }
            else 
            {
                forwardSpeed = 0; 
            }
        }

        //Reverse
        if (moveDirection.y < 0) 
        {
            if (forwardSpeed < maxForardSpeed) 
            {
                forwardSpeed += Time.deltaTime * forwardAcceleration; 
            }
            else 
            {
                forwardSpeed = maxForardSpeed;
            }
        }

    }


    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;

    }

    private void Breaking()
    {
        frontRightWheelCollider.brakeTorque = brakeForce * 3;
        frontLeftWheelCollider.brakeTorque = brakeForce * 3;
        rearLeftWheelCollider.brakeTorque = brakeForce * 3;
        rearRightWheelCollider.brakeTorque = brakeForce * 3;

        //Debug.Log("Estoy frenando por medio de Breaking()");
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * moveDirection.x; //Horizontal input solo aca
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheeTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot
        ;wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}