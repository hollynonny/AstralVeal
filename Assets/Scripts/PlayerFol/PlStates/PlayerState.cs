namespace PlayerFol.PlStates
{
    public abstract class PlayerState
    {
        protected PlayerMovement Movement;
        
        public PlayerState(PlayerMovement movement) => Movement = movement;

        public abstract void Enter();
        public abstract void LogicUpdate();
        public abstract void PhysicsUpdate();
        public abstract void Exit();
    }
}
