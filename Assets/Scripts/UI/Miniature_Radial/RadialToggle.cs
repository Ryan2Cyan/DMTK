using UnityEngine;

namespace UI.Miniature_Radial
{
    public class RadialToggle : RadialBase
    {
        public bool ToggleColours;
        public Color BaseUnhighlightOnColour;
        public Color BaseUnhighlightOffColour;
        public Color BaseHighlightOnColour;
        public Color BaseHighlightOffColour;
        
        public Color IconUnhighlightOnColour;
        public Color IconUnhighlightOffColour;
        public Color IconHighlightOnColour;
        public Color IconHighlightOffColour;
        
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
            BaseImage.color = BaseUnhighlightOffColour;
            IconImage.color = IconUnhighlightOffColour;
            Toggle = false;
        }

        protected override void OnHighlight()
        {
            if (Disabled) return;
            base.OnHighlight();
            BaseImage.color = Toggle ? BaseHighlightOnColour : BaseHighlightOffColour;
            IconImage.color = Toggle ? IconHighlightOnColour : IconHighlightOffColour;
        }

        protected override void OnUnhighlight()
        {
            if (Disabled) return;
            base.OnUnhighlight();
            BaseImage.color = Toggle ? BaseUnhighlightOnColour : BaseUnhighlightOffColour;
            IconImage.color = Toggle ? IconUnhighlightOnColour : IconUnhighlightOffColour;
        }

        protected override void OnPress()
        {
            if (Disabled) return;
            base.OnPress();
            Toggle = !Toggle;
            if (Highlighted)
            {
                BaseImage.color = Toggle ? BaseHighlightOnColour : BaseHighlightOffColour;
                IconImage.color = Toggle ? IconHighlightOnColour : IconHighlightOffColour;
            }
            else
            {
                BaseImage.color = Toggle ? BaseUnhighlightOnColour : BaseUnhighlightOffColour;
                IconImage.color = Toggle ? IconUnhighlightOnColour : IconUnhighlightOffColour;
            }
        }

        #endregion
    }
}
