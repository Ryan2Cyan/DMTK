using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public enum ResizerType
    {
        HorizontalRight,
        HorizontalLeft,
        VerticalUp,
        VerticalDown
    }
    
    public class WindowResizer : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        public ResizerType ResizerType;
        public RectTransform WindowRectTransform;
        private Vector3 _mouseClickPosition;
        private bool _pressed;

        public void OnDrag(PointerEventData eventData)
        {
            if (!_pressed) return;
            
            var distance = Input.mousePosition - _mouseClickPosition;
            var localPosition = WindowRectTransform.localPosition;
            var sizeDelta = WindowRectTransform.sizeDelta;
            switch (ResizerType)
            {
                case ResizerType.HorizontalRight:
                {
                    sizeDelta = new Vector2(sizeDelta.x + distance.x * 2, sizeDelta.y);
                    WindowRectTransform.sizeDelta = sizeDelta;
                    WindowRectTransform.localPosition = new Vector3(localPosition.x + distance.x, localPosition.y, localPosition.z);
                } break;
                case ResizerType.HorizontalLeft:
                {
                    WindowRectTransform.sizeDelta = new Vector2(-distance.x * 2f, WindowRectTransform.sizeDelta.y);
                    WindowRectTransform.localPosition = new Vector3(localPosition.x - distance.x, localPosition.y, localPosition.z);
                } break;
                case ResizerType.VerticalUp:
                {
                    WindowRectTransform.sizeDelta = new Vector2(WindowRectTransform.sizeDelta.x,distance.y * 2f);
                    WindowRectTransform.localPosition = new Vector3(localPosition.x, localPosition.y + distance.y, localPosition.z);
                } break;
                case ResizerType.VerticalDown:
                {
                    WindowRectTransform.sizeDelta = new Vector2(WindowRectTransform.sizeDelta.x,-distance.y * 2f);
                    WindowRectTransform.localPosition = new Vector3(localPosition.x, localPosition.y - distance.y, localPosition.z);
                } break;
                default: throw new ArgumentOutOfRangeException();
            }
            _mouseClickPosition = Input.mousePosition;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _mouseClickPosition = Input.mousePosition;
            _pressed = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _pressed = false;
        }
    }
}
