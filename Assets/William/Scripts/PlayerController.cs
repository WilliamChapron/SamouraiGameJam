using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Anim
    private Animator animator;

    // Speed
    public float curSpeed = 0f;
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;

    // Camera update depending on player
    public float rotationSpeed = 700f;
    public Transform cameraTransform;

    // Character movement
    private CharacterController characterController;
    private float rotationVelocity;
    private Vector3 currentVelocity; 
    private Vector3 velocityXZ; 
    private float velocityY;

    // Jump & Gravity
    public float gravity = -9.81f;
    public float jumpHeight = 2f;
    private bool isGrounded;
    private bool isJumping;

    // Roll
    public float rollSpeed = 15f;  // La vitesse initiale de la roulade
    public float rollAcceleration = 25f;  // L'accélération pendant la roulade
    public float rollDeceleration = 5f;  // La décélération après la roulade
    public float rollDuration = 1f;  // Durée de la roulade

    private bool isRolling = false;  // Si le joueur est en train de rouler
    private float rollTime = 0f;  // Temps écoulé dans la roulade
    private Vector3 rollDirection;  // Direction de la roulade

    // Crouch settings for roll
    private float originalHeight;
    private Vector3 originalCenter;
    public float crouchHeight = 0.5f;  // Hauteur du collider en mode roulade
    public Vector3 crouchCenter = new Vector3(0f, 0.25f, 0f);  // Centre du collider en mode roulade


    // Shift control to active roll at time
    private float lastShiftTime = -1f; 
    private float maxShiftDelay = 0.5f; 

    //

    #region Initialization
    void Start()
    {
        curSpeed = walkSpeed;
        characterController = GetComponent<CharacterController>();

        originalHeight = characterController.height;
        originalCenter = characterController.center;

        animator = GetComponentInChildren<Animator>();
    }
    #endregion

    #region Update Method
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isRolling && isGrounded)  
        {
            if (lastShiftTime >= 0f && (Time.time - lastShiftTime <= maxShiftDelay))
            {
                StartRoll();
            }
            
        }

        if (isRolling)
        {
            // Handle rolling
            HandleRoll();
        }
        else
        {
            // Handle Rotate, Jump, Sprint
            HandleMovement();
        }

        ApplyGravity();

        // Velocity
        currentVelocity.x = velocityXZ.x;
        currentVelocity.z = velocityXZ.z;
        currentVelocity.y = velocityY;
        characterController.Move(currentVelocity * Time.deltaTime);

        // Anim
        animator.SetFloat("speed", Mathf.SmoothStep(animator.GetFloat("speed"), curSpeed, Time.deltaTime * 21));
        animator.SetBool("isRolling", isRolling);
        //Debug.Log(curSpeed);
    }
    #endregion

    #region Movement Handling
    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized;

        if (Input.GetKey(KeyCode.LeftShift)) 
        {
            curSpeed = sprintSpeed;
            lastShiftTime = Time.time;  
        }
        else
        {
            curSpeed = walkSpeed;
        }

        // Jump
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            animator.SetTrigger("Jump");
            velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isJumping = true;
        }
        //

        if (movement.magnitude >= 0.1f)
        {
            HandleRotation(movement);
            HandleTranslation(movement);
        }
        else
        {
            velocityXZ = Vector3.zero;
            curSpeed = 0f;
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
        Vector3 direction;

        if (isRolling)
        {
            direction = rollDirection;  
        }
        else
        {
            direction = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f) * movement;  
        }

        velocityXZ.x = direction.x * curSpeed;
        velocityXZ.z = direction.z * curSpeed;
    }
    #endregion

    #region Gravity Handling
    private void ApplyGravity()
    {
        isGrounded = characterController.isGrounded;

        if (!isGrounded)
        {
            velocityY += gravity * Time.deltaTime;
        }
        else if (isGrounded && velocityY < 0)
        {
            velocityY = -2f;
            if (isJumping)
            {
                animator.SetTrigger("ExitJump");
                isJumping = false;
            }
            
        }
    }
    #endregion

    #region Roll Handling
    private void StartRoll()
    {
        animator.applyRootMotion = false;
        isRolling = true;
        rollTime = 0f;
        curSpeed = rollSpeed;  

        characterController.height = crouchHeight;
        characterController.center = crouchCenter;

        rollDirection = transform.forward;
    }

    private void HandleRoll()
    {
        rollTime += Time.deltaTime;  // Incrément du temps de roulade

        // Application de l'accélération pendant la première moitié de la roulade
        if (rollTime < rollDuration / 2f)
        {
            float accelerationFactor = Mathf.Lerp(1f, 1 + rollAcceleration, rollTime / (rollDuration / 2f));
            curSpeed = rollSpeed * accelerationFactor;  // Augmente la vitesse pendant la première moitié
        }
        // Application de la décélération pendant la seconde moitié de la roulade
        else if (rollTime < rollDuration)
        {
            float decelerationFactor = Mathf.Lerp(1f, 1 - rollDeceleration, (rollTime - (rollDuration / 2f)) / (rollDuration / 2f));
            curSpeed = rollSpeed * decelerationFactor;  // Réduit la vitesse pendant la seconde moitié
        }
        else
        {
            // Fin de la roulade, retour à l'état initial
            isRolling = false;
            curSpeed = walkSpeed;
            characterController.height = originalHeight;
            characterController.center = originalCenter;
            Debug.Log("Roll Ended: RollTime = " + rollTime);
            animator.applyRootMotion = true;
        }

        HandleTranslation(Vector3.forward);
    }


    #endregion
}

// #TODO box collide crouch - a tester
// 2 compétences

// 2 attaque, lourde et légère
// Intégrer l'animator
// Contre attaque (1 chance de louper)
// knockBack 
// lancer shuriken COMPETENCE 1
// esquive
// HIT REACTION (react