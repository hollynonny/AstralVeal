namespace PlayerFol.PlStates
{
    public class FallState : PlayerState
    {
        public FallState(PlayerMovement movement) : base(movement) { }
        
        public override void Enter() {}

        public override void LogicUpdate()
        {
            if (Movement.PlayerFlags.IsDashed)
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

            if (Movement.IsTouchingWall())
            {
                Movement.StateManager.ChangeState(Movement.PlayerStates.WallSlidingState);
                return;
            }

            if (Movement.IsGrounded())
            {
                Movement.StateManager.ChangeState(Movement.PlayerStates.MoveState);
            }
        }

        public override void PhysicsUpdate()
        {
            Movement.ApplyAirControl();
            
            Movement.UpdateFallTimers();   
        }

        
        public override void Exit() {}
    }
}
