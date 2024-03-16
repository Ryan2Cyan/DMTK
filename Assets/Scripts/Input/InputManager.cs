using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputManager : MonoBehaviour
    {
        public delegate void DMTKInputAction();
        
        public static event DMTKInputAction OnMouseDown;
        public static event DMTKInputAction OnMouseUp;
        public static event DMTKInputAction OnConserveSize;
        public static event DMTKInputAction OnConserveSizeCancel;
        public static Vector2 MousePosition;
        
        private DMTKActions _inputActions;

        private void Awake()
        {
            _inputActions ??= new DMTKActions();
        }

        private void OnEnable()
        {
            _inputActions ??= new DMTKActions();
            _inputActions.DMTKPlayer.Enable();
            
            // Register to input events:
            _inputActions.DMTKPlayer.MouseDown.performed += MouseDown;
            _inputActions.DMTKPlayer.MouseDown.canceled += MouseUp;
            _inputActions.DMTKPlayer.ConservativeResize.performed += ConserveSize;
            _inputActions.DMTKPlayer.ConservativeResize.canceled += ConserveSizeCancel;
        }

        private void OnDisable()
        {
            _inputActions.DMTKPlayer.Disable();
            
            // Unregister from input events:
            _inputActions.DMTKPlayer.MouseDown.performed -= MouseDown;
            _inputActions.DMTKPlayer.MouseDown.canceled -= MouseUp;
            _inputActions.DMTKPlayer.ConservativeResize.performed -= ConserveSize;
            _inputActions.DMTKPlayer.ConservativeResize.canceled -= ConserveSizeCancel;
        }

        private void Update()
        {
            MousePosition = _inputActions.DMTKPlayer.MousePosition.ReadValue<Vector2>();
        }

        private static void MouseDown(InputAction.CallbackContext context) { OnMouseDown?.Invoke(); }
        private static void MouseUp(InputAction.CallbackContext context) { OnMouseUp?.Invoke(); }
        private static void ConserveSize(InputAction.CallbackContext context) { OnConserveSize?.Invoke(); }
        private static void ConserveSizeCancel(InputAction.CallbackContext context) { OnConserveSizeCancel?.Invoke(); }
    }
}
