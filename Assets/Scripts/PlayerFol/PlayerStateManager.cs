using PlayerFol.PlStates;
using UnityEngine;

namespace PlayerFol
{
    public class PlayerStateManager
    {
        public PlayerState CurrentState;
        
        public void Initialize(PlayerState state)
        {
            CurrentState = state;
        }

        public void ChangeState(PlayerState state)
        {
            CurrentState.Exit();
            CurrentState = state;
            CurrentState.Enter();
        }
    }
}
