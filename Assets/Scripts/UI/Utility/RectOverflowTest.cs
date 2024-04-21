using System.Collections.Generic;
using UnityEngine;

namespace UI.Utility
{
    [ExecuteAlways]
    public class RectOverflowTest : MonoBehaviour
    {
        [Header("Components")] 
        public RectTransform ParentRectTransform;
        public List<RectTransform> Elements;
        public Canvas ParentCanvas;
        public Transform Alignment;

        [Header("Element Settings")]
        public Vector2 ElementSize;
        public Vector2 Spacing;
        
        
        private readonly Vector3[] _elementCorners = new Vector3[4];
        private readonly Vector3[] _boundsCorners = new Vector3[4];
        private Vector2 _bounds;
        private Vector2 _maxElementSize;
        private Vector2 _topLeft;
        private float _targetYTopPos;
        private float _parentStartPosition;
        private float _maxHeight;

        [Header("Settings")] 
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
            SetAlignment();
        }

        private void Update()
        {
            if (ParentCanvas == null) return;
            
            // Calculate bounds:
            var previousBounds = _bounds;
            ParentRectTransform.GetWorldCorners(_boundsCorners);
            _bounds = new Vector2(_boundsCorners[3].x - _boundsCorners[0].x, _boundsCorners[1].y - _boundsCorners[0].y) / ParentCanvas.scaleFactor;
            _topLeft = _boundsCorners[1];
            
            // Calculate total size of an element:
            var previousMaxSize = _maxElementSize;
            _maxElementSize = ElementSize + Spacing;
            
            if(previousBounds != _bounds || previousMaxSize != _maxElementSize) CalculateOverflow();
        }

        #endregion

        #region PrivateFunctions
        
        private void CalculateOverflow()
        {
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
                ParentRectTransform.GetLocalCorners(_elementCorners);
                var newTop = _parentStartPosition + _elementCorners[1].y;
                var position1 = ParentRectTransform.localPosition;
                var moveVector = newTop - _targetYTopPos;
                position1 = new Vector3(position1.x,  _parentStartPosition - moveVector, position1.z);
                ParentRectTransform.localPosition = position1;
            }
        }
        
        #endregion
    }
}
