using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    /// <summary> Changes the colour of a button's TMP Texts depending on whether the user's cursor is
    /// hovering over it or not. </summary>
    public class ButtonOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public List<TextMeshProUGUI> AllTMPTexts;
        public Color OnHoverColour;
        public Color OffHoverColour;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            foreach (var text in AllTMPTexts) text.color = OnHoverColour;
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            foreach (var text in AllTMPTexts) text.color = OffHoverColour;
        }

        private void OnDisable()
        {
            foreach (var text in AllTMPTexts) text.color = OffHoverColour;
        }
    }
}
