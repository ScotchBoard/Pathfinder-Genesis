using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBehaviour: MonoBehaviour
{
    // Private
    private float localTimeScale = 1.0f; // the lower it is, the slower the objects inside the force field moves
    private Rigidbody rigidBody;

    private float drag, angularDrag, mass;
    private Vector3 angularVelocity, velocity;

    private GameObject forceField;

    // Public
    public bool IsSlowed { get; private set; }

    private void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        GravityCounter();
    }

    private void Update()
    {
        if(forceField == null && IsSlowed)
        {
            RestoreData();
        }
    }

    public void TimeController(float value)
    {
        float multiplier = value / localTimeScale;

        rigidBody.drag *= multiplier;
        rigidBody.angularDrag *= multiplier;
        rigidBody.angularVelocity *= multiplier;
        rigidBody.velocity *= multiplier;
        rigidBody.mass /= multiplier;

        localTimeScale = value;
    }

    public void SaveData()
    {
        drag = rigidBody.drag;
        angularDrag = rigidBody.angularDrag;
        angularVelocity = rigidBody.angularVelocity;
        velocity = rigidBody.velocity;
        mass = rigidBody.mass;
    }

    public void RestoreData()
    {
        rigidBody.drag = drag;
        rigidBody.angularDrag = angularDrag;
        rigidBody.angularVelocity = angularVelocity;
        rigidBody.velocity = velocity;
        rigidBody.mass = mass;

        IsSlowed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Force Field")
        {
            forceField = other.gameObject;
            IsSlowed = true;
        }
    }

    private void GravityCounter()
    {
        rigidBody.AddForce(-Physics.gravity + (Physics.gravity * Mathf.Pow(localTimeScale, 2)), ForceMode.Acceleration);
    }
}
