using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public enum ResizerType
    {
        HorizontalRight,
        HorizontalLeft,
        VerticalUp,
        VerticalDown,
        DiagonalTopRight,
        DiagonalBottomRight,
        DiagonalTopLeft,
        DiagonalBottomLeft
    }

    /// <summary>
    /// A "resizer" is one of the small click-and-drag icons that appear when transforming an image.
    /// The resizer can adjust an image in a variety of directions depending on the type. Additionally it can
    /// change colour depending on whether it is pressed, highlighted, or un-highlighted.
    /// </summary>
    public class WindowResizer : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Required")]
        public ResizerType ResizerType;
        public RectTransform WindowRectTransform;
        public Image ResizerImage;
        public Canvas ParentCanvas;
        
        [Header("Colour Settings")]
        public Color NotPressed;
        public Color Pressed;
        
        private Vector3 _mousePositionOnClick;
        private Vector3 _windowPositionOnClick;
        private Vector3 _resizeVector;
        private Vector2 _sizeDeltaOnClick;
        private readonly Vector2 _minimumWindowSize = new Vector2(100f, 100f);
        private float _heightRatioOnClick;

        private bool _clicked;
        private bool _pointerPresent;
        private bool _conserveResize;
        private DMTKActions _inputActions;

        #region UnityStateFunctions

        private void OnEnable()
        {
            _inputActions = new DMTKActions();
            _inputActions.DMTKPlayer.Enable();
            _inputActions.DMTKPlayer.ConservativeResize.canceled += OnConserveResizeCancelled;
            _inputActions.DMTKPlayer.ConservativeResize.performed += OnConserveResize;
            TurnOff();
        }

        private void OnDisable()
        {
            _inputActions.DMTKPlayer.ConservativeResize.performed -= OnConserveResize;
            _inputActions.DMTKPlayer.ConservativeResize.canceled -= OnConserveResizeCancelled;
        }

        #endregion

        #region UnityInteractionFunctions
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_clicked) return;
            if (_pointerPresent) return;

            ResizerImage.enabled = true;
            ResizerImage.color = NotPressed;
            _pointerPresent = true;
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            _pointerPresent = false;
            if (_clicked) return;
            TurnOff();
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_pointerPresent) return;
            OnClick();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_pointerPresent)
            {
                ResizerImage.enabled = true;
                ResizerImage.color = NotPressed;
            }
            else TurnOff();
            _clicked = false;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if (!_clicked) return;
            
            _resizeVector = Input.mousePosition / ParentCanvas.scaleFactor - _mousePositionOnClick;
            
            // If holding shift, convert the y-value of the resize vector to keep the resize coherent with the window's
            // original aspect ration:
            if (_conserveResize)
            {
                _resizeVector.y = _resizeVector.x * _heightRatioOnClick;
                if (ResizerType is ResizerType.DiagonalBottomRight or ResizerType.DiagonalTopLeft) _resizeVector.y *= -1;
            }
            
            // Store current values in case they need to be reverted later:
            var sizeDeltaBefore = WindowRectTransform.sizeDelta;
            var localPositionBefore = WindowRectTransform.localPosition;
            
            switch (ResizerType)
            {
                case ResizerType.HorizontalRight:
                {
                    WindowRectTransform.sizeDelta = new Vector2(_sizeDeltaOnClick.x + _resizeVector.x, WindowRectTransform.sizeDelta.y);
                    WindowRectTransform.localPosition = new Vector3(_windowPositionOnClick.x + _resizeVector.x / 2f , _windowPositionOnClick.y, _windowPositionOnClick.z);
                } break;
                case ResizerType.HorizontalLeft:
                {
                    WindowRectTransform.sizeDelta = new Vector2(_sizeDeltaOnClick.x + _resizeVector.x * -1, _sizeDeltaOnClick.y);
                    WindowRectTransform.localPosition = new Vector3(_windowPositionOnClick.x + _resizeVector.x / 2f, _windowPositionOnClick.y, _windowPositionOnClick.z);
                } break;
                case ResizerType.VerticalUp:
                {
                    WindowRectTransform.sizeDelta = new Vector2(_sizeDeltaOnClick.x,_sizeDeltaOnClick.y + _resizeVector.y);
                    WindowRectTransform.localPosition = new Vector3(_windowPositionOnClick.x, _windowPositionOnClick.y + _resizeVector.y / 2f, _windowPositionOnClick.z);
                } break;
                case ResizerType.VerticalDown:
                {
                    WindowRectTransform.sizeDelta = new Vector2(_sizeDeltaOnClick.x, _sizeDeltaOnClick.y + _resizeVector.y * -1);
                    WindowRectTransform.localPosition = new Vector3(_windowPositionOnClick.x, _windowPositionOnClick.y + _resizeVector.y / 2f, _windowPositionOnClick.z);
                } break;
                case ResizerType.DiagonalTopRight:
                {
                    WindowRectTransform.sizeDelta = new Vector2(_sizeDeltaOnClick.x + _resizeVector.x, _sizeDeltaOnClick.y + _resizeVector.y);
                    WindowRectTransform.localPosition = new Vector3(_windowPositionOnClick.x + _resizeVector.x / 2f, _windowPositionOnClick.y + _resizeVector.y / 2f, _windowPositionOnClick.z);
                }break;
                case ResizerType.DiagonalBottomRight:
                {
                    WindowRectTransform.sizeDelta = new Vector2(_sizeDeltaOnClick.x + _resizeVector.x, _sizeDeltaOnClick.y + _resizeVector.y * -1);
                    WindowRectTransform.localPosition = new Vector3(_windowPositionOnClick.x + _resizeVector.x / 2f, _windowPositionOnClick.y + _resizeVector.y / 2f, _windowPositionOnClick.z);
                }break;
                case ResizerType.DiagonalTopLeft:
                {
                    WindowRectTransform.sizeDelta = new Vector2(_sizeDeltaOnClick.x + _resizeVector.x * -1, _sizeDeltaOnClick.y + _resizeVector.y);
                    WindowRectTransform.localPosition = new Vector3(_windowPositionOnClick.x + _resizeVector.x / 2f, _windowPositionOnClick.y + _resizeVector.y / 2f, _windowPositionOnClick.z);
                } break;
                case ResizerType.DiagonalBottomLeft:
                {
                    WindowRectTransform.sizeDelta = new Vector2(_sizeDeltaOnClick.x + _resizeVector.x * -1, _sizeDeltaOnClick.y + _resizeVector.y * -1);
                    WindowRectTransform.localPosition = new Vector3(_windowPositionOnClick.x + _resizeVector.x / 2f, _windowPositionOnClick.y + _resizeVector.y / 2f, _windowPositionOnClick.z);
                } break;
                
                default: throw new ArgumentOutOfRangeException();
            }
            
            // Revert resize if the window is below minimum size:
            var localPosition = WindowRectTransform.localPosition;
            if (WindowRectTransform.sizeDelta.x < _minimumWindowSize.x)
            {
                WindowRectTransform.sizeDelta = new Vector2(sizeDeltaBefore.x, WindowRectTransform.sizeDelta.y);
                localPosition = new Vector3(localPositionBefore.x, localPosition.y, localPosition.z);
                WindowRectTransform.localPosition = localPosition;
            }

            if (WindowRectTransform.sizeDelta.y < _minimumWindowSize.y)
            {
                WindowRectTransform.sizeDelta = new Vector2(WindowRectTransform.sizeDelta.x, sizeDeltaBefore.y);
                WindowRectTransform.localPosition = new Vector3(localPosition.x, localPositionBefore.y, localPosition.z);
            }
        }
        
        #endregion

        #region UtilityFunctions

        private void OnClick()
        {
            // Store all data about the image when the resizer is clicked. Divide by scale factor to convert mouse
            // positions to canvas space:
            _mousePositionOnClick = Input.mousePosition / ParentCanvas.scaleFactor;
            _windowPositionOnClick = WindowRectTransform.localPosition;
            _sizeDeltaOnClick = WindowRectTransform.sizeDelta;
            _heightRatioOnClick = _sizeDeltaOnClick.y / _sizeDeltaOnClick.x;
            ResizerImage.color = Pressed;
            _clicked = true;
        }

        private void TurnOff()
        {
            ResizerImage.enabled = false;
            _clicked = false;
            _pointerPresent = false;
        }

        #endregion

        #region EventFunctions

        private void OnConserveResize(InputAction.CallbackContext context)
        {
            _conserveResize = true;
        }
        
        private void OnConserveResizeCancelled(InputAction.CallbackContext context)
        {
            _conserveResize = false;
        }

        #endregion
    }
}
