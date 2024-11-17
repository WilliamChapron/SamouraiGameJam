using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Anim
    private Animator animator;

    private HealthComponent healthComponent;
    private Attack attackComponent;

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

    private float jumpTime = 0f; // Temps écoulé pendant le saut
    public float accelerationFactor = 2.5f;  // Facteur d'accélération initiale
    public float decelerationFactor = 0.5f;  // Facteur de décélération (ralentissement)
    public float fallAccelerationFactor = 2.5f;  // Facteur d'accélération de la chute après 2 secondes
    public float slowTimeDuration = 2f;  // Durée pendant laquelle le saut ralentit

    // Roll
    public float rollSpeed = 15f;  
    public float rollAcceleration = 25f;  
    public float rollDeceleration = 5f; 
    public float rollDuration = 1f;  

    private bool isRolling = false;  
    private float rollTime = 0f; 
    private Vector3 rollDirection;  

    // Crouch settings for roll
    private float originalHeight;
    private Vector3 originalCenter;
    public float crouchHeight = 0.5f; 
    public Vector3 crouchCenter = new Vector3(0f, 0.25f, 0f);  


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

        // Component
        healthComponent = GetComponentInChildren<HealthComponent>();
        attackComponent = GetComponentInChildren<Attack>();
    }
    #endregion

    #region Update Method
    private void Update()
    {

        if (healthComponent.isDead)
        {
            ApplyGravity();

            // Velocity
            currentVelocity.x = 0;
            currentVelocity.z = 0;
            currentVelocity.y = velocityY;
            characterController.Move(currentVelocity * Time.deltaTime);

            if (attackComponent.attackPoint1 != null)
            {
                attackComponent.attackPoint1.parent = null;
                Rigidbody rb1 = attackComponent.attackPoint1.gameObject.GetComponent<Rigidbody>();
                rb1.isKinematic = false; 
                rb1.useGravity = true; 
            }

            if (attackComponent.attackPoint2 != null)
            {
                attackComponent.attackPoint2.parent = null;
                Rigidbody rb2 = attackComponent.attackPoint2.gameObject.GetComponent<Rigidbody>();
                rb2.isKinematic = false; 
                rb2.useGravity = true; 
            }

            return;
        }

        if (Input.GetKeyDown(KeyCode.C) && !isRolling && isGrounded)  
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
        HandleJump();
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

    private void HandleJump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            animator.SetTrigger("Jump");
            isJumping = true;
            jumpTime = 0f; 
            velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity); 
        }

        if (isJumping)
        {
            jumpTime += Time.deltaTime;

            if (jumpTime < 0.3f) 
            {
                velocityY = Mathf.Lerp(velocityY, velocityY + accelerationFactor, jumpTime / 0.3f); 
            }
            else if (jumpTime < slowTimeDuration) 
            {
                velocityY = velocityY + gravity * Time.deltaTime * decelerationFactor; 
            }

            else 
            {
                velocityY = velocityY + gravity * Time.deltaTime * fallAccelerationFactor;
            }
        }
    }

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
        rollTime += Time.deltaTime;  

        if (rollTime < rollDuration / 2f)
        {
            float accelerationFactor = Mathf.Lerp(1f, 1 + rollAcceleration, rollTime / (rollDuration / 2f));
            curSpeed = rollSpeed * accelerationFactor;  
        }
        else if (rollTime < rollDuration)
        {
            float decelerationFactor = Mathf.Lerp(1f, 1 - rollDeceleration, (rollTime - (rollDuration / 2f)) / (rollDuration / 2f));
            curSpeed = rollSpeed * decelerationFactor;  
        }
        else
        {

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






// knockBack 
// UI degat

// Trainee katana
// Finir kunai lancer

// visualiser les degats qu'ont te fait 
//visualiser les degats que tu fait 