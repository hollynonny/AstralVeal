using UnityEngine;

namespace PlayerFol.PlStates
{
    public class EdgeClimbingState : PlayerState
    {
        private Vector2 _targetPos;
        
        public EdgeClimbingState(PlayerMovement movement) : base(movement) {}

        public override void Enter()
        {
            Movement.PlayerData.Rigidbody.linearVelocity = Vector2.zero;
            Movement.PlayerData.Rigidbody.gravityScale = 0.0f;
            Movement.PlayerData.Rigidbody.bodyType = RigidbodyType2D.Kinematic;
            
            _targetPos = Movement.GetLedgePos();
            if (_targetPos == Vector2.zero)
            {
                Movement.StateManager.ChangeState(Movement.PlayerStates.FallState);
                return;
            }


            Movement.StartEdgeClimbingTimer();
        }

        public override void LogicUpdate()
        {
            if (Movement.PlayerFlags.ClimbingOnEdge) return;
            
            if (Movement.IsGrounded())
            {
                Movement.StateManager.ChangeState(Movement.PlayerStates.MoveState);
                return;
            }
            
            if (!Movement.IsGrounded())
            {
                Movement.StateManager.ChangeState(Movement.PlayerStates.FallState);
            }
        }

        public override void PhysicsUpdate()
        {
            Movement.UpdateEdgeClimbingTimers();

            Movement.PlayerData.PlayerTransform.position = Vector2.MoveTowards(
                Movement.PlayerData.PlayerTransform.position,
                _targetPos,
                Movement.PlayerEdgeClimbData.ClimbingEdgeTime
            );
        }

        public override void Exit()
        {
            Movement.PlayerData.Rigidbody.bodyType = RigidbodyType2D.Dynamic;
            Movement.TurnOnGravity();
        }
    }
}
