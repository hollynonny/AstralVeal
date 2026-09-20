using UnityEngine;

namespace PlayerFol.PlStates
{
    public class JumpState : PlayerState
    {
        private float _maxJumpTimer;
        
        public JumpState(PlayerMovement movement) : base(movement) { }

        public override void Enter()
        {
            _maxJumpTimer = Movement.PlayerJumpData.MaxJumpHoldTime;
            Movement.ApplyJumpForce();
        }

        public override void LogicUpdate()
        {
            _maxJumpTimer -= Time.deltaTime;

            if (!Movement.AstralSystem.IsAstral && Movement.PlayerFlags.IsDashed)
            {
                Movement.TurnDashedOff();
                Movement.StateManager.ChangeState(Movement.PlayerStates.DashState);
                return;
            }
            
            if (Movement.OnEdge())
            {
                Movement.StateManager.ChangeState(Movement.PlayerStates.EdgeClimbingState);
                return;
            }
            
            if (!Movement.AstralSystem.IsAstral && Movement.IsTouchingWall())
            {
                Movement.StateManager.ChangeState(Movement.PlayerStates.WallSlidingState);
                return;
            }

            if (!Movement.PlayerJumpData.IsJumpHeld)
            {
                Movement.CutJumpHeight();
                Movement.StateManager.ChangeState(Movement.PlayerStates.FallState);
                return;
            }

            if (_maxJumpTimer <= 0 || Movement.PlayerData.Rigidbody.linearVelocity.y <= 0)
            {
                Movement.StateManager.ChangeState(Movement.PlayerStates.FallState);
            }
        }

        public override void PhysicsUpdate()
        {
            Movement.ApplyAirControl();
        }
        
        public override void Exit() {}
    }
}
