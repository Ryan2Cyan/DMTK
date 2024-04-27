using UnityEngine;

namespace UI.Miniature_Radial
{
    /// <summary>Icon that appears within miniature radial UI. Can be highlighted, or un-highlighted. Clicking
    /// on a radial icon will have a different effect depending on it's event.</summary>
    public class RadialBasic : RadialBase
    {
        public bool UseColour;
        public Color BaseHighlightedColour;
        public Color BaseUnhighlightedColour;
        public Color IconHighlightedColour;
        public Color IconUnhighlightedColour;
        
        public bool UseSprites;
        public Sprite IconHighlightedSprite;
        public Sprite IconUnhighlightedSprite;
        
        #region ProtectedFunctions

        protected override void OnHighlight()
        {
            if (Disabled) return;
            base.OnHighlight();

            if (UseColour)
            {
                BaseImage.color = BaseHighlightedColour;
                IconImage.color = IconHighlightedColour;
            }
            
            if (UseSprites) IconImage.sprite = IconHighlightedSprite;
        }

        protected override void OnUnhighlight()
        {
            if (Disabled) return;
            base.OnUnhighlight();
            if (UseColour)
            {
                BaseImage.color = BaseUnhighlightedColour;
                IconImage.color = IconUnhighlightedColour;
            }

            if (UseSprites) IconImage.sprite = IconUnhighlightedSprite;
        }

        protected override void OnPress()
        {
            if (Disabled) return;
            base.OnPress();
            if (UseColour)
            {
                BaseImage.color = BaseUnhighlightedColour;
                IconImage.color = IconUnhighlightedColour;
            }
            
            if (UseSprites) IconImage.sprite = IconUnhighlightedSprite;
        }

        #endregion
    }
}
