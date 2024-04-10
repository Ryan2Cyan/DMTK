using System;
using Input;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Window
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
    
    /// <summary>A "resizer" is a small click-and-drag icon that appear when transforming an DMTK window. The resizer can adjust a
    /// window depending on the resizer type. Also changes colour depending on whether it is pressed, highlighted, or un-highlighted.</summary>
    public class WindowResizer : UIElement, IInputElement
    {
        [Header("Settings")]
        public ResizerType ResizerType;
        public Image ResizerImage;
        
        [Header("Colour Settings")]
        public Color NotPressed;
        public Color Pressed;

        private Window _window;
        private OnClickData _onClickData;
        private RectTransform _windowRectTransform;
        private Canvas _canvas;
        
        public bool _clicked;
        private bool _conserveResize;
        private bool _active;

        #region UnityStateFunctions

        private void Start()
        {
            _window = GetComponentInParent<Window>();
            _windowRectTransform = _window.GetRectTransform();
            _canvas = _window.GetCanvas();
            
            // Clamp to minimum size is overflowing:
            if (_windowRectTransform.sizeDelta.x / _canvas.scaleFactor < _window.MinimumSize.x / _canvas.scaleFactor)
            {
                _windowRectTransform.sizeDelta = new Vector2(_window.MinimumSize.x, _windowRectTransform.sizeDelta.y);
            }
            if (_windowRectTransform.sizeDelta.y / _canvas.scaleFactor < _window.MinimumSize.y / _canvas.scaleFactor)
            {
                _windowRectTransform.sizeDelta = new Vector2(_windowRectTransform.sizeDelta.x, _window.MinimumSize.y);
            }

            if (!_window.FixedSize) return;
            enabled = false;
            _active = false;
        }

        private void OnEnable()
        {
            ResizerImage.enabled = false;
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
        public void OnMouseEnter()
        {
            if (!_active) return;
            
            ResizerImage.enabled = true;
            ResizerImage.color = NotPressed;
            _clicked = false;
        }
        
        public void OnMouseExit()
        {
            if (!_active) return;
            
            ResizerImage.enabled = false;
        }
        
        public void OnMouseDown()
        {
            if (!_active) return;
            
            var SizeDelta = _windowRectTransform.sizeDelta;
            _onClickData = new OnClickData(
                InputManager.MousePosition /  _canvas.scaleFactor,
                _windowRectTransform.localPosition,
                SizeDelta,
                SizeDelta.y / SizeDelta.x);
            ResizerImage.color = Pressed;
            _clicked = true;
        }

        public void OnMouseUp()
        {
            if (!_active) return;
            
            ResizerImage.enabled = true;
            ResizerImage.color = NotPressed;
            _clicked = false;
        }
        
        public void OnDrag()
        {
            if (!_active) return;
            
            if (!_clicked) return;
            
            // Calculate how much the window will be adjusted:
            var resizeVector = InputManager.MousePosition / _canvas.scaleFactor - _onClickData.MousePosition;
            
            // If conserve resize: change y-value to keep window's original aspect ratio:
            if (_conserveResize)
            {
                if (ResizerType is ResizerType.DiagonalTopLeft or ResizerType.DiagonalTopRight or ResizerType.DiagonalBottomRight or ResizerType.DiagonalBottomLeft)
                {
                    resizeVector.y = resizeVector.x * _onClickData.HeightRatio;
                    if (ResizerType is ResizerType.DiagonalBottomRight or ResizerType.DiagonalTopLeft) resizeVector.y *= -1;
                } 
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
            if (_windowRectTransform.sizeDelta.x / _canvas.scaleFactor < _window.MinimumSize.x / _canvas.scaleFactor)
            {
                _windowRectTransform.sizeDelta = new Vector2(sizeDeltaBefore.x, _windowRectTransform.sizeDelta.y);
                localPosition = new Vector3(localPositionBefore.x, localPosition.y, localPosition.z);
                _windowRectTransform.localPosition = localPosition;
            }
            if (_windowRectTransform.sizeDelta.y / _canvas.scaleFactor < _window.MinimumSize.y / _canvas.scaleFactor)
            {
                _windowRectTransform.sizeDelta = new Vector2(_windowRectTransform.sizeDelta.x, sizeDeltaBefore.y);
                _windowRectTransform.localPosition = new Vector3(localPosition.x, localPositionBefore.y, localPosition.z);
            }
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
