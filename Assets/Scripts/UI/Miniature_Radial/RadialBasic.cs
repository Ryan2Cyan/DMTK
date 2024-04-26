using UnityEngine;

namespace UI.Miniature_Radial
{
    /// <summary>Icon that appears within miniature radial UI. Can be highlighted, or un-highlighted. Clicking
    /// on a radial icon will have a different effect depending on it's event.</summary>
    public class RadialBasic : RadialBase
    {
        [Header("Base Colour Settings")]
        public Color BaseHighlightedColour;
        public Color BaseUnhighlightedColour;
        
        [Header("Icon Colour Settings")]
        public Color IconHighlightedColour;
        public Color IconUnhighlightedColour;
        
        #region ProtectedFunctions

        protected override void OnHighlight()
        {
            if (Disabled) return;
            base.OnHighlight();
            BaseImage.color = BaseHighlightedColour;
            IconImage.color = IconHighlightedColour;
        }

        protected override void OnUnhighlight()
        {
            if (Disabled) return;
            base.OnUnhighlight();
            BaseImage.color = BaseUnhighlightedColour;
            IconImage.color = IconUnhighlightedColour;
        }

        protected override void OnPress()
        {
            if (Disabled) return;
            base.OnPress();
            BaseImage.color = BaseUnhighlightedColour;
            IconImage.color = IconUnhighlightedColour;
        }

        #endregion
    }
}
