using System.Collections.Generic;
using Input;
using UnityEngine;

namespace UI.Utility
{
    [ExecuteAlways]
    public class RectOverflow : MonoBehaviour, UIElement
    {
        [Header("Components")] 
        public RectTransform ParentRectTransform;
        public List<RectTransform> Elements;
        public Canvas ParentCanvas;

        [Header("Element Settings")]
        public Vector2 ElementSize;
        public Vector2 Spacing;

        [Header("Vertical Scrolling Settings")] 
        public RectTransform ViewPort;
        public Transform Alignment;
        public float ScrollSpeed;
        public float ScrollPadding;
        public bool EnableScrolling;
        
        
        private readonly Vector3[] _rectCorners = new Vector3[4];
        private Vector2 _bounds;
        private Vector2 _maxElementSize;
        private Vector2 _topLeft;
        private float _targetYTopPos;
        private float _parentStartPosition;
        private float _maxHeight;
        private float _yScrollOffset;
        
        public bool UIElementActive { get; set; }
        public int UIElementPriority { get; set; }

        [Header("Vertical Scaling Settings")] 
        public bool ScaleYToMaxRowCount;

        [ContextMenu("SetStartPosition")]
        public void SetAlignment()
        {
            if (Alignment == null) return;
            _parentStartPosition = ParentRectTransform.localPosition.y;
            _targetYTopPos = Alignment.localPosition.y;
        }
        

        #region UnityFunctions

        private void Awake()
        {
            UIElementPriority = 1;
            CalculateOverflow();
        }

        private void OnEnable()
        {
            UIElementActive = true;
            InputManager.OnMouseScroll += OnMouseScroll;
        }

        private void OnDisable()
        {
            UIElementActive = false;
            InputManager.OnMouseScroll -= OnMouseScroll;
        }

        private void Update()
        {
            if (ParentCanvas == null) return;
            
            // Calculate bounds:
            var previousBounds = _bounds;
            ParentRectTransform.GetWorldCorners(_rectCorners);
            _bounds = new Vector2(_rectCorners[3].x - _rectCorners[0].x, _rectCorners[1].y - _rectCorners[0].y) / ParentCanvas.scaleFactor;
            _topLeft = _rectCorners[1];
            
            // Calculate total size of an element:
            var previousMaxSize = _maxElementSize;
            _maxElementSize = ElementSize + Spacing;
            if (Vector2.Distance(previousBounds, _bounds) < 0.1f && Vector2.Distance(previousMaxSize, _maxElementSize) < 0.1f) return;
            
            // Resize rect and order elements:
            _yScrollOffset = 0f;
            CalculateOverflow();
        }

        #endregion

        #region PublicFunctions
        public void CalculateOverflow()
        {
            SetAlignment();

            if (Elements.Count == 0) return;
            
            // Calculate dimensions:
            var halfElementSize = _maxElementSize / 2f;
            var dimensions = new Vector2Int(ScaleYToMaxRowCount ? int.MaxValue : (int)(_bounds.y / halfElementSize.y), (int)(_bounds.x / halfElementSize.x));
            if (dimensions.x < 0 || dimensions.y < 0) return;
            ScaleWindowToRows();
            
            // Place all elements within confines of the dimensions:
            var elementIndex = 0;
            var currentElementPosition = _topLeft;
            _maxHeight = _maxElementSize.y;
            
            for (var i = 0; i < dimensions.x; i++)
            {
                // Reset row position:
                currentElementPosition.x = _topLeft.x;
                if (dimensions.y == 0) return;
                for (var j = 0; j < dimensions.y; j++)
                {
                    // Position element in rect:
                    var currentElement = Elements[elementIndex];
                    currentElement.position = currentElementPosition;
                    currentElement.sizeDelta = ElementSize;
                    currentElement.gameObject.SetActive(true);
                    elementIndex++;
                 
                    // Increment row:
                    currentElementPosition.x += halfElementSize.x * ParentCanvas.scaleFactor;
                    
                    // Increment column: 
                    if (j == dimensions.y - 1)
                    {
                        currentElementPosition.y -= halfElementSize.y * ParentCanvas.scaleFactor;
                        _maxHeight += _maxElementSize.y;
                    }
                    
                    // All elements are organised, exit:
                    if (elementIndex < Elements.Count) continue;
                    DeactivateRemainingElements();
                    return;
                }
            }

            // Filled rect, exit:
            DeactivateRemainingElements();
            return;

            void DeactivateRemainingElements()
            {
                for (var i = elementIndex; i < Elements.Count; i++) Elements[i].gameObject.SetActive(false);
            }
            
            void ScaleWindowToRows()
            {
                if (!ScaleYToMaxRowCount) return;
                
                // Set scale to height of combined rows:
                ParentRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _maxHeight == 0 ? 1 : _maxHeight * ParentCanvas.scaleFactor);
                    
                // Move rect to align with y-axis point:
                ParentRectTransform.GetLocalCorners(_rectCorners);
                var newTop = _parentStartPosition + _rectCorners[1].y;
                var position1 = ParentRectTransform.localPosition;
                var moveVector = newTop - _targetYTopPos;
                position1 = new Vector3(position1.x,  _parentStartPosition - moveVector + _yScrollOffset, position1.z);
                ParentRectTransform.localPosition = position1;
            }
        }
        
        #endregion
        
        #region PrivateFunctions
        
        private void OnMouseScroll()
        {
            if (!EnableScrolling) return;
            
            // Calculate how much distance can be scrolled:
            ViewPort.GetWorldCorners(_rectCorners);
            var maxScrollHeight = _bounds.y - (_rectCorners[1].y - _rectCorners[0].y) * 2f + ScrollPadding;
            if (maxScrollHeight <= 0f) return;
            
            // Increment scroll:
            _yScrollOffset += -Mathf.Sign(InputManager.MouseScroll) * (ScrollSpeed * Time.deltaTime);
            
            if (_yScrollOffset <= 0f) _yScrollOffset = 0; 
            else _yScrollOffset = _yScrollOffset > maxScrollHeight ? maxScrollHeight : _yScrollOffset;
            
            // Reposition elements:
            CalculateOverflow();
        }
        
        #endregion
        
        public void OnMouseDown() { }
        public void OnMouseUp() { }

        public void OnMouseEnter()
        {
            EnableScrolling = true;
        }

        public void OnMouseExit()
        {
            EnableScrolling = false;
        }
        public void OnDrag() { }
    }
}
