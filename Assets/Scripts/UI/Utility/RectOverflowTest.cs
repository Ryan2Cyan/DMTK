using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Utility
{
    [ExecuteAlways]
    public class RectOverflowTest : MonoBehaviour
    {
        [Header("Components")] 
        public RectTransform ParentRectTransform;
        public List<RectTransform> Elements;
        public float MaxHeight;

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

        [FormerlySerializedAs("ScaleToMaxColumnCount")] [Header("Settings")] 
        public bool ScaleYToMaxRowCount;
        public bool EnableScaling;

        [ContextMenu("SetParentPos")]
        public void SetParentPos()
        {
            ParentRectTransform.GetLocalCorners(_elementCorners);
            ParentStartTopPosition = Mathf.Abs(ParentRectTransform.localPosition.y + _elementCorners[1].y);
            ParentStartPosition = ParentRectTransform.localPosition.y;
        }
        
        private void Update()
        {
            // Calculate bounds:
            ParentRectTransform.GetWorldCorners(_boundsCorners);
            TopLeft = _boundsCorners[1];
            Bounds = new Vector2(_boundsCorners[3].x - _boundsCorners[0].x, _boundsCorners[1].y - _boundsCorners[0].y);
            
            // Calculate element size:
            // if (Elements.Count == 0) return;
            // Elements[0].GetWorldCorners(_elementCorners);
            // ElementSize = new Vector2(_elementCorners[3].x - _elementCorners[0].x, _elementCorners[1].y - _elementCorners[0].y);
            
            // Calculate dimensions x: rows, y: columns:
            var maxElementSize = ElementSize + Spacing;
            var halfElementSize = maxElementSize / 2f;
            Dimensions = new Vector2Int((int)(Bounds.y / halfElementSize.y), (int)(Bounds.x / halfElementSize.x));
            // Dimensions = new Vector2Int()
                
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
                    currentElementPosition.x += halfElementSize.x;
                    
                    // Increment column: 
                    if (j == Dimensions.y - 1)
                    {
                        currentElementPosition.y -= halfElementSize.y;
                        MaxHeight += maxElementSize.y;
                    }
                    
                    // All elements are organised, exit:
                    if (elementIndex < Elements.Count) continue;
                    for (var k = elementIndex; k < Elements.Count; k++) Elements[k].gameObject.SetActive(false);
                    if (ScaleYToMaxRowCount)
                    {
                        ParentRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, MaxHeight == 0 ? 1 : MaxHeight);

                        if (EnableScaling)
                        {
                            ParentRectTransform.GetLocalCorners(_elementCorners);
                            var newTop = Mathf.Abs(ParentStartPosition + _elementCorners[1].y);
                            var position1 = ParentRectTransform.localPosition;
                            // Debug.Log("ParentStartPos :" + ParentStartPosition + " ParentStartTop :" +
                            //           ParentStartTopPosition + " New Top: " + newTop + " NewPos: " +
                            //           (ParentStartPosition - (ParentStartTopPosition - newTop)));
                            var modifier = ParentStartPosition > 0 ? ParentStartTopPosition - newTop : -(ParentStartTopPosition - newTop);
                            position1 = new Vector3(position1.x,  ParentStartPosition + modifier, position1.z);
                            ParentRectTransform.localPosition = position1;
                        }
                    }
                    return;
                }
            }
            
            if (ScaleYToMaxRowCount)
            {
                ParentRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, MaxHeight == 0 ? 1 : MaxHeight);
            }
            for (var k = elementIndex; k < Elements.Count; k++) Elements[k].gameObject.SetActive(false);
        }
    }
}
