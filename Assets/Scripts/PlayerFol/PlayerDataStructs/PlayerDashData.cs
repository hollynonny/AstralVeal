using UnityEngine;

namespace PlayerFol.PlayerDataStructs
{
    public class PlayerDashData
    {
        public float DashTimer { get; } = 0.3f;
        public float DashTimerCounter { get; set; }
        public float DashCooldown { get; } = 0.25f;
        public float DashCooldownCounter { get; set; }
    }
}
