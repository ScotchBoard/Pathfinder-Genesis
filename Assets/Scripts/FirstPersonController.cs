using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    #region Variables
    // Movement
    private float forwardSpeed;
    private float sideSpeed;
    private float upwardsSpeed; // Speed for moving forward, backwards, left and right
    private float verticalVelocity = 0.0f;
    private float movementSpeed;
    [Header("Movement")]
    [SerializeField]
    private float normalSpeed = 10.0f;    // Movement speed in those directions
    [SerializeField]
    private float sprintSpeed = 600f;
    [SerializeField]
    private float jumpSpeed = 10.0f;

    // Dashing
    private bool canDash = true;
    [Header("Dash")]
    [SerializeField]
    private float maxDashTime = 2.0f;
    [SerializeField]
    private float dashEnergyConsumption = 10f;

    // Rotation
    private float rotLeftRight; // Camera rotation speed
    private float verticalRotation = 0.0f;   // Used for camera degrees rotation
    [Header("Rotation")]
    [SerializeField]
    private float mouseSensitivity = 2.5f; 
    [SerializeField]
    private float upDownRange = 60.0f;     // The range of how many degrees the camera can move

    // CharacterController
    private CharacterController characterController;
    private Vector3 speed;

    // Dieing animation
    private float totalRotation = 0;
    [Header("Dieing animation")]
    [SerializeField]
    private float rotationDegreesPerSecond = 45f;
    [SerializeField]
    private float rotationDegreesAmount = 60f;

    private PlayerInfo playerInfo;
    #endregion

    void Start ()
    {
        Cursor.visible = false;
        characterController = GetComponent<CharacterController>();
        playerInfo = GetComponent<PlayerInfo>();
    }

	void Update ()
    {
        if (!GameManager.INSTANCE.GameOver && (transform.rotation.x >= -1 && transform.rotation.x <= 1))
        {
            Movement();
            Rotation();
            totalRotation = 0;
        }
        else
        {
            Die();
        }
	}

    // Mouse
    private void Rotation()
    {
        rotLeftRight = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, rotLeftRight, 0);

        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        
    }

    // Changes the movement speed depending if the player is sprinting or not
    private void ChooseSpeed()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            movementSpeed = sprintSpeed;

            StartCoroutine(Dash(maxDashTime));
        }
        else
        {
            movementSpeed = normalSpeed;
        }
    }

    IEnumerator Dash(float cooldown)
    {
        canDash = false;
        playerInfo.SetPlayerEnergy(dashEnergyConsumption);
        yield return new WaitForSeconds(cooldown);
        if (playerInfo.CanUseEnergyDash())
        {
            canDash = true;
        }
    }

    // Keyboard
    private void Movement()
    {
        ChooseSpeed();

        forwardSpeed = Input.GetAxis("Vertical") * movementSpeed;
        sideSpeed = Input.GetAxis("Horizontal") * movementSpeed;
        // TODO jump dash
        if (characterController.isGrounded && Input.GetKey(KeyCode.Space))
        {
            if (movementSpeed == sprintSpeed)
            {
                verticalVelocity = sprintSpeed / 80;//jumpSpeed * (sprintSpeed / 10);
            }
            else
            {
                verticalVelocity = jumpSpeed;
            }
        }

        if (verticalVelocity >= -5.0f)
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime; // Increase the speed you are being pulled by. Just gravity stuff
        }

        speed = new Vector3(sideSpeed, verticalVelocity, forwardSpeed);
        speed = transform.rotation * speed;

        characterController.Move(speed * Time.deltaTime);
    }

    public void Die()
    {  
        if (Mathf.Abs(totalRotation) < Mathf.Abs(rotationDegreesAmount))
        {
            float currentAngleX = transform.rotation.eulerAngles.x;
            transform.rotation = Quaternion.AngleAxis(currentAngleX + (Time.deltaTime * rotationDegreesPerSecond), Vector3.right);
            totalRotation += Time.deltaTime * rotationDegreesPerSecond;
        }
    }
}
