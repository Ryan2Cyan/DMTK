using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Input
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance;
        public delegate void DMTKInputAction();
        
        public static event DMTKInputAction OnMouseDown;
        public static event DMTKInputAction OnMouseUp;
        public static event DMTKInputAction OnMouseHold;
        public static event DMTKInputAction OnMouseHoldCancelled;
        public static event DMTKInputAction OnConserveSize;
        public static event DMTKInputAction OnConserveSizeCancel;
        public static event DMTKInputAction OnTabDown;
        public static event DMTKInputAction OnTabUp;
        
        public static Vector2 MousePosition;
        
        private DMTKActions _inputActions;

        [FormerlySerializedAs("UIInteraction")] public bool InteractionOccured;

        #region UnityFunctions
        
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
            _inputActions.DMTKPlayer.MouseDown.canceled += MouseUp;
            _inputActions.DMTKPlayer.ConservativeResize.performed += ConserveSize;
            _inputActions.DMTKPlayer.ConservativeResize.canceled += ConserveSizeCancel;
            _inputActions.DMTKPlayer.MouseHold.performed += MouseHold;
            _inputActions.DMTKPlayer.MouseHold.canceled += MouseHoldCancelled;
            _inputActions.DMTKPlayer.TabDown.performed += TabDown;
            _inputActions.DMTKPlayer.TabDown.canceled += TabUp;
        }

        private void OnDisable()
        {
            _inputActions.DMTKPlayer.Disable();
            
            // Unregister from input events:
            _inputActions.DMTKPlayer.MouseDown.performed -= MouseDown;
            _inputActions.DMTKPlayer.MouseDown.canceled -= MouseUp;
            _inputActions.DMTKPlayer.ConservativeResize.performed -= ConserveSize;
            _inputActions.DMTKPlayer.ConservativeResize.canceled -= ConserveSizeCancel;
            _inputActions.DMTKPlayer.MouseHold.canceled -= MouseHoldCancelled;
            _inputActions.DMTKPlayer.TabDown.performed -= TabDown;
            _inputActions.DMTKPlayer.TabDown.canceled -= TabUp;
        }

        private void Update()
        {
            MousePosition = _inputActions.DMTKPlayer.MousePosition.ReadValue<Vector2>();
        }
        
        #endregion

        #region EventInvokers
        
        private static void MouseDown(InputAction.CallbackContext context) { OnMouseDown?.Invoke(); }
        private static void MouseUp(InputAction.CallbackContext context) { OnMouseUp?.Invoke(); }
        private static void MouseHold(InputAction.CallbackContext context) { OnMouseHold?.Invoke(); }
        private static void MouseHoldCancelled(InputAction.CallbackContext context) { OnMouseHoldCancelled?.Invoke(); }
        private static void ConserveSize(InputAction.CallbackContext context) { OnConserveSize?.Invoke(); }
        private static void ConserveSizeCancel(InputAction.CallbackContext context) { OnConserveSizeCancel?.Invoke(); }
        private static void TabDown(InputAction.CallbackContext context) { OnTabDown?.Invoke(); }
        private static void TabUp(InputAction.CallbackContext context) { OnTabUp?.Invoke(); }
        
        #endregion

        #region PublicFunctions

        public static void UIInteractionToggle(bool toggle)
        {
            Instance.InteractionOccured = toggle;
        }

        #endregion
    }
}
