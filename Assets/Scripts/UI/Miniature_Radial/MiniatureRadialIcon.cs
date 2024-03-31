using UnityEngine;

namespace UI.Miniature_Radial
{
    /// <summary>Icon that appears within miniature radial UI. Can be highlighted, or un-highlighted. Clicking
    /// on a radial icon will have a different effect depending on it's event.</summary>
    public class MiniatureRadialIcon : BaseMiniatureRadial
    {
        [Header("Colour Settings")]
        public Color BaseHighlightedColour;
        public Color BaseUnhighlightedColour;
        public Color IconHighlightedColour;
        public Color IconUnhighlightedColour;
        
        #region ProtectedFunctions

        protected override void OnHighlight()
        {
            base.OnHighlight();
            _baseImage.color = BaseHighlightedColour;
            _iconImage.color = IconHighlightedColour;
        }

        protected override void OnUnhighlight()
        {
            base.OnUnhighlight();
            _baseImage.color = BaseUnhighlightedColour;
            _iconImage.color = IconUnhighlightedColour;
        }
        #endregion
    }
}
