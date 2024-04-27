using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Miniature_Radial
{
    public class RadialBase : MonoBehaviour, UIElement
    {
        public bool UseTitle;
        public string Title;
        
        public enum RadialTitleDisplayDirection { Left, Right }
        public RadialTitleDisplayDirection TitleDisplayDirection;

        public Sprite DefaultIconSprite;
        
        public Color DefaultBaseColour;
        public Color DefaultIconColour;
        
        public Color DisabledBaseColour;
        public Color DisabledIconColour;
        
        public UnityEvent OnPressEvent;
        
        public Image BaseImage;
        public Image IconImage;
        
        public bool Interactable = true;
        public bool DisableOnEnable;
        public bool DebugActive;
        public bool Disabled;
        
        protected Animator _titleAnimator;
        protected bool _initialised;
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
            Interactable = true;
        }

        protected virtual void OnEnable()
        {
            if(UseTitle) _titleAnimator.SetBool(RightParam, TitleDisplayDirection == RadialTitleDisplayDirection.Right);
            if (DisableOnEnable) Disabled = true;
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
                if(DebugActive) Debug.Log("Radial [" + gameObject.name + "] Disabled: On");
                BaseImage.color = DisabledBaseColour;
                IconImage.color = DisabledIconColour;
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
            if (UseTitle)
            {
                _titleAnimator = transform.GetChild(0).GetComponent<Animator>();
                _titleTMP = transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                _titleTMP.text = Title;
            }

            BaseImage = transform.GetChild(1).GetComponent<Image>();
            BaseImage.color = DefaultBaseColour;
            IconImage = transform.GetChild(2).GetComponent<Image>();
            IconImage.sprite = DefaultIconSprite;
            IconImage.color = DefaultIconColour;
            _initialised = true;
            _initialised = true;
        }
        
        protected virtual void OnHighlight()
        {
            if(DebugActive) Debug.Log("Radial [" + gameObject.name + "] Highlight");
            
            if (!gameObject.activeInHierarchy) return;
            if(UseTitle) _titleAnimator.SetBool(ActiveParam, true);
            Highlighted = true;
        }

        protected virtual void OnUnhighlight()
        {
            if(DebugActive) Debug.Log("Radial [" + gameObject.name + "] Unhighlight");
            
            if (!gameObject.activeInHierarchy) return;
            if(UseTitle) _titleAnimator.SetBool(ActiveParam, false);
            Highlighted = false;
        }

        protected virtual void OnPress()
        {
            if(DebugActive) Debug.Log("Radial [" + gameObject.name + "] Press");
            if(UseTitle) _titleAnimator.SetBool(ActiveParam, false);
            OnPressEvent.Invoke();
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

        #endregion

        #region GUIFunctions

        [ExecuteAlways]
        public void SetGUI(Color baseColour, Color iconColour, Sprite iconSprite)
        {
            BaseImage.color = baseColour;
            IconImage.color = iconColour;
            IconImage.sprite = iconSprite;
        }
        
        #endregion
    }
}
