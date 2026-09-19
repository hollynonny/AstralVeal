using UnityEngine;

namespace PlayerFol.PlStates
{
    public class MoveState : GroundState
    {
        public MoveState(PlayerMovement movement) : base(movement) { }

        public override void Enter()
        {
            
        }
        
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            
            Movement.PlayerData.Rigidbody.linearVelocity = new Vector2(
                Movement.PlayerData.MoveInput.x * Movement.PlayerParameters.Speed,
                Movement.PlayerData.Rigidbody.linearVelocity.y
            );
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
        }

        public override void Exit()
        {
            
        }
    }
}
