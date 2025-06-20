using System;

public  abstract class PlayerBaseState 
{
    private protected bool _isRootState = false;
    private protected PlayerStateMachine _ctx;
    private protected PlayerStateFactory _factory;
    private protected PlayerBaseState _currentSuperState;
    private protected PlayerBaseState _currentSubState;
    protected bool IsRootState{ set {_isRootState = value;}}
    protected PlayerStateMachine Ctx{ get { return _ctx;}}
    protected PlayerStateFactory Factory{ get {return _factory;}}
    public PlayerBaseState(PlayerStateMachine currentContext,PlayerStateFactory playerStateFactory){
        _ctx = currentContext;
        _factory = playerStateFactory;
    }
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();
    public abstract void InitalizeSubState();

    public void UpdateStates()
    {
        UpdateState();
        if(_currentSubState != null){
            _currentSubState.UpdateStates();
        }
    }

    
    
    protected void SwitchState(PlayerBaseState newState){ 
        //* current state exit state
        ExitState();

        //* new state enter state
        newState.EnterState();

        if(_isRootState){
            //* switch current state of context
            _ctx.CurrentState = newState;
        }else if(_currentSuperState != null){
            _currentSuperState.SetSubState(newState);
        }

    }
    protected void SetSuperState(PlayerBaseState newSuperState){
        _currentSuperState = newSuperState;
    }
    protected void SetSubState(PlayerBaseState newSubState){
        _currentSubState = newSubState;
        newSubState.SetSubState(this);
    }
    
}
