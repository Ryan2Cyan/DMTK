using System.Collections.Generic;
using UI.Miniature_Radial;
using UnityEngine;
using UnityEngine.Events;

namespace UI.UI_Interactables
{
    public class DMTKToggleOptions : MonoBehaviour
    {
        public List<RadialToggleOptions> RadialToggles;
        public int SelectedIndex;
        public UnityEvent OnOptionSelected;

        private void OnEnable()
        {
            RadialToggles[SelectedIndex].TurnOn();
        }

        public void SelectOption(int index)
        {
            SelectedIndex = index;
            for (var i = 0; i < RadialToggles.Count; i++)
            {
                if (index == i) continue;
                RadialToggles[i].TurnOff();
            }
            OnOptionSelected?.Invoke();
        }
    }
}
