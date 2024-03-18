using System;
using Input;
using UnityEngine;
using UnityEngine.EventSystems;
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
    
    internal class OnClickData
    {
        public OnClickData(Vector2 mousePosition, Vector2 windowPosition, Vector2 sizeDelta, float heightRatio)
        {
            MousePosition = mousePosition;
            WindowPosition = windowPosition;
            SizeDelta = sizeDelta;
            HeightRatio = heightRatio;
        }
        
        public Vector2 MousePosition;
        public Vector2 WindowPosition;
        public Vector2 SizeDelta;
        public readonly float HeightRatio;
    }
    
    /// <summary>
    /// A "resizer" is one of the small click-and-drag icons that appear when transforming an image.
    /// The resizer can adjust an image in a variety of directions depending on the type. Additionally it can
    /// change colour depending on whether it is pressed, highlighted, or un-highlighted.
    /// </summary>
    public class WindowResizer : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Settings")]
        public ResizerType ResizerType;
        public Image ResizerImage;
        
        [Header("Colour Settings")]
        public Color NotPressed;
        public Color Pressed;

        private Window.Window _window;
        
        private OnClickData _onClickData;
        private RectTransform _windowRectTransform;
        private Canvas _canvas;
        
        private bool _clicked;
        private bool _pointerPresent;
        private bool _conserveResize;

        #region UnityStateFunctions

        private void Start()
        {
            _window = GetComponentInParent<Window.Window>();
            _windowRectTransform = _window.GetRectTransform();
            _canvas = _window.GetCanvas();
        }

        private void OnEnable()
        {
            TurnOff();
            InputManager.OnConserveSize += OnConserveResize;
            InputManager.OnConserveSizeCancel += OnConserveResizeCancelled;
        }

        private void OnDisable()
        {
            InputManager.OnConserveSize -= OnConserveResize;
            InputManager.OnConserveSizeCancel -= OnConserveResizeCancelled;
        }

        #endregion

        #region UnityInteractionFunctions
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_window.FixedSize) return;
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
            
            // Calculate how much the window will be adjusted:
            var resizeVector = InputManager.MousePosition / _canvas.scaleFactor - _onClickData.MousePosition;
            
            // If conserve resize: change y-value to keep window's original aspect ratio:
            if (_conserveResize)
            {
                resizeVector.y = resizeVector.x * _onClickData.HeightRatio;
                if (ResizerType is ResizerType.DiagonalBottomRight or ResizerType.DiagonalTopLeft) resizeVector.y *= -1;
            }
            
            // Store pre-change values (in case they need to be reverted later):
            var sizeDeltaBefore = _windowRectTransform.sizeDelta;
            var localPositionBefore = _windowRectTransform.localPosition;
            
            // Adjust the window's size depending on the resizer type:
            switch (ResizerType)
            {
                case ResizerType.HorizontalRight:
                {
                    _windowRectTransform.sizeDelta = new Vector2(_onClickData.SizeDelta.x + resizeVector.x, _windowRectTransform.sizeDelta.y);
                    _windowRectTransform.localPosition = new Vector3(_onClickData.WindowPosition.x + resizeVector.x / 2f , _onClickData.WindowPosition.y, _windowRectTransform.localPosition.z);
                } break;
                case ResizerType.HorizontalLeft:
                {
                    _windowRectTransform.sizeDelta = new Vector2(_onClickData.SizeDelta.x + resizeVector.x * -1, _onClickData.SizeDelta.y);
                    _windowRectTransform.localPosition = new Vector3(_onClickData.WindowPosition.x + resizeVector.x / 2f, _onClickData.WindowPosition.y, _windowRectTransform.localPosition.z);
                } break;
                case ResizerType.VerticalUp:
                {
                    _windowRectTransform.sizeDelta = new Vector2(_onClickData.SizeDelta.x,_onClickData.SizeDelta.y + resizeVector.y);
                    _windowRectTransform.localPosition = new Vector3(_onClickData.WindowPosition.x, _onClickData.WindowPosition.y + resizeVector.y / 2f, _windowRectTransform.localPosition.z);
                } break;
                case ResizerType.VerticalDown:
                {
                    _windowRectTransform.sizeDelta = new Vector2(_onClickData.SizeDelta.x, _onClickData.SizeDelta.y + resizeVector.y * -1);
                    _windowRectTransform.localPosition = new Vector3(_onClickData.WindowPosition.x, _onClickData.WindowPosition.y + resizeVector.y / 2f, _windowRectTransform.localPosition.z);
                } break;
                case ResizerType.DiagonalTopRight:
                {
                    _windowRectTransform.sizeDelta = new Vector2(_onClickData.SizeDelta.x + resizeVector.x, _onClickData.SizeDelta.y + resizeVector.y);
                    _windowRectTransform.localPosition = new Vector3(_onClickData.WindowPosition.x + resizeVector.x / 2f, _onClickData.WindowPosition.y + resizeVector.y / 2f, _windowRectTransform.localPosition.z);
                }break;
                case ResizerType.DiagonalBottomRight:
                {
                    _windowRectTransform.sizeDelta = new Vector2(_onClickData.SizeDelta.x + resizeVector.x, _onClickData.SizeDelta.y + resizeVector.y * -1);
                    _windowRectTransform.localPosition = new Vector3(_onClickData.WindowPosition.x + resizeVector.x / 2f, _onClickData.WindowPosition.y + resizeVector.y / 2f, _windowRectTransform.localPosition.z);
                }break;
                case ResizerType.DiagonalTopLeft:
                {
                    _windowRectTransform.sizeDelta = new Vector2(_onClickData.SizeDelta.x + resizeVector.x * -1, _onClickData.SizeDelta.y + resizeVector.y);
                    _windowRectTransform.localPosition = new Vector3(_onClickData.WindowPosition.x + resizeVector.x / 2f, _onClickData.WindowPosition.y + resizeVector.y / 2f, _windowRectTransform.localPosition.z);
                } break;
                case ResizerType.DiagonalBottomLeft:
                {
                    _windowRectTransform.sizeDelta = new Vector2(_onClickData.SizeDelta.x + resizeVector.x * -1, _onClickData.SizeDelta.y + resizeVector.y * -1);
                    _windowRectTransform.localPosition = new Vector3(_onClickData.WindowPosition.x + resizeVector.x / 2f, _onClickData.WindowPosition.y + resizeVector.y / 2f, _windowRectTransform.localPosition.z);
                } break;
                
                default: throw new ArgumentOutOfRangeException();
            }
            
            // Prevent resize if the window is below minimum size:
            var localPosition = _windowRectTransform.localPosition;
            if (_windowRectTransform.sizeDelta.x / _canvas.scaleFactor < _window.MinimumSize.x)
            {
                _windowRectTransform.sizeDelta = new Vector2(sizeDeltaBefore.x, _windowRectTransform.sizeDelta.y);
                localPosition = new Vector3(localPositionBefore.x, localPosition.y, localPosition.z);
                _windowRectTransform.localPosition = localPosition;
            }
            if (_windowRectTransform.sizeDelta.y / _canvas.scaleFactor < _window.MinimumSize.y)
            {
                _windowRectTransform.sizeDelta = new Vector2(_windowRectTransform.sizeDelta.x, sizeDeltaBefore.y);
                _windowRectTransform.localPosition = new Vector3(localPosition.x, localPositionBefore.y, localPosition.z);
            }
        }
        
        #endregion

        #region UtilityFunctions

        // Store all data about the image when the resizer is clicked. Divide by scale factor to convert mouse
        // positions to canvas space.
        private void OnClick()
        {
            var SizeDelta = _windowRectTransform.sizeDelta;
            _onClickData = new OnClickData(
                InputManager.MousePosition / _canvas.scaleFactor,
                _windowRectTransform.localPosition,
                SizeDelta,
                SizeDelta.y / SizeDelta.x);
            ResizerImage.color = Pressed;
            _clicked = true;
        }

        // Disable resizer.
        private void TurnOff()
        {
            ResizerImage.enabled = false;
            _clicked = false;
            _pointerPresent = false;
        }

        #endregion

        #region EventFunctions

        private void OnConserveResize()
        {
            _conserveResize = true;
        }
        
        private void OnConserveResizeCancelled()
        {
            _conserveResize = false;
        }

        #endregion
    }
}
