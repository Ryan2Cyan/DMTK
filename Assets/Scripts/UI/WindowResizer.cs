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
        public Color NotPressed;
        public Color Pressed;

        private enum State
        {
            SelectedCursorPresent,
            SelectedCursorNotPresent,
            Idle,
            Disabled
        }
        private State _currentState;
        private Vector3 _mouseClickStartPosition;

        private void Start()
        {
            ChangeState(State.Disabled);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_currentState != State.SelectedCursorPresent && _currentState != State.SelectedCursorNotPresent) return;
            
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
        public void OnPointerEnter(PointerEventData eventData)
        {
            if(_currentState == State.Disabled) ChangeState(State.Idle);
            if(_currentState == State.SelectedCursorNotPresent) ChangeState(State.SelectedCursorPresent);
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            ChangeState(_currentState == State.SelectedCursorPresent ? State.SelectedCursorNotPresent : State.Disabled);
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (_currentState != State.Idle) return;
            ChangeState(State.SelectedCursorPresent);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            ChangeState(_currentState == State.SelectedCursorPresent ? State.Idle : State.Disabled);
        }

        private void ChangeState(State state)
        {
            switch (state)
            {
                case State.SelectedCursorPresent:
                {
                    _mouseClickStartPosition = Input.mousePosition;
                    ResizerImage.color = Pressed;   
                } break;
                case State.SelectedCursorNotPresent:
                {
                    
                } break;
                case State.Idle:
                {
                    ResizerImage.enabled = true;
                    ResizerImage.color = NotPressed; 
                } break;
                case State.Disabled:
                {
                    ResizerImage.enabled = false;  
                } break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
            _currentState = state;
        }
    }
}
