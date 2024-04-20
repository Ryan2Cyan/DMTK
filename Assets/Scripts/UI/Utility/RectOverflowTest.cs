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
        public float MaxHeight;
        public Canvas ParentCanvas;

        [Header("Element Settings")]
        public Vector2 ElementSize;
        public Vector2 Spacing;
        private readonly Vector3[] _elementCorners = new Vector3[4];

        [Header("Details")] 
        public float ParentStartTopPosition;
        public float ParentStartPosition;
        public Vector2 Bounds;
        public Vector2 TopLeft;
        public Vector2Int Dimensions;
        private readonly Vector3[] _boundsCorners = new Vector3[4];

        [Header("Settings")] 
        public bool ScaleYToMaxRowCount;

        [ContextMenu("SetParentPos")]
        public void SetParentPos()
        {
            ParentRectTransform.GetLocalCorners(_elementCorners);
            var localPosition = ParentRectTransform.localPosition;
            ParentStartTopPosition = localPosition.y + _elementCorners[1].y;
            ParentStartPosition = localPosition.y;
        }
        
        private void Update()
        {
            // Calculate bounds:
            ParentRectTransform.GetWorldCorners(_boundsCorners);
            var scaleFactor = ParentCanvas.scaleFactor;
            
            TopLeft = _boundsCorners[1];
            Bounds = new Vector2(_boundsCorners[3].x - _boundsCorners[0].x, _boundsCorners[1].y - _boundsCorners[0].y) / scaleFactor;
            
            // Calculate dimensions x: rows, y: columns:
            var maxElementSize = ElementSize + Spacing;
            var halfElementSize = maxElementSize / 2f;
            Dimensions = new Vector2Int((int)(Bounds.y / halfElementSize.y), (int)(Bounds.x / halfElementSize.x));
                
            // Place all elements within confines of the dimensions:
            var elementIndex = 0;
            var currentElementPosition = TopLeft;
            MaxHeight = maxElementSize.y;
            
            for (var i = 0; i < (ScaleYToMaxRowCount ? int.MaxValue : Dimensions.x); i++)
            {
                // Reset row position:
                currentElementPosition.x = TopLeft.x;
                if (Dimensions.y == 0) return;
                for (var j = 0; j < Dimensions.y; j++)
                {
                    var currentElement = Elements[elementIndex];
                    currentElement.position = currentElementPosition;
                    currentElement.sizeDelta = ElementSize;
                    currentElement.gameObject.SetActive(true);
                    elementIndex++;
                 
                    // Increment row:
                    currentElementPosition.x += halfElementSize.x * ParentCanvas.scaleFactor;
                    
                    // Increment column: 
                    if (j == Dimensions.y - 1)
                    {
                        currentElementPosition.y -= halfElementSize.y * ParentCanvas.scaleFactor;
                        MaxHeight += maxElementSize.y;
                    }
                    
                    // All elements are organised, exit:
                    if (elementIndex < Elements.Count) continue;
                    for (var k = elementIndex; k < Elements.Count; k++) Elements[k].gameObject.SetActive(false);

                    ScaleWindowToRows();
                    return;
                }
            }
            
            for (var k = elementIndex; k < Elements.Count; k++) Elements[k].gameObject.SetActive(false);
            ScaleWindowToRows();
            return;

            void ScaleWindowToRows()
            {
                if (!ScaleYToMaxRowCount) return;
                
                // Set scale to height of combined rows:
                ParentRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, MaxHeight == 0 ? 1 : MaxHeight * ParentCanvas.scaleFactor);
                    
                // Move rect to align with y-axis point:
                ParentRectTransform.GetLocalCorners(_elementCorners);
                var newTop = ParentStartPosition + _elementCorners[1].y;
                var position1 = ParentRectTransform.localPosition;
                var moveVector = newTop - ParentStartTopPosition;
                position1 = new Vector3(position1.x,  ParentStartPosition - moveVector, position1.z);
                ParentRectTransform.localPosition = position1;
            }
        }
    }
}
