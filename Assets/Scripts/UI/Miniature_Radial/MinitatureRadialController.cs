using System.Collections.Generic;
using UnityEngine;

namespace UI.Miniature_Radial
{
    public class MinitatureRadialController : MonoBehaviour
    {
        private List<MiniatureRadialBasic> _radialIcons;

        private void Awake()
        {
            var radialIcons = GetComponentsInChildren<MiniatureRadialBasic>();
            _radialIcons = new List<MiniatureRadialBasic>(radialIcons);
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
