using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext,playerStateFactory){
        _isRootState = true;
        InitalizeSubState();
    }
    public override void EnterState(){
        _ctx.CurrentMovementY = _ctx.GroundedGravity;
        _ctx.AppliedMovementY  = _ctx.GroundedGravity;
    }
    public override void UpdateState(){
        CheckSwitchStates();
    }
    public override void ExitState(){}
    public override void CheckSwitchStates(){
        //* if player is grounded and jump is pressed, switch to jump state
        if(_ctx.IsJumpPressed && !_ctx.RequirementNewJumpPress){
            SwitchState(_factory.Jump());
        }
    }
    public override void InitalizeSubState(){
        if(!_ctx.IsMovementPressed && !_ctx.IsRunPressed){
            SetSubState(_factory.Idle());
        }else if(_ctx.IsMovementPressed && !_ctx.IsRunPressed){
            SetSubState(_factory.Walk());
        }else{
            SetSubState(_factory.Run());
        }
    }
}
