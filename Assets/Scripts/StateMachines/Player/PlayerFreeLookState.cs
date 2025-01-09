using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");

    public PlayerFreeLookState(PlayerStateMachine stateMachine) : base(stateMachine){}

    private const float AnimatorDampTime = 0.1f;
    public override void Enter()
    {

    }
    public override void Tick(float deltaTime)
    {
        

        Vector3 movement = CalculateMovement();

        stateMachine.CharacterController.Move(movement * stateMachine.FreeLookMovementSpeed * deltaTime);

        if(stateMachine.InputReader.MovementValue == Vector2.zero) 
        {
            stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0, AnimatorDampTime, deltaTime);
            return; 
        }

        stateMachine.Animator.SetFloat(FreeLookSpeedHash, 1, AnimatorDampTime, deltaTime);
        FaceMovementDirection(movement, deltaTime);
    }
    public override void Exit()
    {

    }

    private Vector3 CalculateMovement()
    {
        Vector3 rightVector = stateMachine.MainCameraTransform.right;
        Vector3 forwardVector = stateMachine.MainCameraTransform.forward;
        
        rightVector.y = 0;
        forwardVector.y = 0;
        
        rightVector.Normalize();
        forwardVector.Normalize();
        
        return forwardVector * stateMachine.InputReader.MovementValue.y +
            rightVector * stateMachine.InputReader.MovementValue.x;
    }

    private void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        stateMachine.transform.rotation = Quaternion.Lerp(
            stateMachine.transform.rotation,
            Quaternion.LookRotation(movement),
            deltaTime * stateMachine.RoatationSmoothValue);
    }
}
