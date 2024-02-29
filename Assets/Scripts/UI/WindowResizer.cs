using System;
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

    // public class ResizerData
    // {
    //     public Vector2 WindowSize;
    //     public Vector2 WindowSize;
    // }
    
    /// <summary> A "resizer" is one of the small click-and-drag icons that appear when transforming an image.
    /// The resizer can adjust an image in a variety of directions depending on the type. Additionally it can
    /// change colour depending on whether it is pressed, highlighted, or un-highlighted.</summary>
    public class WindowResizer : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Required")]
        public ResizerType ResizerType;
        public RectTransform WindowRectTransform;
        public Image ResizerImage;
        public RectTransform ParentTransform;
        public Canvas ParentCanvas;

        [Header("Colour Settings")]
        public Color NotPressed;
        public Color Pressed;
        
        private Vector3 _mousePositionOnClick;
        private Vector3 _windowPositionOnClick;
        private Vector3 _resizeVector;
        private Vector2 _sizeDeltaOnClick;

        private bool _clicked;
        private bool _pointerPresent;

        private void Start()
        {
            TurnOff();
        }

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

        private void OnClick()
        {
            _mousePositionOnClick = Input.mousePosition / ParentCanvas.scaleFactor;
            _windowPositionOnClick = WindowRectTransform.localPosition;
            _sizeDeltaOnClick = WindowRectTransform.sizeDelta;
            ResizerImage.color = Pressed;
            _clicked = true;
        }

        private void TurnOff()
        {
            ResizerImage.enabled = false;
            _clicked = false;
            _pointerPresent = false;
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if (!_clicked) return;

            _resizeVector = (Input.mousePosition / ParentCanvas.scaleFactor) - _mousePositionOnClick;
            switch (ResizerType)
            {
                case ResizerType.HorizontalRight:
                {
                    WindowRectTransform.sizeDelta = new Vector2(_sizeDeltaOnClick.x + _resizeVector.x, WindowRectTransform.sizeDelta.y);
                    WindowRectTransform.localPosition = new Vector3(_windowPositionOnClick.x + _resizeVector.x / 2f , _windowPositionOnClick.y, _windowPositionOnClick.z);
                } break;
                // case ResizerType.HorizontalLeft:
                // {
                //     WindowRectTransform.sizeDelta = new Vector2(sizeDelta.x + resizeVector.x * -1, sizeDelta.y);
                //     WindowRectTransform.localPosition = new Vector3(localPosition.x + resizeVector.x / 2f, localPosition.y, localPosition.z);
                // } break;
                // case ResizerType.VerticalUp:
                // {
                //     WindowRectTransform.sizeDelta = new Vector2(sizeDelta.x,sizeDelta.y + resizeVector.y);
                //     WindowRectTransform.localPosition = new Vector3(localPosition.x, localPosition.y + resizeVector.y / 2f, localPosition.z);
                // } break;
                // case ResizerType.VerticalDown:
                // {
                //     WindowRectTransform.sizeDelta = new Vector2(sizeDelta.x, sizeDelta.y + resizeVector.y * -1);
                //     WindowRectTransform.localPosition = new Vector3(localPosition.x, localPosition.y + resizeVector.y / 2f, localPosition.z);
                // } break;
                // case ResizerType.DiagonalTopRight:
                // {
                //     WindowRectTransform.sizeDelta = new Vector2(sizeDelta.x + resizeVector.x, sizeDelta.y + resizeVector.y);
                //     WindowRectTransform.localPosition = new Vector3(localPosition.x + resizeVector.x / 2f, localPosition.y + resizeVector.y / 2f, localPosition.z);
                // }break;
                // case ResizerType.DiagonalBottomRight:
                // {
                //     WindowRectTransform.sizeDelta = new Vector2(sizeDelta.x + resizeVector.x, sizeDelta.y + resizeVector.y * -1);
                //     WindowRectTransform.localPosition = new Vector3(localPosition.x + resizeVector.x / 2f, localPosition.y + resizeVector.y / 2f, localPosition.z);
                // }break;
                // case ResizerType.DiagonalTopLeft:
                // {
                //     WindowRectTransform.sizeDelta = new Vector2(sizeDelta.x + resizeVector.x * -1, sizeDelta.y + resizeVector.y);
                //     WindowRectTransform.localPosition = new Vector3(localPosition.x + resizeVector.x / 2f, localPosition.y + resizeVector.y / 2f, localPosition.z);
                // } break;
                // case ResizerType.DiagonalBottomLeft:
                // {
                //     WindowRectTransform.sizeDelta = new Vector2(sizeDelta.x + resizeVector.x * -1, sizeDelta.y + resizeVector.y * -1);
                //     WindowRectTransform.localPosition = new Vector3(localPosition.x + resizeVector.x / 2f, localPosition.y + resizeVector.y / 2f, localPosition.z);
                // } break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}
