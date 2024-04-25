using UnityEngine;

namespace UI.Miniature_Radial
{
    public class RadialToggle : RadialBase
    {
        [Header("Base Colour Settings")]
        public Color BaseToggleOnColour;
        public Color BaseToggleOffColour;
        public Color BaseHighlightedOnColour;
        public Color BaseHighlightedOffColour;
        
        [Header("Icon Colour Settings")]
        public Color IconToggleOnColour;
        public Color IconToggleOffColour;
        public Color IconHighlightedOnColour;
        public Color IconHighlightedOffColour;

        [Header("Sprite Settings")] 
        public bool ToggleSprite;
        public Sprite SpriteToggleOn;
        public Sprite SpriteToggleOff;
        
        public bool Toggle
        {
            get => _toggle;
            set
            {
                _toggle = value;
                if(Highlighted) OnHighlight();
                else OnUnhighlight();
            }
        }

        protected bool _toggle;
        
        #region UnityFunctions

        protected override void Awake()
        {
            UIElementPriority = 1;
            OnInitialise();
        }

        #endregion
        
        #region OverrideFunctions

        protected override void OnInitialise()
        {
            if (_initialised) return;
            base.OnInitialise();
            BaseImage.color = BaseToggleOffColour;
            IconImage.color = IconToggleOffColour;
            Toggle = false;
        }

        protected override void OnHighlight()
        {
            if (Disabled) return;
            base.OnHighlight();
            BaseImage.color = Toggle ? BaseHighlightedOnColour : BaseHighlightedOffColour;
            IconImage.color = Toggle ? IconHighlightedOnColour : IconHighlightedOffColour;
        }

        protected override void OnUnhighlight()
        {
            if (Disabled) return;
            base.OnUnhighlight();
            BaseImage.color = Toggle ? BaseToggleOnColour : BaseToggleOffColour;
            IconImage.color = Toggle ? IconToggleOnColour : IconToggleOffColour;
        }

        protected override void OnPress()
        {
            if (Disabled) return;
            base.OnPress();
            Toggle = !Toggle;
            if (Highlighted)
            {
                BaseImage.color = Toggle ? BaseHighlightedOnColour : BaseHighlightedOffColour;
                IconImage.color = Toggle ? IconHighlightedOnColour : IconHighlightedOffColour;
            }
            else
            {
                BaseImage.color = Toggle ? BaseToggleOnColour : BaseToggleOffColour;
                IconImage.color = Toggle ? IconToggleOnColour : IconToggleOffColour;
            }
        }

        #endregion
    }
}
