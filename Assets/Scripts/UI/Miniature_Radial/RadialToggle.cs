using UnityEngine;

namespace UI.Miniature_Radial
{
    public class RadialToggle : RadialBase
    {
        public bool ToggleTrueOnAwake;
        
        public bool UseColours;
        public Color BaseUnhighlightOnColour;
        public Color BaseUnhighlightOffColour;
        public Color BaseHighlightOnColour;
        public Color BaseHighlightOffColour;
        
        public Color IconUnhighlightOnColour;
        public Color IconUnhighlightOffColour;
        public Color IconHighlightOnColour;
        public Color IconHighlightOffColour;
        
        public bool UseSprites;
        public Sprite IconOnHighlightSprite;
        public Sprite IconOnUnhighlightSprite;
        public Sprite IconOffHighlightSprite;
        public Sprite IconOffUnhighlightSprite;
        
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

        private bool _toggle;
        
        #region UnityFunctions

        protected override void Awake()
        {
            UIElementPriority = 1;
            Interactable = true;
            OnInitialise();
        }

        #endregion
        
        #region OverrideFunctions

        protected override void OnInitialise()
        {
            if (_initialised) return;
            base.OnInitialise();
            
            if (UseColours)
            {
                BaseImage.color = ToggleTrueOnAwake ? BaseUnhighlightOnColour : BaseUnhighlightOffColour;
                IconImage.color = ToggleTrueOnAwake ? IconUnhighlightOnColour : IconUnhighlightOffColour;   
            }
            
            if (UseSprites) IconImage.sprite = ToggleTrueOnAwake ? IconOnUnhighlightSprite : IconOffUnhighlightSprite;
            
            Toggle = ToggleTrueOnAwake;
        }

        protected override void OnHighlight()
        {
            if (Disabled) return;
            base.OnHighlight();
            
            if (UseColours)
            {
                BaseImage.color = Toggle ? BaseHighlightOnColour : BaseHighlightOffColour;
                IconImage.color = Toggle ? IconHighlightOnColour : IconHighlightOffColour;   
            }
            
            if (UseSprites) IconImage.sprite = Toggle ? IconOnHighlightSprite : IconOffHighlightSprite;
        }

        protected override void OnUnhighlight()
        {
            if (Disabled) return;
            base.OnUnhighlight();

            if (UseColours)
            {
                BaseImage.color = Toggle ? BaseUnhighlightOnColour : BaseUnhighlightOffColour;
                IconImage.color = Toggle ? IconUnhighlightOnColour : IconUnhighlightOffColour;   
            }
            
            if (UseSprites) IconImage.sprite = Toggle ? IconOnUnhighlightSprite : IconOffUnhighlightSprite;
        }

        protected override void OnPress()
        {
            if (Disabled) return;
            base.OnPress();
            Toggle = !Toggle;
            
            if (Highlighted)
            {
                if (UseColours)
                {
                    BaseImage.color = Toggle ? BaseHighlightOnColour : BaseHighlightOffColour;
                    IconImage.color = Toggle ? IconHighlightOnColour : IconHighlightOffColour;
                }
                if (UseSprites) IconImage.sprite = Toggle ? IconOnHighlightSprite : IconOffHighlightSprite;
            }
            else
            {
                if (UseColours)
                {
                    BaseImage.color = Toggle ? BaseUnhighlightOnColour : BaseUnhighlightOffColour;
                    IconImage.color = Toggle ? IconUnhighlightOnColour : IconUnhighlightOffColour;    
                }
                
                if (UseSprites) IconImage.sprite = Toggle ? IconOnUnhighlightSprite : IconOffUnhighlightSprite;
            }
        }

        #endregion
    }
}
