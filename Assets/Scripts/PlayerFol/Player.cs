using Managers;
using UnityEngine;

namespace PlayerFol
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Transform groundCheckPoint;
    
        private PlayerMovement _playerMovement;
        private PlayerAnimation _playerAnimation;

        private Rigidbody2D _rb;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            if(_rb == null) Debug.LogWarning("No Rigidbody2D attached to the Player!");
            
            Animator anim = GetComponent<Animator>();
            if(anim == null) Debug.LogWarning("No Animator attached to the Player!");
            
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (_spriteRenderer == null) Debug.LogWarning("No SpriteRenderer attached to the Player!");
            
            ParticleSystem pS = GetComponentInChildren<ParticleSystem>();
            if(pS == null) Debug.LogWarning("No ParticleSystem attached to the Player!");
            pS.Stop();
            
            BoxCollider2D bc = GetComponent<BoxCollider2D>();
        
            _playerMovement = new PlayerMovement(_rb, this.transform, groundCheckPoint, bc, pS);
            _playerAnimation = new PlayerAnimation(anim);
        }
        
        private void OnEnable()
        {
            InputHandler.Instance.OnMove += OnMove;
        
            InputHandler.Instance.OnJumpStarted += OnJumpStarted;
            InputHandler.Instance.OnJumpStopped += OnJumpStopped;
        
            InputHandler.Instance.OnDashStarted += OnDashStarted;
            InputHandler.Instance.OnDashStopped += OnDashStopped;
        }
    
        private void OnDisable()
        {
            InputHandler.Instance.OnMove -= OnMove;
        
            InputHandler.Instance.OnJumpStarted -= OnJumpStarted;
            InputHandler.Instance.OnJumpStopped -= OnJumpStopped;
        
            InputHandler.Instance.OnDashStarted -= OnDashStarted;
            InputHandler.Instance.OnDashStopped -= OnDashStopped;
        }
    
        private void OnMove(Vector2 input)
        {
            _playerMovement.SetMove(input);
            _playerAnimation.SetMoveAnimation(Mathf.Abs(input.x));
        }

        private void OnJumpStarted()
        {
            _playerMovement.OnJumpStarted();
        }

        private void OnJumpStopped()
        {
            _playerMovement.OnJumpStopped();
        }

        private void OnDashStarted()
        {
            _playerMovement.OnDashStarted();
        }

        private void OnDashStopped()
        {
            _playerMovement.OnDashStopped();
        }

        private void FixedUpdate()
        {
            _playerMovement.FixedUpdate();
        }

        private void Update()
        {
            _playerMovement.Update();
            _playerAnimation.SetJumpAnimation(_rb.linearVelocity.y);
            _playerAnimation.SetDashAnimation(_playerMovement.PlayerFlags.IsDashing);
            _spriteRenderer.flipX = !_playerMovement.PlayerFlags.IsFacingRight;
        }
    }
}
