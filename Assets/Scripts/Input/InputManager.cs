using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputManager : MonoBehaviour
    {
        public delegate void DMTKInputAction();
        
        public static event DMTKInputAction OnMouseDown;
        public static event DMTKInputAction OnMouseUp;
        public static event DMTKInputAction OnConserverSize;
        
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
        }

        private void OnDisable()
        {
            _inputActions.DMTKPlayer.Disable();
            
            // Unregister from input events:
            _inputActions.DMTKPlayer.MouseDown.performed -= MouseDown;
            _inputActions.DMTKPlayer.MouseDown.canceled -= MouseUp;
            _inputActions.DMTKPlayer.ConservativeResize.performed -= ConserveSize;
        }
        
        private static void MouseDown(InputAction.CallbackContext context) { OnMouseDown?.Invoke(); }
        private static void MouseUp(InputAction.CallbackContext context) { OnMouseUp?.Invoke(); }
        private static void ConserveSize(InputAction.CallbackContext context) { OnConserverSize?.Invoke(); }
    }
}
