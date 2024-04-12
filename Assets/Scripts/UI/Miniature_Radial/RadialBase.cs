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

        [Header("Disabled Settings")] 
        public Color DisabledBaseColour;
        public Color DisabledIconColour;
        
        [Header("On Press")] 
        public UnityEvent OnPressEvent;
        
        public bool Interactable = true;
        public bool Disabled;
        
        protected Image _baseImage;
        protected Image _iconImage;
        protected Animator _titleAnimator;
        private TextMeshProUGUI _titleTMP;
        protected bool _initialised;
        
        private static readonly int Right = Animator.StringToHash("Right");
        protected static readonly int Active = Animator.StringToHash("Active");

        #region UnityFunctions
        protected virtual void Awake()
        {
            OnInitialise();
        }

        protected virtual void OnEnable()
        {
            OnUnhighlight();
        }
        #endregion

        #region PublicFunctions
        
        public virtual void OnToggleDisable(bool toggle)
        {
            if(!_initialised) OnInitialise();
            Disabled = toggle;
            if (toggle)
            {
                _iconImage.color = DisabledIconColour;
                _baseImage.color = DisabledBaseColour;
            }
            else OnUnhighlight();
            
        }

        #endregion
        
        #region ProtectedFunctions

        protected virtual void OnInitialise()
        {
            _titleAnimator = transform.GetChild(0).GetComponent<Animator>();
            if(gameObject.activeInHierarchy) _titleAnimator.SetBool(Right, !IsTitleDisplayLeft);
            _titleTMP = transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            _baseImage = transform.GetChild(1).GetComponent<Image>();
            _iconImage = transform.GetChild(2).GetComponent<Image>();
            _titleTMP.text = Title;
            _initialised = true;
        }
        
        protected virtual void OnHighlight()
        {
            if(gameObject.activeInHierarchy) _titleAnimator.SetBool(Active, true);
        }

        protected virtual void OnUnhighlight()
        {
            if(gameObject.activeInHierarchy) _titleAnimator.SetBool(Active, false);
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
