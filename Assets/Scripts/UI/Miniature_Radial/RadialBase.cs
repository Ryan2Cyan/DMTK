using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Miniature_Radial
{
    public class RadialBase : MonoBehaviour, UIElement
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
        
        private static readonly int RightParam = Animator.StringToHash("Right");
        protected static readonly int ActiveParam = Animator.StringToHash("Active");
        
        public bool UIElementActive { get; set; }

        #region UnityFunctions
        protected virtual void Awake()
        {
            OnInitialise();
            OnToggleDisable(Disabled);
        }

        protected virtual void OnEnable()
        {
            OnUnhighlight();
        }
        #endregion

        #region PublicFunctions
        
        public void OnToggleDisable(bool toggle)
        {
            if(!_initialised) OnInitialise();
            Disabled = toggle;
            if (toggle)
            {
                _iconImage.color = DisabledIconColour;
                _baseImage.color = DisabledBaseColour;
                UIElementActive = false;
            }
            else
            {
                OnUnhighlight();
                UIElementActive = true;
            }
        }

        #endregion
        
        #region ProtectedFunctions

        protected virtual void OnInitialise()
        {
            _titleAnimator = transform.GetChild(0).GetComponent<Animator>();
            if(gameObject.activeInHierarchy) _titleAnimator.SetBool(RightParam, !IsTitleDisplayLeft);
            _titleTMP = transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            _baseImage = transform.GetChild(1).GetComponent<Image>();
            _iconImage = transform.GetChild(2).GetComponent<Image>();
            _titleTMP.text = Title;
            _initialised = true;
        }
        
        protected virtual void OnHighlight()
        {
            if(gameObject.activeInHierarchy) _titleAnimator.SetBool(ActiveParam, true);
        }

        protected virtual void OnUnhighlight()
        {
            if(gameObject.activeInHierarchy) _titleAnimator.SetBool(ActiveParam, false);
        }

        protected virtual void OnPress()
        {
            OnPressEvent.Invoke();
            _titleAnimator.SetBool(ActiveParam, false);
        }
        #endregion
        
        #region InputFunctions

        public void OnMouseDown()
        {
            if (!Interactable) return;
            OnPress();
        }

        public void OnMouseUp()
        {
            
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
