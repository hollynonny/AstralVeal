using UnityEngine;

namespace PlayerFol.PlayerDataStructs
{
    public class PlayerFlags
    {
        public bool IsDashHeld { get; set; }
        public bool IsDashing { get; set; }
        public bool CanDash { get; set; }
        public bool IsDashed { get; set; }
        public bool OnObjectDash { get; set; }
        public bool ClimbingOnEdge { get; set; }
        public bool IsFacingRight { get; set; }
    }
}
