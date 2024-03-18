using TMPro;
using UnityEngine;

namespace UI.Window
{
    public class Window : MonoBehaviour
    {
        public string WindowTitle;
        public TextMeshProUGUI TitleTMP;
        public Vector2 MinimumSize;
        public bool FixedSize;
        public bool FixedPosition;
        
        private Canvas _parentCanvas;
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _parentCanvas = GetComponentInParent<Canvas>();
        }
        
        [ExecuteAlways]
        private void OnValidate()
        {
            TitleTMP.text = WindowTitle;
        }

        public RectTransform GetRectTransform()
        {
            return _rectTransform;
        }

        public Canvas GetCanvas()
        {
            return _parentCanvas;
        }
    }
}
