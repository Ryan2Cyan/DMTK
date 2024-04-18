using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Utility
{
    [ExecuteAlways]
    public class RectOverflow : MonoBehaviour
    {
        [Header("Settings")] 
        public Vector2 ParentSizeDelta;
        public Vector2 ElementSize;
        public Vector2 Spacing;
        public Vector2 Offset;
        public bool PrioritiseColumns;

        public enum HorizontalAlignment { Left, Right }
        public enum VerticalAlignment { Up, Down }
        public HorizontalAlignment RowAlignment = HorizontalAlignment.Right;
        public VerticalAlignment ColumnAlignment = VerticalAlignment.Down;
        
        public enum Anchor { TopLeft, TopCentre, TopRight, MiddleLeft, MiddleCentre, MiddleRight, BottomLeft, BottomCentre, BottomRight }

        public Anchor ElementAnchor;

        [Header("Components")] 
        public RectTransform RectBounds;
        public List<RectTransform> RectElements;

        private readonly Vector3[] _boundsCorners = new Vector3[4];
        private Vector2 _bounds;
        private Vector2 _elementAnchor;

        #region UnityFunctions

        private void Update()
        {
            RectBounds.sizeDelta = ParentSizeDelta;
            // Calculate bounds:
            RectBounds.GetLocalCorners(_boundsCorners);
            _bounds = new Vector2(_boundsCorners[3].x - _boundsCorners[0].x, _boundsCorners[1].y - _boundsCorners[0].y);
            _elementAnchor = ElementAnchor switch
            {
                Anchor.TopLeft => Vector2.up,
                Anchor.TopCentre => new Vector2(0.5f, 1f),
                Anchor.TopRight => Vector2.one,
                Anchor.MiddleLeft => new Vector2(0f, 0.5f),
                Anchor.MiddleCentre => new Vector2(0.5f, 0.5f),
                Anchor.MiddleRight => new Vector2(1f, 0.5f),
                Anchor.BottomLeft => Vector2.zero,
                Anchor.BottomCentre => new Vector2(0.5f, 0f),
                Anchor.BottomRight => Vector2.right,
                _ => throw new ArgumentOutOfRangeException()
            };
            RecalculateDimensions();
        }

        #endregion

        #region PrivateFunctions

        private void RecalculateDimensions()
        {
            var elementTotalSize = ElementSize + Spacing;
            var halfElementTotalSize = elementTotalSize / 2f;
            var rows = 0;
            var columns = 0;
            
            // Calculate rows, columns, and elements that fit within the bounds:
            var currentIndex = 0;
            var count = Vector2.zero;
            while (currentIndex < RectElements.Count)
            {
                // Handle element placement:
                var currentElement = RectElements[currentIndex];
                currentElement.gameObject.SetActive(true);
                currentElement.anchorMin = _elementAnchor;
                currentElement.anchorMax = _elementAnchor;
                currentElement.anchoredPosition = new Vector2(
                    RowAlignment == HorizontalAlignment.Left ? -(count.x + Offset.x) : count.x + Offset.x,
                    ColumnAlignment == VerticalAlignment.Down ? -(count.y + Offset.y) : count.y + Offset.y);
                currentElement.sizeDelta = ElementSize;
                
                // Calculate next row/column:
                currentIndex++;
                
                if (!PrioritiseColumns)
                {
                    count.x += halfElementTotalSize.x;
                    if (!(count.x >= _bounds.x)) continue;
                    
                    count.y += halfElementTotalSize.y;
                    if (count.y >= _bounds.y) break;
                    rows++;
                    count.x = 0f;
                }
                else
                {
                    count.y += halfElementTotalSize.y;
                    if (!(count.y >= _bounds.y)) continue;
            
                    count.x += halfElementTotalSize.x;
                    if (count.x >= _bounds.x) break;
                    count.y = 0f;
                }
            }

            // RectBounds.sizeDelta = new Vector2(RectBounds.sizeDelta.x, rows * elementTotalSize.y);
            // Disable all elements that don't fit within the bounds:
            if (currentIndex >= RectElements.Count) return;
            for (var i = currentIndex; i < RectElements.Count; i++)
            {
                RectElements[i].gameObject.SetActive(false);
            }
        }

        #endregion
    }
}
