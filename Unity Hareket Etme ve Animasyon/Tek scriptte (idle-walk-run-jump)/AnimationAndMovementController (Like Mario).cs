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
    Vector3 appliedMovement;
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
    float maxJumpHeight = 2.0f;
    float maxJumpTime = 0.75f;
    bool isJumping = false;
    int isJumpingHash;
    int jumpCountHash;

    bool isJumpAnimating = false;
    int jumpCount = 0;
    Coroutine currentJumpResetRoutine = null;

    Dictionary<int, float> initialJumpVelocities = new Dictionary<int, float>(); 
    Dictionary<int, float> jumpGravities = new Dictionary<int, float>(); 

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
        isJumpingHash = Animator.StringToHash("isJumping");
        jumpCountHash = Animator.StringToHash("jumpCount");


        //* set the player input callbacks
        playerInput.CharacterControls.Move.started += OnMovementInput;
        playerInput.CharacterControls.Move.canceled += OnMovementInput;
        playerInput.CharacterControls.Move.performed += OnMovementInput;
        playerInput.CharacterControls.Run.started += onRun;
        playerInput.CharacterControls.Run.canceled += onRun;
        playerInput.CharacterControls.Jump.started += onJump;
        playerInput.CharacterControls.Jump.canceled += onJump;
        SetupJumpVariables();

    }

    void SetupJumpVariables()
    {
        float timeToApex = (maxJumpTime /2);
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2*maxJumpHeight)/ timeToApex;
        float secondJumpGravity = (-2 * (maxJumpHeight + 2)) / Mathf.Pow((timeToApex * 1.25f),2);
        float secondJumpInıtialVelocity= (2 * (maxJumpHeight + 2)) / (timeToApex * 1.25f);
        float thirdJumpGravity = (-2 * (maxJumpHeight + 4)) / Mathf.Pow((timeToApex * 1.5f),2);
        float thirdJumpInıtialVelocity= (2 * (maxJumpHeight + 4)) / (timeToApex * 1.5f);

        initialJumpVelocities.Add(1,initialJumpVelocity);
        initialJumpVelocities.Add(2,secondJumpInıtialVelocity);
        initialJumpVelocities.Add(3,thirdJumpInıtialVelocity);

        jumpGravities.Add(0,gravity);
        jumpGravities.Add(1,gravity);
        jumpGravities.Add(2,secondJumpGravity);
        jumpGravities.Add(3,thirdJumpGravity);

    }

    void JandleJump()
    {
        if(!isJumping && characterController.isGrounded && isJumpedPressed){
            if(jumpCount < 3 && currentJumpResetRoutine != null){
                StopCoroutine(currentJumpResetRoutine);
            }
            animator.SetBool(isJumpingHash, true);
            isJumpAnimating = true;
            isJumping = true;
            jumpCount += 1;
            animator.SetInteger(jumpCountHash, jumpCount);
            currentMovement.y = initialJumpVelocities[jumpCount];
            appliedMovement.y = initialJumpVelocities[jumpCount];
            
        }else if(!isJumpedPressed && characterController.isGrounded && isJumping){
            isJumping = false;
        }
    }

    IEnumerator jumpResetRoutine(){
        yield return new WaitForSeconds(.5f);
        jumpCount = 0;
    }
    void onJump(InputAction.CallbackContext context)
    {
        isJumpedPressed = context.ReadValueAsButton();
    }


    void onRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();

    }
    void HandleRotation()
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
        HandleRotation();
        HandleAnimation();

        if(isRunPressed)
        {
            appliedMovement.x = currentRunMovement.x;
            appliedMovement.z = currentRunMovement.z;
        } else {
            appliedMovement.x = currentRunMovement.x;
            appliedMovement.z = currentRunMovement.z;
        }

        characterController.Move(appliedMovement* Time.deltaTime);
        HandleGravity();
        HandleJump();


        
    }
    

    void OnMovementInput(InputAction.CallbackContext context){
        currentMovementInput =  context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        currentRunMovement.x = currentMovementInput.x *runMultiplier;
        currentRunMovement.z = currentMovementInput.y *runMultiplier;
        isMovementPressed = currentMovementInput.x !=zero || currentMovementInput.y !=zero ;

    }

    void HandleAnimation()
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

    void HandleGravity()
    {
        bool isFalling = currentMovement.y <= 0.0f || !isJumpedPressed;
        float fallMultiplier = 2.0f;
        //* apply proper gravity if the player is grounded or not
        if(characterController.isGrounded)
        {
            if(isJumpAnimating ){
                animator.SetBool(isJumpingHash, false);
                isJumpAnimating = false;
                currentJumpResetRoutine = StartCoroutine(jumpResetRoutine());
                if(jumpCount == 3){
                    jumpCount =0;
                    animator.SetInteger(jumpCountHash,jumpCount);
                }
            }
            currentMovement.y = groundedGravity;
            appliedMovement.y  = groundedGravity;
        }else if(isFalling){
            float previousYVelocity = currentMovement.y;
            currentMovement.y = currentMovement.y + (jumpGravities[jumpCount] * fallMultiplier* Time.deltaTime);
            appliedMovement.y =Mathf.Max((previousYVelocity + currentMovement.y) * .5f,-20.0f);
            

        }else{
            float previousYVelocity = currentMovement.y;
            currentMovement.y = currentMovement.y + (jumpGravities[jumpCount] * Time.deltaTime);
            appliedMovement.y =(previousYVelocity + currentMovement.y) * .5f;
            
            
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
