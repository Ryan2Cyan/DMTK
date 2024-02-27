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
    
    /// <summary> A "resizer" is one of the small click-and-drag icons that appear when transforming an image.
    /// The resizer can adjust an image in a variety of directions depending on the type. Additionally it can
    /// change colour depending on whether it is pressed, highlighted, or un-highlighted.</summary>
    public class WindowResizer : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Required")]
        public ResizerType ResizerType;
        public RectTransform WindowRectTransform;
        public Image ResizerImage;
        
        [Header("Colour Settings")]
        
        public Color Unhighlighted;
        public Color Highlighted;
        public Color Pressed;
        
        private Vector3 _mouseClickStartPosition;
        private bool _pressed;
        private bool _cursorPresent;

        private void Start()
        {
            ResizerImage.color = Unhighlighted;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_pressed) return;
            
            var distance = Input.mousePosition - _mouseClickStartPosition;
            var localPosition = WindowRectTransform.localPosition;
            var sizeDelta = WindowRectTransform.sizeDelta;
            switch (ResizerType)
            {
                case ResizerType.HorizontalRight:
                {
                    WindowRectTransform.sizeDelta = new Vector2(sizeDelta.x + distance.x, sizeDelta.y);
                    WindowRectTransform.localPosition = new Vector3(localPosition.x + distance.x / 2f, localPosition.y, localPosition.z);
                } break;
                case ResizerType.HorizontalLeft:
                {
                    WindowRectTransform.sizeDelta = new Vector2(sizeDelta.x + distance.x * -1, sizeDelta.y);
                    WindowRectTransform.localPosition = new Vector3(localPosition.x + distance.x / 2f, localPosition.y, localPosition.z);
                } break;
                case ResizerType.VerticalUp:
                {
                    WindowRectTransform.sizeDelta = new Vector2(sizeDelta.x,sizeDelta.y + distance.y);
                    WindowRectTransform.localPosition = new Vector3(localPosition.x, localPosition.y + distance.y / 2f, localPosition.z);
                } break;
                case ResizerType.VerticalDown:
                {
                    WindowRectTransform.sizeDelta = new Vector2(sizeDelta.x, sizeDelta.y + distance.y * -1);
                    WindowRectTransform.localPosition = new Vector3(localPosition.x, localPosition.y + distance.y / 2f, localPosition.z);
                } break;
                case ResizerType.DiagonalTopRight:
                {
                    WindowRectTransform.sizeDelta = new Vector2(sizeDelta.x + distance.x, sizeDelta.y + distance.y);
                    WindowRectTransform.localPosition = new Vector3(localPosition.x + distance.x / 2f, localPosition.y + distance.y / 2f, localPosition.z);
                }break;
                case ResizerType.DiagonalBottomRight:
                {
                    WindowRectTransform.sizeDelta = new Vector2(sizeDelta.x + distance.x, sizeDelta.y + distance.y * -1);
                    WindowRectTransform.localPosition = new Vector3(localPosition.x + distance.x / 2f, localPosition.y + distance.y / 2f, localPosition.z);
                }break;
                case ResizerType.DiagonalTopLeft:
                {
                    WindowRectTransform.sizeDelta = new Vector2(sizeDelta.x + distance.x * -1, sizeDelta.y + distance.y);
                    WindowRectTransform.localPosition = new Vector3(localPosition.x + distance.x / 2f, localPosition.y + distance.y / 2f, localPosition.z);
                } break;
                case ResizerType.DiagonalBottomLeft:
                {
                    WindowRectTransform.sizeDelta = new Vector2(sizeDelta.x + distance.x * -1, sizeDelta.y + distance.y * -1);
                    WindowRectTransform.localPosition = new Vector3(localPosition.x + distance.x / 2f, localPosition.y + distance.y / 2f, localPosition.z);
                } break;
                default: throw new ArgumentOutOfRangeException();
            }
            _mouseClickStartPosition = Input.mousePosition;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _mouseClickStartPosition = Input.mousePosition;
            _pressed = true;
            ResizerImage.color = Pressed;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _pressed = false;
            ResizerImage.color = _cursorPresent ? Highlighted : Unhighlighted;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _cursorPresent = true;
            if (_pressed) return;
            ResizerImage.color = Highlighted;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _cursorPresent = false;
            if (_pressed) return;
            ResizerImage.color = Unhighlighted;
        }
    }
}
