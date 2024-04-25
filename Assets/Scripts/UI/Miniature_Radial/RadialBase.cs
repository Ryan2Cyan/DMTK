using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Miniature_Radial
{
    public class RadialBase : MonoBehaviour, UIElement
    {
        public string Title;
        
        public enum RadialTitleDisplayDirection { Left, Right }
        public RadialTitleDisplayDirection TitleDisplayDirection;
        
        public Color DisabledBaseColour;
        public Color DisabledIconColour;
        
        public UnityEvent OnPressEvent;
        
        public bool Interactable = true;
        public bool DisableOnEnable;
        public bool DebugActive;
        
        public Image BaseImage;
        public Image IconImage;
        
        protected Animator _titleAnimator;
        protected bool _initialised;
        protected bool _disabled;
        private TextMeshProUGUI _titleTMP;
        
        [HideInInspector] public bool Highlighted;
        
        private static readonly int RightParam = Animator.StringToHash("Right");
        protected static readonly int ActiveParam = Animator.StringToHash("Active");
        
        public bool UIElementActive { get; set; }
        public int UIElementPriority { get; set; }

        #region UnityFunctions
        protected virtual void Awake()
        {
            UIElementPriority = 1;
            OnInitialise();
            OnToggleDisable(DisableOnEnable);
        }

        protected virtual void OnEnable()
        {
            _titleAnimator.SetBool(RightParam, TitleDisplayDirection == RadialTitleDisplayDirection.Right);
            if (DisableOnEnable) _disabled = true;
            OnUnhighlight();
        }
        #endregion

        #region PublicFunctions
        
        public void OnToggleDisable(bool toggle)
        {
            if(!_initialised) OnInitialise();
            _disabled = toggle;
            if (toggle)
            {
                if(DebugActive) Debug.Log("Radial [" + gameObject.name + "] Disabled: On");
                IconImage.color = DisabledIconColour;
                BaseImage.color = DisabledBaseColour;
                UIElementActive = false;
            }
            else
            {
                if(DebugActive) Debug.Log("Radial [" + gameObject.name + "] Disabled: Off");
                OnUnhighlight();
                UIElementActive = true;
            }
        }

        #endregion
        
        #region ProtectedFunctions

        protected virtual void OnInitialise()
        {
            _titleAnimator = transform.GetChild(0).GetComponent<Animator>();
            _titleTMP = transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            BaseImage = transform.GetChild(1).GetComponent<Image>();
            IconImage = transform.GetChild(2).GetComponent<Image>();
            _titleTMP.text = Title;
            _initialised = true;
        }
        
        protected virtual void OnHighlight()
        {
            if(DebugActive) Debug.Log("Radial [" + gameObject.name + "] Highlight");
            
            if (!gameObject.activeInHierarchy) return;
            _titleAnimator.SetBool(ActiveParam, true);
            Highlighted = true;
        }

        protected virtual void OnUnhighlight()
        {
            if(DebugActive) Debug.Log("Radial [" + gameObject.name + "] Unhighlight");
            
            if (!gameObject.activeInHierarchy) return;
            _titleAnimator.SetBool(ActiveParam, false);
            Highlighted = false;
        }

        protected virtual void OnPress()
        {
            if(DebugActive) Debug.Log("Radial [" + gameObject.name + "] Press");
            OnPressEvent.Invoke();
            _titleAnimator.SetBool(ActiveParam, false);
        }
        
        #endregion
        
        #region InputFunctions

        public void OnMouseDown()
        {
            
        }

        public void OnMouseUp()
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
            // if (!Interactable) return;
            OnUnhighlight();
        }
        
        public void OnDrag() { }


        #region GUIFunctions

        public void GUIDisable()
        {
            IconImage.color = DisabledIconColour;
            BaseImage.color = DisabledBaseColour;
        }

        #endregion
        #endregion
    }
}
