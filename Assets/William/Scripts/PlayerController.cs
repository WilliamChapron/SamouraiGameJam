using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Public Variables
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;

    public float curSpeed = 0f;

    public float rotationSpeed = 700f;
    public Transform cameraTransform;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;
    #endregion

    #region Private Variables
    private CharacterController characterController;
    private float rotationVelocity;
    private Vector3 currentVelocity; 
    private Vector3 velocityXZ; 
    private float velocityY; 
    #endregion

    private bool isGrounded;

    #region Initialization
    void Start()
    {
        curSpeed = walkSpeed;
        characterController = GetComponent<CharacterController>();
    }
    #endregion

    #region Update Method
    void Update()
    {
        isGrounded = characterController.isGrounded;

        HandleMovement();
        ApplyGravity();

        // Jump
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity); 
            Debug.Log("Jumping! VelocityY: " + velocityY); 
        }

        currentVelocity.x = velocityXZ.x;
        currentVelocity.z = velocityXZ.z;
        currentVelocity.y = velocityY;

        characterController.Move(currentVelocity * Time.deltaTime);
    }
    #endregion

    #region Movement Handling
    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized;

        if (movement.magnitude >= 0.1f)
        {
            HandleRotation(movement);
            HandleTranslation(movement);
        }
        else
        {
            velocityXZ = Vector3.zero;
        }
    }
    #endregion

    #region Rotation Handling
    private void HandleRotation(Vector3 movement)
    {
        float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationVelocity, 0.1f);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }
    #endregion

    #region Translation Handling
    private void HandleTranslation(Vector3 movement)
    {
        Vector3 direction = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f) * movement;

        velocityXZ.x = direction.x * curSpeed;
        velocityXZ.z = direction.z * curSpeed;
    }
    #endregion

    #region Gravity Handling
    private void ApplyGravity()
    {
        if (!isGrounded)
        {
            velocityY += gravity * Time.deltaTime;
            //Debug.Log("Applying gravity. VelocityY: " + velocityY); 
        }
        else if (isGrounded && velocityY < 0)
        {
            velocityY = -2f; 
            //Debug.Log("Landed. VelocityY: " + velocityY); 
        }
    }
    #endregion
}
