using System.Collections.Generic;
using UnityEngine;

namespace UI.Utility
{
    [ExecuteAlways]
    public class RectOverflow : MonoBehaviour
    {
        [Header("Settings")]
        public Vector2 ElementSize;
        public Vector2 Spacing;
        
        [Header("Components")]
        public RectTransform RectBounds;
        public List<RectTransform> RectElements;

        private Vector2Int Dimensions;
        private Vector2 _previousBoundSize;

        #region UnityFunctions

        private void Awake()
        {
            RecalculateDimensions();
        }

        private void Update()
        {
            // // Only execute if the bounds have changed:
            // if (RectBounds.sizeDelta == _previousBoundSize)
            // {
            //     _previousBoundSize = RectBounds.sizeDelta;
            //     return;
            // }

            RecalculateDimensions();
            _previousBoundSize = RectBounds.sizeDelta;
        }

        #endregion

        #region PrivateFunctions

        private void RecalculateDimensions()
        {
            var elementTotalSize = ElementSize + Spacing;
            var halfElementTotalSize = elementTotalSize / 2f;
            var constraints = RectBounds.sizeDelta;
            
            // // Calculate rows, columns, and elements that fit within the bounds:
            // var dimensions = new Vector2(Mathf.Floor(constraints.x / elementTotalSize.x), Mathf.Floor(constraints.y / elementTotalSize.y));
            var currentIndex = 0;
            var count = Vector2.zero;
            while (currentIndex < RectElements.Count)
            {
                var currentElement = RectElements[currentIndex];
                currentElement.gameObject.SetActive(true);
                currentElement.anchoredPosition = new Vector2(count.x, -count.y);
                currentElement.sizeDelta = ElementSize;
                currentIndex++;

                count.x += halfElementTotalSize.x;
                if (!(count.x >= constraints.x)) continue;
                
                count.y += halfElementTotalSize.y;
                if (count.y >= constraints.y) break;
                count.x = 0f;
            }
            
            //
            // for (var row = 1; row <= dimensions.x; row++)
            // {
            //     for (var column = 1; column <= dimensions.y; column++)
            //     {
            //         var currentElement = RectElements[currentIndex];
            //         currentElement.gameObject.SetActive(true);
            //         currentElement.anchoredPosition = new Vector2(row * halfElementTotalSize.x, column * -halfElementTotalSize.y);
            //         currentElement.sizeDelta = ElementSize;
            //         if (currentIndex + 1 >= RectElements.Count) return;
            //         currentIndex++;
            //     }
            // }
            
            // Disable all elements that don't fit within the bounds:
            if (currentIndex >= RectElements.Count) return;
            for (var i = currentIndex; i < RectElements.Count; i++) RectElements[i].gameObject.SetActive(false);
        }

        #endregion
    }
}
