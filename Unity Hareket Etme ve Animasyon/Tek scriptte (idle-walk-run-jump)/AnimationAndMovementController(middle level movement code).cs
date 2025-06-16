//? optimizasyon yapılmış kod

using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


public class AnimationAndMovementController : MonoBehaviour
{
    //* declare referance variables
    PlayerInput playerInput;
    CharacterController characterController;
    Animator animator;


    // //* variables to store player input values
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    bool isMovementPressed;
    bool isRunPressed;

    //* for optimization(performance)
    int isWalkingHash;
    int isRunningHash;

    //* constants
    float runMultiplier = 3f;
    float rotationFactorForFrame = 15.0f;

    int zero = 0;

    //* gravity variables
    float gravity = -9.8f;
    float groundedGravity = -.05f;

    //* Jumping variables

    bool isJumpedPressed = false;
    float initialJumpVelocity;
    float maxJumpHeight = 4.0f;
    float maxJumpTime = 0.75f;
    bool isJumping = false;

    //* awake is called earlier than start functions in unity's event life cycle
    void Awake()
    {
        //* initially set referance variables
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>(); 
        animator = GetComponent<Animator>();

        //* for performance(optimization)
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");


        //* set the player input callbacks
        playerInput.CharacterControls.Move.started += OnMovementInput;
        playerInput.CharacterControls.Move.canceled += OnMovementInput;
        playerInput.CharacterControls.Move.performed += OnMovementInput;
        playerInput.CharacterControls.Run.started += onRun;
        playerInput.CharacterControls.Run.canceled += onRun;
        playerInput.CharacterControls.Jump.started += onJump;
        playerInput.CharacterControls.Jump.canceled += onJump;
        setupJumpVariables();

    }

    void setupJumpVariables()
    {
        float timeToApex = (maxJumpTime /2);
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2*maxJumpHeight)/ timeToApex;
    }

    void handleJump()
    {

        if(!isJumping && characterController.isGrounded && isJumpedPressed){
            isJumping = true;
            currentMovement.y = initialJumpVelocity * .5f;
            currentRunMovement.y = initialJumpVelocity *.5f;
            // float previousYVelocity = currentMovement.y;
            // float newYVelocity = currentMovement.y + (gravity * Time.deltaTime);
            // float nextYVelocity =(previousYVelocity + newYVelocity) * .5f;
            // currentMovement.y = nextYVelocity;
            // currentRunMovement.y = nextYVelocity;
        }else if(!isJumpedPressed && characterController.isGrounded && isJumping){
            isJumping = false;
        }
    }
    void onJump(InputAction.CallbackContext context)
    {
        isJumpedPressed = context.ReadValueAsButton();
    }


    void onRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();

    }
    void handleRotation()
    {
        //* the change in position our chracter should porint to
        Vector3 positionToLookAt;
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;
        //* the current rotation our character
        Quaternion currrentRotation = transform.rotation;
        
        if(isMovementPressed)
        {
            //* create a new rotation based on where player is currently pressing
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currrentRotation,targetRotation,rotationFactorForFrame* Time.deltaTime);

        }
    }

    void Update()
    {
        handleRotation();
        handleAnimation();

        if(isRunPressed)
        {
            characterController.Move(currentRunMovement * Time.deltaTime);
            
        } else {
            characterController.Move(currentMovement * Time.deltaTime);

        }
        handleGravity();
        handleJump();


        
    }
    

    void OnMovementInput(InputAction.CallbackContext context){
        currentMovementInput =  context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        currentRunMovement.x = currentMovementInput.x *runMultiplier;
        currentRunMovement.z = currentMovementInput.y *runMultiplier;
        isMovementPressed = currentMovementInput.x !=zero || currentMovementInput.y !=zero ;

    }

    void handleAnimation()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);

        if(isMovementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
        }
        else if(!isMovementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }

        if((isMovementPressed && isRunPressed) && !isRunning)
        {
            animator.SetBool(isRunningHash, true);

        }
        else if((!isMovementPressed || !isRunPressed) && isRunning)
        {
            animator.SetBool(isRunningHash, false);
            
        }

    }

    void handleGravity()
    {
        bool isFalling = currentMovement.y <= 0.0f || !isJumpedPressed;
        float fallMultiplier = 2.0f;
        if(characterController.isGrounded)
        {
            currentMovement.y = groundedGravity;
            currentRunMovement.y  = groundedGravity;
        }else if(isFalling){
            float previousYVelocity = currentMovement.y;
            float newYVelocity = currentMovement.y + (gravity* fallMultiplier* Time.deltaTime);
            float nextYVelocity =Mathf.Max((previousYVelocity + newYVelocity) * .5f,-20.0f);
            currentMovement.y = nextYVelocity;
            currentRunMovement.y = nextYVelocity;

        }else{
            float previousYVelocity = currentMovement.y;
            float newYVelocity = currentMovement.y + (gravity * Time.deltaTime);
            float nextYVelocity =(previousYVelocity + newYVelocity) * .5f;
            currentMovement.y = nextYVelocity;
            currentRunMovement.y = nextYVelocity;
            
        }
    }

    
   
    void OnEnable()
    {
        //* Enable the chractercontrols action map
        playerInput.CharacterControls.Enable();
    }
    void OnDisable()
    {
        //* Disable the chractercontrols action map
        playerInput.CharacterControls.Disable();
    }


    
}
