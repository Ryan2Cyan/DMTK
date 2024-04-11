using Input;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Miniature_Radial
{
    public class RadialBase : UIElement, IInputElement
    {
        [Header("Title Settings")]
        public string Title;
        public bool IsTitleDisplayLeft;
        
        [Header("On Press")] 
        public UnityEvent OnPressEvent;
        
        public bool Interactable = true;
        
        protected Image _baseImage;
        protected Image _iconImage;
        private TextMeshProUGUI _titleTMP;
        private Animator _titleAnimator;
        
        private static readonly int Right = Animator.StringToHash("Right");
        private static readonly int Active = Animator.StringToHash("Active");

        #region UnityFunctions
        protected virtual void Awake()
        {
            _titleAnimator = transform.GetChild(0).GetComponent<Animator>();
            _titleAnimator.SetBool(Right, !IsTitleDisplayLeft);
            _titleTMP = transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            _baseImage = transform.GetChild(1).GetComponent<Image>();
            _iconImage = transform.GetChild(2).GetComponent<Image>();
            _titleTMP.text = Title;
        }

        protected virtual void OnEnable()
        {
            OnUnhighlight();
        }
        #endregion
        
        #region ProtectedFunctions
        protected virtual void OnHighlight()
        {
            _titleAnimator.SetBool(Active, true);
        }

        protected virtual void OnUnhighlight()
        {
            _titleAnimator.SetBool(Active, false);
        }

        protected virtual void OnPress()
        {
            OnPressEvent.Invoke();
            _titleAnimator.SetBool(Active, false);
        }
        #endregion
        
        #region InputFunctions

        public void OnMouseDown()
        {
            if (!Interactable) return;
            OnPress();
        }

        public void OnMouseEnter()
        {
            if (!Interactable) return;
            OnHighlight();
        }

        public void OnMouseExit()
        {
            if (!Interactable) return;
            OnUnhighlight();
        }
        
        public void OnDrag() { }
        
        #endregion
    }
}
