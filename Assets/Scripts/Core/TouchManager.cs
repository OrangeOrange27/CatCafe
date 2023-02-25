using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core
{
    public class TouchManager : MonoBehaviour
    {
        [SerializeField] private PlayerInput _playerInput;

        private InputAction _touchPositionAction;
        private InputAction _touchPressAction;
        private Camera _mainCamera;

        public event Action<Vector2> OnTouchPerformed;
        public event Action<Vector2> OnTouchPositionChanged;
        public event Action OnTouchEnded;

        private void Awake()
        {
            _touchPressAction = _playerInput.actions[GlobalConstants.TOUCH_PRESS_ACTION_NAME];
            _touchPositionAction = _playerInput.actions[GlobalConstants.TOUCH_POSITION_ACTION_NAME];
        }

        private void OnEnable()
        {
            _touchPressAction.performed += OnTouchPressed;
            _touchPressAction.canceled += OnTouchCanceled;

            _touchPositionAction.performed += OnTouchPositionActionPerformed;
        }

        private void OnDisable()
        {
            _touchPressAction.performed -= OnTouchPressed;
            _touchPressAction.canceled -= OnTouchCanceled;
            
            _touchPositionAction.performed -= OnTouchPositionActionPerformed;
        }

        private void OnTouchPressed(InputAction.CallbackContext context)
        {
            OnTouchPerformed?.Invoke(_touchPositionAction.ReadValue<Vector2>());
        }

        private void OnTouchCanceled(InputAction.CallbackContext context)
        {
            OnTouchEnded?.Invoke();
        }

        private void OnTouchPositionActionPerformed(InputAction.CallbackContext context)
        {
            OnTouchPositionChanged?.Invoke(context.ReadValue<Vector2>());
        }
    }
}
