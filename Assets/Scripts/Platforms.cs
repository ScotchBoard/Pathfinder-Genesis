using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platforms : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private bool xAxis;
    [SerializeField]
    private bool yAxis;
    [SerializeField]
    private bool zAxis;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float floatingDistance;

    private Rigidbody rigidBody;
    private Vector3 startPosition, offset;
    private float startSpeed;
    private bool changeDirection = false;

    #endregion

    private void Start()
    {
        startPosition = gameObject.transform.position;
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        FloatAround();
    }

    private void FloatAround()
    {
        offset = gameObject.transform.position - startPosition;

        if (offset.magnitude <= floatingDistance && !changeDirection)
        {
            rigidBody.velocity = new Vector3(xAxis ? speed : 0, yAxis ? speed : 0, zAxis ? speed : 0);
        }
        else
        {
            changeDirection = true;    
        }
        

        if(changeDirection)
        {
            if(offset.magnitude >= 0.5f)
            {
                rigidBody.velocity = new Vector3(xAxis ? -speed : 0, yAxis ? -speed : 0, zAxis ? -speed : 0);
            }
            else
            {
                changeDirection = false;
            }
        }
    }
}
