using System.Collections.Generic;
using UnityEngine;

namespace UI.Miniature_Radial
{
    public class RadialAnimationFunctions : MonoBehaviour
    {
        private List<RadialBase> _radialIcons;

        private void Awake()
        {
            var radialIcons = GetComponentsInChildren<RadialBase>();
            _radialIcons = new List<RadialBase>(radialIcons);
        }

        public void EnableIconInteraction()
        {
            foreach (var radialIcon in _radialIcons)
            {
                radialIcon.Interactable = true;
                radialIcon.UIElementActive = true;
            }
        }
        
        public void DisableIconInteraction()
        {
            foreach (var radialIcon in _radialIcons)
            {
                radialIcon.Interactable = false;
                radialIcon.UIElementActive = false;
            }
        }
    }
}
