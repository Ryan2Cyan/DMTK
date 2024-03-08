using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputManager : MonoBehaviour
    {
        public InputManager Instance;
        
        public delegate void DMTKInputAction();
        
        public static event DMTKInputAction OnMouseDown;
        public static event DMTKInputAction OnConserverSize;
        
        private DMTKActions _inputActions;

        private void Awake()
        {
            Instance = this;
            _inputActions ??= new DMTKActions();
        }

        private void OnEnable()
        {
            _inputActions ??= new DMTKActions();
            _inputActions.DMTKPlayer.Enable();
            
            // Register to input events:
            _inputActions.DMTKPlayer.MouseDown.performed += MouseDown;
            _inputActions.DMTKPlayer.ConservativeResize.performed += ConserveSize;
        }

        private void OnDisable()
        {
            _inputActions.DMTKPlayer.Disable();
            
            // Unregister from input events:
            _inputActions.DMTKPlayer.MouseDown.performed -= MouseDown;
            _inputActions.DMTKPlayer.ConservativeResize.performed -= ConserveSize;
        }
        
        private static void MouseDown(InputAction.CallbackContext context) { OnMouseDown?.Invoke(); }
        private static void ConserveSize(InputAction.CallbackContext context) { OnConserverSize?.Invoke(); }
    }
}
