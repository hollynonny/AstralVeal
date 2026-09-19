using UnityEngine;

namespace PlayerFol.PlayerDataStructs
{
    public class PlayerEdgeClimbData
    {
        public float ClimbingEdgeTime { get; } = 0.25f;
        public float ClimbingEdgeTimer { get; set; }
        public RaycastHit2D EdgeHit { get; set; }
    }
}
