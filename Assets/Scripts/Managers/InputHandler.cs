using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    public class InputHandler : MonoBehaviour
    {
        public static InputHandler Instance { get; set; }
    
        // Connection with Input Asset
        [SerializeField] private InputActionAsset inputActionAsset;
    
        private InputActionMap _playerActionMap;
        private InputActionMap _uiActionMap;

        private InputAction _moveAction;
        private InputAction _jumpAction;
        private InputAction _attackAction;
        private InputAction _dashAction;
    
        // Connection with other GameObjects
        public event Action<Vector2> OnMove;
        public event Action OnJumpStarted;
        public event Action OnJumpStopped;
        public event Action OnAttack;
        public event Action OnDashStarted;
        public event Action OnDashStopped;

        private void Awake()
        {
            inputActionAsset = Resources.Load<InputActionAsset>("InputSystem_Actions");
        
            _playerActionMap = inputActionAsset.FindActionMap("Player");
            _uiActionMap = inputActionAsset.FindActionMap("UI");
        
            _moveAction = _playerActionMap.FindAction("Move");
            _jumpAction = _playerActionMap.FindAction("Jump");
            _attackAction = _playerActionMap.FindAction("Attack");
            _dashAction = _playerActionMap.FindAction("Dash");

            if (Instance == null)
            {
                Instance = this;
            }
        }

        private void OnEnable()
        {
            _moveAction.Enable();
            _moveAction.performed += OnMovePerformed;
            _moveAction.canceled += OnMoveCanceled;
        
            _jumpAction.Enable();
            _jumpAction.performed += OnJumpPerformed;
            _jumpAction.canceled += OnJumpCanceled;
        
            _attackAction.Enable();
            _attackAction.performed += OnAttackPerformed;
        
            _dashAction.Enable();
            _dashAction.performed += OnDashPerformed;
            _dashAction.canceled += OnDashCanceled;
        }
    
        private void OnDisable()
        {
            _moveAction.performed -= OnMovePerformed;
            _moveAction.canceled -= OnMoveCanceled;
            _moveAction.Disable();
        
            _jumpAction.performed -= OnJumpPerformed;
            _jumpAction.canceled -= OnJumpCanceled;
            _jumpAction.Disable();
        
            _attackAction.performed -= OnAttackPerformed;
            _attackAction.Disable();
        
            _dashAction.performed -= OnDashPerformed;
            _dashAction.canceled -= OnDashCanceled;
            _dashAction.Disable();
        }

        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            OnMove?.Invoke(context.ReadValue<Vector2>());
        }

        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            OnMove?.Invoke(Vector2.zero);
        }

        private void OnJumpPerformed(InputAction.CallbackContext context)
        {
            OnJumpStarted?.Invoke();
        }

        private void OnJumpCanceled(InputAction.CallbackContext context)
        {
            OnJumpStopped?.Invoke();
        }
    
        private void OnAttackPerformed(InputAction.CallbackContext context)
        {
            OnAttack?.Invoke();
        }

        private void OnDashPerformed(InputAction.CallbackContext context)
        {
            OnDashStarted?.Invoke();
        }

        private void OnDashCanceled(InputAction.CallbackContext context)
        {
            OnDashStopped?.Invoke();
        }
    
        void Start()
        {
        }

        void Update()
        {
        
        }
    }
}
