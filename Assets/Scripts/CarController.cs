using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public enum Cr
    {
        Fronts,
        Rears
    }

    [Serializable]
    public struct Wheel
    {
        public GameObject wheelMesh;
        public WheelCollider wheelCollider;
        public Cr cr;
    }

    public float maxAcc = 10f;
    public float brakeAcc = 16.66f;

    public float turnSensitivity = 1.0f;

    public Vector3 _centerOfMass;

    public List<Wheel> wheels;

    float moveInput;
    float steerInput;

    private Rigidbody carRb;

    void Start()
    {
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = _centerOfMass;
    }

    void FixedUpdate()
    {
        GetInputs();
        AnimateWheels();
    }

    void LateUpdate()
    {
        Move();
        Steer();
    }

    void GetInputs()
    {
        moveInput = SimpleInput.GetAxis("Vertical");
        steerInput = SimpleInput.GetAxis("Horizontal");
    }

    void Move()
    {
        foreach(var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = moveInput * 295 * maxAcc * Time.deltaTime;
        }
    }

    void Steer()
    {
        foreach(var wheel in wheels)
        {
            if (wheel.cr == Cr.Fronts)
            {
                var _steerAngle = steerInput * turnSensitivity * 15f;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAngle, 0.6f);
            }
        }
    }
    public void Brake()
    {
        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.brakeTorque = 300 * brakeAcc * Time.deltaTime;
        }
    }

    public void BrakeRelease()
    {
        foreach(var wheel in wheels)
        {
            wheel.wheelCollider.brakeTorque = 0;
        }
    }

    void AnimateWheels()
    {
        foreach(var wheel in wheels)
        {
            Quaternion rot;
            Vector3 pos;
            wheel.wheelCollider.GetWorldPose(out pos, out rot);
            wheel.wheelMesh.transform.position = pos;
            wheel.wheelMesh.transform.rotation = rot;

        }
    }
}
