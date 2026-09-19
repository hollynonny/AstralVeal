using Managers;
using PlayerFol.PlayerDataStructs;
using UnityEngine;

namespace PlayerFol
{
    public class PlayerMovement
    {
        #region Variables
        
        public PlayerData PlayerData { get; private set; }

        public PlayerStateManager StateManager { get; private set; }
        
        public PlayerStates PlayerStates { get; private set; }
        public PlayerFlags PlayerFlags { get; private set; }
        
        public PlayerParameters PlayerParameters { get; } = new();
        public PlayerJumpData PlayerJumpData { get; } = new();
        public PlayerDashData PlayerDashData { get; } = new();
        public PlayerWallData PlayerWallData { get; } = new();
        public PlayerEdgeClimbData PlayerEdgeClimbData { get; } = new();
        
        #endregion

        #region Constructor
        
        public PlayerMovement(Rigidbody2D rb, Transform playerTransform, Transform groundCheckPoint,
            BoxCollider2D bc, ParticleSystem pS)
        {
            PlayerData = new PlayerData(playerTransform, groundCheckPoint, rb, bc);
            PlayerStates = new PlayerStates(this, pS);
            PlayerFlags = new PlayerFlags();
            
            StateManager = new PlayerStateManager();
            StateManager.Initialize(PlayerStates.MoveState);
        }
        
        #endregion

        #region Update Methods
        
        public void Update()
        {
            StateManager.CurrentState.LogicUpdate();
        }

        public void FixedUpdate()
        {
            StateManager.CurrentState.PhysicsUpdate();
            PlayerJumpData.JumpBufferCounter = Mathf.Max(0, PlayerJumpData.JumpBufferCounter - Time.fixedDeltaTime);
            PlayerWallData.WallUnstickTimer = Mathf.Max(0, PlayerWallData.WallUnstickTimer - Time.fixedDeltaTime);
            
            if(!PlayerFlags.CanDash)
                PlayerDashData.DashCooldownCounter = Mathf.Max(0, PlayerDashData.DashCooldownCounter - Time.fixedDeltaTime);

            if (PlayerDashData.DashCooldownCounter <= 0 && PlayerFlags.OnObjectDash)
            {
                PlayerFlags.OnObjectDash = false;
                SetCanDash(true);
            }
        }
        
        #endregion
        
        #region Jump Methods
        
        public void ApplyJumpForce(float multiplier)
        {
            PlayerJumpData.JumpBufferCounter = 0.0f;
            PlayerJumpData.CoyoteTimeCounter = 0.0f;

            float direction;
            if (IsTouchingWall() || PlayerWallData.WallUnstickTimer > 0)
            {
                float jumpOutDirection = PlayerFlags.IsFacingRight ? -1.0f : 1.0f;
                direction = (PlayerParameters.WallJumpForce * jumpOutDirection) 
                            + (PlayerData.MoveInput.x * (PlayerParameters.Speed * 0.5f));
            }
            else
            {
                direction = PlayerData.Rigidbody.linearVelocity.x;
            }

            PlayerData.Rigidbody.linearVelocity = new Vector2(
                direction,
                PlayerParameters.JumpForce * multiplier
            );
        }

        public void CutJumpHeight()
        {
            PlayerData.Rigidbody.linearVelocity = new Vector2(
                PlayerData.Rigidbody.linearVelocity.x,
                PlayerData.Rigidbody.linearVelocity.y * 0.3f
            );
        }

        public void OnJumpStarted()
        {
            PlayerJumpData.JumpBufferCounter = PlayerJumpData.JumpBuffer;
            PlayerJumpData.IsJumpHeld = true;
        }

        public void OnJumpStopped()
        {
            PlayerJumpData.IsJumpHeld = false;
        }
        
        #endregion

        #region Timers
        
        public void UpdateGroundedTimers()
        {
            PlayerJumpData.CoyoteTimeCounter = PlayerJumpData.CoyoteTime;

            if (PlayerDashData.DashCooldownCounter <= 0)
                SetCanDash(true);
        }

        public void UpdateFallTimers()
        { 
            PlayerJumpData.CoyoteTimeCounter = Mathf.Max(0, PlayerJumpData.CoyoteTimeCounter - Time.fixedDeltaTime);
        }

        public void UpdateWallTimers()
        {
            PlayerJumpData.CoyoteTimeCounter = PlayerJumpData.CoyoteTime;
            
            if(PlayerDashData.DashCooldownCounter <= 0)
                SetCanDash(true);
        }

        public void StartEdgeClimbingTimer()
        {
            PlayerFlags.ClimbingOnEdge = true;
            PlayerEdgeClimbData.ClimbingEdgeTimer = PlayerEdgeClimbData.ClimbingEdgeTime;
        }

        public void UpdateEdgeClimbingTimers()
        {
            PlayerEdgeClimbData.ClimbingEdgeTimer = Mathf.Max(0, PlayerEdgeClimbData.ClimbingEdgeTimer - Time.fixedDeltaTime);
            
            if (PlayerEdgeClimbData.ClimbingEdgeTimer <= 0)
                PlayerFlags.ClimbingOnEdge = false;
        } 
        
        #endregion

        #region Air(Fall) Methods
        
        public void ApplyAirControl()
        {
            if (PlayerWallData.WallUnstickTimer > 0) return;
            
            PlayerData.Rigidbody.linearVelocity = new Vector2(
                PlayerData.MoveInput.x * PlayerParameters.AirSpeed,
                PlayerData.Rigidbody.linearVelocity.y
            );
        }
        
        #endregion

        #region Movement Methods

        public void SetMove(Vector2 input)
        {
            PlayerData.MoveInput = input;
            
            if (input.x == 0) return;
            PlayerFlags.IsFacingRight = input.x > 0 || !(input.x < 0);
        }
        
        #endregion

        #region Dash Methods
        
        public void OnDashStarted()
        {
            if (!PlayerFlags.CanDash || PlayerFlags.IsDashing) return;

            PlayerFlags.IsDashHeld = true;
            
            PlayerFlags.IsDashed = true;
            
            PlayerFlags.IsDashing = true;
            SetCanDash(false);
            PlayerDashData.DashTimerCounter = PlayerDashData.DashTimer; 
            PlayerData.Rigidbody.gravityScale = 0;
        }

        public void OnDashStopped()
        {
            PlayerFlags.IsDashHeld = false;
        }

        public void TurnDashedOff()
        {
            PlayerFlags.IsDashed = false;
        }

        public void UpdateDashState()
        {
            PlayerDashData.DashTimerCounter -= Time.deltaTime;
            
            if (PlayerDashData.DashTimerCounter <= 0 || !PlayerFlags.IsDashHeld)
            {
                if (IsGrounded() || IsTouchingWall())
                    PlayerFlags.OnObjectDash = true;
                
                PlayerFlags.IsDashing = false;
                PlayerDashData.DashCooldownCounter = PlayerDashData.DashCooldown;
                StateManager.ChangeState(PlayerStates.FallState);
            }
        }
        
        private void SetCanDash(bool canDash)
        {
            PlayerFlags.CanDash = canDash;
        }
        
        #endregion

        #region Gravity Methods
        
        public void TurnOnGravity()
        {
            PlayerData.Rigidbody.gravityScale = PlayerParameters.DefaultGravityScale;
        }
        
        #endregion

        #region Check Methods
        
        public bool IsGrounded()
        {
            return Physics2D.OverlapCircle(
                PlayerData.GroundCheckPoint.position, 
                PlayerParameters.GroundCheckRadius, 
                LayerManager.GroundLayerMask
            );
        }

        public bool IsTouchingWall()
        {
            if (PlayerWallData.WallUnstickTimer > 0.0f) return false;
            
            Vector2 direction = Vector2.right * (PlayerFlags.IsFacingRight ? 1.0f : -1.0f);

            bool hit = Physics2D.BoxCast(
                PlayerData.Collider.bounds.center,
                PlayerData.Collider.bounds.size,
                0.0f,
                direction,
                PlayerParameters.WallCheckDistance,
                LayerManager.GroundLayerMask
            );

            return hit;
        }
        
        public bool OnEdge()
        {
            float directionX = PlayerFlags.IsFacingRight ? 1.0f : -1.0f;
            Vector2 direction = Vector2.right * directionX;

            bool previousQueries = Physics2D.queriesStartInColliders;
            Physics2D.queriesStartInColliders = false;

            float originX = PlayerData.Collider.bounds.center.x;
            float castDistance = PlayerParameters.WallCheckDistance + PlayerData.Collider.bounds.extents.x;
            
            Vector2 bottomOrigin = new Vector2(
                originX, 
                PlayerData.Collider.bounds.center.y + (PlayerData.Collider.bounds.extents.y * 0.3f)
            );

            Vector2 topOrigin = new Vector2(
                originX, 
                PlayerData.Collider.bounds.max.y + 0.05f
            );

            RaycastHit2D bottomHit = Physics2D.Raycast(
                bottomOrigin, 
                direction, 
                castDistance, 
                LayerManager.GroundLayerMask
            );
    
            RaycastHit2D topHit = Physics2D.BoxCast(
                topOrigin, 
                new Vector2(0.1f, 0.2f), 
                0f, 
                direction, 
                castDistance, 
                LayerManager.GroundLayerMask
            );
            
            Physics2D.queriesStartInColliders = previousQueries;

            if (bottomHit.collider && !topHit.collider)
            {
                PlayerEdgeClimbData.EdgeHit = bottomHit;
                return true;
            }

            return false;
        } 
        
        #endregion

        #region Get Methods
        
        public Vector2 GetLedgePos()
        {
            if (!PlayerEdgeClimbData.EdgeHit.collider) return Vector2.zero;
            
            float direction = PlayerFlags.IsFacingRight ? 1.0f : -1.0f;

            float wallX = PlayerEdgeClimbData.EdgeHit.point.x;
            float platformTopY = PlayerEdgeClimbData.EdgeHit.collider.bounds.max.y;

            float targetX = wallX + (direction * PlayerData.Collider.bounds.extents.x);
            float targetY = platformTopY + PlayerData.Collider.bounds.extents.y;
            
            return new Vector2(targetX, targetY);
        }
        
        #endregion
    }
}
