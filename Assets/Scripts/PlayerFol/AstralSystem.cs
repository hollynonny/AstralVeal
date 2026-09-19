using UnityEngine;

namespace PlayerFol
{
    public class AstralSystem
    {
        public bool IsAstral { get; private set; }
        
        public AstralSystem()
        {
            IsAstral = false;
        }

        public void ToggleSystem(bool newState)
        {
            IsAstral = newState;
        }
    }
}
