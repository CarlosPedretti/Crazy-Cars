using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    public Vector2 moveDirection;
    private PlayerInput playerInput;


    //[SerializeField] GameObject centerOfMass;

    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    [SerializeField] private float reverseSpeed;
    [SerializeField] private float brakeForce;
    [SerializeField] private float maxSteerAngle;



    private bool isBreaking;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    private Rigidbody rb;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        // rb.centerOfMass = centerOfMass.transform.position;
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();

        Vector3 localAngularVelocity = transform.InverseTransformDirection(rb.angularVelocity);
        float antiRollForce = 1400; // Valor de fuerza contraria ajustable

        float antiRollTorque = localAngularVelocity.x * antiRollForce;
        rb.AddTorque(transform.up * -antiRollTorque);
    }

    private void GetInput()
    {
        moveDirection = playerInput.actions["Movement"].ReadValue<Vector2>();
    }

    private void HandleMotor()
    {
        float speed = rb.velocity.magnitude;


        if (moveDirection.y > 0)
        {

            //Debug.Log("Acelerando adelante");
            // Acelerar hacia adelante
            if (speed < maxSpeed)
            {
                //Debug.Log("Acelerando adelante");
                rb.AddForce(transform.forward * acceleration * moveDirection.y);
            }
        }
        else if (moveDirection.y < 0)
        {
            // Acelerar en reversa
            //Debug.Log("Acelerando atras");
            if (speed > reverseSpeed)
            {
                //Debug.Log("Acelerando atras");
                rb.AddForce(transform.forward * acceleration * moveDirection.y);
            }
        }
        else
        {
            // Desacelerar
            /*float brakeSpeed = speed > deceleration ? deceleration : speed;
            rb.AddForce(-rb.velocity.normalized * brakeSpeed);*/
    }

        isBreaking = playerInput.actions["Brakes"].ReadValue<float>() > 0.1f;
         ApplyBrakes();
    }

    private void ApplyBrakes()
    {
        float currentBreakForce = isBreaking ? brakeForce : 0f;
        frontRightWheelCollider.brakeTorque = currentBreakForce;
        frontLeftWheelCollider.brakeTorque = currentBreakForce;
        rearLeftWheelCollider.brakeTorque = currentBreakForce;
        rearRightWheelCollider.brakeTorque = currentBreakForce;
    }

    private void HandleSteering()
    {
        float currentSteerAngle = maxSteerAngle * moveDirection.x;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}






/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    public Vector2 moveDirection;
    private PlayerInput playerInput;

    [SerializeField] private float maxSpeed;
    [SerializeField] private float minSpeed;
    [SerializeField] private float reductionSpeed;
    [SerializeField] private float Speed;
    [SerializeField] private float reverseSpeed;

    [SerializeField] private float brakeForce;
    [SerializeField] private float maxSteerAngle;

    private float currentbreakForce;
    private bool isBreaking;



    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput()
    {
        moveDirection = playerInput.actions["Movement"].ReadValue<Vector2>();

        if (moveDirection.y == 0)
        {
            // Reduce la velocidad en la dirección actual.
            Vector3 currentVelocity = GetComponent<Rigidbody>().velocity;
            float currentSpeed = currentVelocity.magnitude;
            Vector3 currentDirection = currentVelocity.normalized;

            GetComponent<Rigidbody>().velocity = currentDirection * Mathf.Lerp(currentSpeed, 0f, Time.deltaTime * reductionSpeed);
        }
    }

    private void HandleMotor()
    {
        //Debug.Log("Velocidad: " + currentSpeed); 
        // Si la dirección de movimiento es hacia adelante (positiva) y la velocidad actual es menor que la velocidad máxima
        if (moveDirection.y > 0)
        {
            // Aplicar fuerza de aceleración
            frontLeftWheelCollider.motorTorque = 1 * Mathf.Abs(Speed);
            frontRightWheelCollider.motorTorque = 1 * Mathf.Abs(Speed);

            Debug.Log("W BIEN");

        }

        if (moveDirection.y < 0)
        {
            // Aplicar fuerza de aceleración en reversa
            frontLeftWheelCollider.motorTorque = -1 * Mathf.Abs(reverseSpeed);
            frontRightWheelCollider.motorTorque = -1 * Mathf.Abs(reverseSpeed);

            Debug.Log("S BIEN");

        }

        currentbreakForce = isBreaking ? brakeForce : 0f;
        ApplyBrakes();

        /* // Aplicar freno si se está presionando el botón de freno
         if (playerInput.actions["Brakes"].ReadValue<float>() > 0.1f)
         {
             ApplyBrakes(brakeForce);
         }
         else
         {
             ApplyBrakes(0f);
         }*/
/* }

 private void ApplyBrakes()
 {
     frontRightWheelCollider.brakeTorque = currentbreakForce;
     frontLeftWheelCollider.brakeTorque = currentbreakForce;
     rearLeftWheelCollider.brakeTorque = currentbreakForce;
     rearRightWheelCollider.brakeTorque = currentbreakForce;
 }

 private void HandleSteering()
 {
     float currentSteerAngle = maxSteerAngle * moveDirection.x;
     frontLeftWheelCollider.steerAngle = currentSteerAngle;
     frontRightWheelCollider.steerAngle = currentSteerAngle;
 }

 private void UpdateWheels()
 {
     UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
     UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
     UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
     UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
 }

 private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
 {
     Vector3 pos;
     Quaternion rot;
     wheelCollider.GetWorldPose(out pos, out rot);
     wheelTransform.rotation = rot;
     wheelTransform.position = pos;
 }
}

*/