using System;
using System.Collections.Generic;
using UI.Miniature_Radial;
using UnityEngine;
using UnityEngine.Events;

namespace UI.UI_Interactables
{
    public class DMTKToggleOptions : MonoBehaviour
    {
        public List<RadialToggle> RadialToggles;
        public int SelectedIndex;
        public UnityEvent OnOptionSelected;

        private void Awake()
        {
            foreach (var toggle in RadialToggles) toggle.ToggleOption = true;
        }

        private void OnEnable()
        {
            foreach (var toggle in RadialToggles) toggle.UIElementActive = true;
            SelectOption(SelectedIndex);
        }

        private void OnDisable()
        {
            foreach (var toggle in RadialToggles) toggle.UIElementActive = false;   
        }

        public void SelectOption(int index)
        {
            SelectedIndex = index;
            RadialToggles[SelectedIndex].TurnOn();
            for (var i = 0; i < RadialToggles.Count; i++)
            {
                if (index == i) continue;
                RadialToggles[i].TurnOff();
            }
            OnOptionSelected?.Invoke();
        }
    }
}
