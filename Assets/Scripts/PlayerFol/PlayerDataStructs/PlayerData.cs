using UnityEngine;

namespace PlayerFol.PlayerDataStructs
{
    public class PlayerData
    {
        public Transform PlayerTransform { get; private set; }
        
        public Transform GroundCheckPoint { get; private set; }
        
        public Rigidbody2D Rigidbody { get; private set; }
        
        public Vector2 MoveInput { get; set; }
        
        public BoxCollider2D Collider { get; private set; }

        public PlayerData(Transform playerTransform, Transform groundCheckPoint, Rigidbody2D rigidbody,
            BoxCollider2D collider)
        {
            PlayerTransform = playerTransform;
            GroundCheckPoint = groundCheckPoint;
            Rigidbody = rigidbody;
            Collider = collider;
        }
    }
}
