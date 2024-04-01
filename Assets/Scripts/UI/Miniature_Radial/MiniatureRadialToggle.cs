using UnityEngine;

namespace UI.Miniature_Radial
{
    public class MiniatureRadialToggle : MiniatureRadialBase
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

        [HideInInspector] public bool Toggle;
        [HideInInspector] public bool Highlighted;
        
        #region UnityFunctions

        protected override void Awake()
        {
            base.Awake();
            _baseImage.color = BaseToggleOffColour;
            _iconImage.color = IconToggleOffColour;
            Toggle = false;
        }

        #endregion

        #region OverrideFunctions

        protected override void OnHighlight()
        {
            base.OnHighlight();
            _baseImage.color = Toggle ? BaseHighlightedOnColour : BaseHighlightedOffColour;
            _iconImage.color = Toggle ? IconHighlightedOnColour : IconHighlightedOffColour;
            Highlighted = true;
        }

        protected override void OnUnhighlight()
        {
            base.OnUnhighlight();
            _baseImage.color = Toggle ? BaseToggleOnColour : BaseToggleOffColour;
            _iconImage.color = Toggle ? IconToggleOnColour : IconToggleOffColour;
            Highlighted = false;
        }

        protected override void OnPress()
        {
            base.OnPress();
            Toggle = !Toggle;
            if (Highlighted)
            {
                _baseImage.color = Toggle ? BaseHighlightedOnColour : BaseHighlightedOffColour;
                _iconImage.color = Toggle ? IconHighlightedOnColour : IconHighlightedOffColour;
            }
            else
            {
                _baseImage.color = Toggle ? BaseToggleOnColour : BaseToggleOffColour;
                _iconImage.color = Toggle ? IconToggleOnColour : IconToggleOffColour;
            }
        }

        #endregion
    }
}
