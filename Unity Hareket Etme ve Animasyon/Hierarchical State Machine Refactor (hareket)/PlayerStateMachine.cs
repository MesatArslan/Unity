using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    //* declare referance variables
    PlayerInput _playerInput;
    CharacterController _characterController;
    Animator _animator;


    // //* variables to store player input values
    Vector2 _currentMovementInput;
    Vector3 _currentMovement;
    Vector3 _currentRunMovement;
    Vector3 _appliedMovement;
    bool _isMovementPressed;
    bool _isRunPressed;

    //* for optimization(performance)
    int _isWalkingHash;
    int _isRunningHash;

    //* constants
    float _runMultiplier = 3f;
    float _rotationFactorForFrame = 15.0f;

    int _zero = 0;

    //* gravity variables
    float _gravity = -9.8f;
    float _groundedGravity = -.05f;

    //* Jumping variables

    bool _isJumpPressed = false;
    float _initialJumpVelocity;
    float _maxJumpHeight = 2.0f;
    float _maxJumpTime = 0.75f;
    bool _isJumping = false;
    int _isJumpingHash;
    int _jumpCountHash;

    bool _requireNewJumpPress = false;
    int _jumpCount = 0;

    Dictionary<int, float> _initialJumpVelocities = new Dictionary<int, float>(); 
    Dictionary<int, float> _jumpGravities = new Dictionary<int, float>(); 

    Coroutine _currentJumpResetRoutine = null;

    //* state variables
    PlayerBaseState _currentState;
    PlayerStateFactory _states;

    //* getters and setters
    public PlayerBaseState CurrentState{ get {return _currentState;} set { _currentState = value; }}
    public Animator Animator{ get {return _animator;}}
    public CharacterController CharacterController{ get {return _characterController;}}
    public Coroutine CurrentJumpResetRoutine { get { return  _currentJumpResetRoutine;} set {_currentJumpResetRoutine = value; }}
    public Dictionary<int, float> InitialJumpVelocities { get {return _initialJumpVelocities; }}
    public Dictionary<int, float> JumpGravities { get {return _jumpGravities; }}
    public int JumpCount{ get {return _jumpCount;} set {_jumpCount = value; }}
    public int IsJumpingHash{ get { return _isJumpingHash;}}
    public int IsWalkingHash{ get {return _isWalkingHash;}}
    public int IsRunningHash{ get {return _isRunningHash;}}
    public int JumpCountHash{ get {return _jumpCountHash;}}
    public bool RequirementNewJumpPress{ get {return _requireNewJumpPress;} set { _requireNewJumpPress = value;}}
    public bool IsJumping{ set { _isJumping = value;}}
    public bool IsJumpPressed{ get {return _isJumpPressed;} }
    public bool IsRunPressed{ get {return _isRunPressed;}}
    public bool IsMovementPressed{ get {return _isMovementPressed;}}
    public float CurrentMovementY{ get {return _currentMovement.y;} set {_currentMovement.y = value;}}
    public float AppliedMovementY{ get {return _appliedMovement.y;} set {_appliedMovement.y = value;}}
    public float GroundedGravity{ get {return _groundedGravity;}}
    public float AppliedMovementX{ get {return _appliedMovement.x;} set {_appliedMovement.x = value;}}
    public float AppliedMovementZ{ get {return _appliedMovement.z;} set {_appliedMovement.z = value;}}
    public float RunMultiplier{ get {return _runMultiplier;}}
    public Vector2 CurrentMovementInput{ get {return _currentMovementInput;}}








    //* awake is called earlier than start functions in unity's event life cycle
    void Awake()
    {
        //* initially set referance variables
        _playerInput = new PlayerInput();
        _characterController = GetComponent<CharacterController>(); 
        _animator = GetComponent<Animator>();

        //* setup state
        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();

        //* for performance(optimization)
        _isWalkingHash = Animator.StringToHash("isWalking");
        _isRunningHash = Animator.StringToHash("isRunning");
        _isJumpingHash = Animator.StringToHash("isJumping");
        _jumpCountHash = Animator.StringToHash("jumpCount");


        //* set the player input callbacks
        _playerInput.CharacterControls.Move.started += OnMovementInput;
        _playerInput.CharacterControls.Move.canceled += OnMovementInput;
        _playerInput.CharacterControls.Move.performed += OnMovementInput;
        _playerInput.CharacterControls.Run.started += onRun;
        _playerInput.CharacterControls.Run.canceled += onRun;
        _playerInput.CharacterControls.Jump.started += onJump;
        _playerInput.CharacterControls.Jump.canceled += onJump;
        SetupJumpVariables();

    }

    void SetupJumpVariables()
    {
        float timeToApex = (_maxJumpTime /2);
        _gravity = (-2 * _maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        _initialJumpVelocity = (2*_maxJumpHeight)/ timeToApex;
        float secondJumpGravity = (-2 * (_maxJumpHeight + 2)) / Mathf.Pow((timeToApex * 1.25f),2);
        float secondJumpInıtialVelocity= (2 * (_maxJumpHeight + 2)) / (timeToApex * 1.25f);
        float thirdJumpGravity = (-2 * (_maxJumpHeight + 4)) / Mathf.Pow((timeToApex * 1.5f),2);
        float thirdJumpInıtialVelocity= (2 * (_maxJumpHeight + 4)) / (timeToApex * 1.5f);

        _initialJumpVelocities.Add(1,_initialJumpVelocity);
        _initialJumpVelocities.Add(2,secondJumpInıtialVelocity);
        _initialJumpVelocities.Add(3,thirdJumpInıtialVelocity);

        _jumpGravities.Add(0,_gravity);
        _jumpGravities.Add(1,_gravity);
        _jumpGravities.Add(2,secondJumpGravity);
        _jumpGravities.Add(3,thirdJumpGravity);

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        HandleRotation();
        _currentState.UpdateState();
        _characterController.Move(_appliedMovement* Time.deltaTime);


        
    }

        void HandleRotation()
    {
        //* the change in position our chracter should porint to
        Vector3 positionToLookAt;
        positionToLookAt.x = _currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = _currentMovement.z;
        //* the current rotation our character
        Quaternion currrentRotation = transform.rotation;
        
        if(_isMovementPressed)
        {
            //* create a new rotation based on where player is currently pressing
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currrentRotation,targetRotation,_rotationFactorForFrame* Time.deltaTime);

        }
    }
    void onJump(InputAction.CallbackContext context)
    {
        _isJumpPressed = context.ReadValueAsButton();
        _requireNewJumpPress = false;

    }


    void onRun(InputAction.CallbackContext context)
    {
        _isRunPressed = context.ReadValueAsButton();

    }
    void OnMovementInput(InputAction.CallbackContext context){
        _currentMovementInput =  context.ReadValue<Vector2>();
        _isMovementPressed = _currentMovementInput.x != _zero || _currentMovementInput.y != _zero ;

    }

      void OnEnable()
    {
        //* Enable the chractercontrols action map
        _playerInput.CharacterControls.Enable();
    }
    void OnDisable()
    {
        //* Disable the chractercontrols action map
        _playerInput.CharacterControls.Disable();
    }


}
