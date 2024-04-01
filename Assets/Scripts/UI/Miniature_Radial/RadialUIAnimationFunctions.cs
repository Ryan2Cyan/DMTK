using System.Collections.Generic;
using UnityEngine;

namespace UI.Miniature_Radial
{
    public class RadialUIAnimationFunctions : MonoBehaviour
    {
        private List<BaseMiniatureRadial> _radialIcons;

        private void Awake()
        {
            var radialIcons = GetComponentsInChildren<BaseMiniatureRadial>();
            _radialIcons = new List<BaseMiniatureRadial>(radialIcons);
        }

        public void EnableIconInteraction()
        {
            foreach (var radialIcon in _radialIcons) radialIcon.Interactable = true;
        }
        
        public void DisableIconInteraction()
        {
            foreach (var radialIcon in _radialIcons) radialIcon.Interactable = false;
        }
    }
}
