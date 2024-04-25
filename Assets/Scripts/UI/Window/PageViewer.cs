using System.Collections.Generic;
using UI.Miniature_Radial;
using UnityEngine;

namespace UI.Window
{
    public class PageViewer : MonoBehaviour
    {
        public List<GameObject> Pages;
        public int SelectedPageIndex;
        public RadialBasic NextRadial;
        public RadialBasic PreviousRadial;
        
        #region UnityFunctions

        private void Awake()
        {
            SelectedPageIndex = 0;
            PreviousRadial.OnToggleDisable(true);
        }

        #endregion

        #region PublicFunctions

        public void NextPage()
        {
            if (SelectedPageIndex == Pages.Count - 1) return;
            
            if(PreviousRadial.DisableOnEnable) PreviousRadial.OnToggleDisable(false);
            Pages[SelectedPageIndex].SetActive(false);
            SelectedPageIndex++;
            Pages[SelectedPageIndex].SetActive(true);
            
            if (SelectedPageIndex == Pages.Count - 1) NextRadial.OnToggleDisable(true);
        }

        public void PreviousPage()
        {
            if (SelectedPageIndex == 0) return;
            
            if(NextRadial.DisableOnEnable) NextRadial.OnToggleDisable(false);
            Pages[SelectedPageIndex].SetActive(false);
            SelectedPageIndex--;
            Pages[SelectedPageIndex].SetActive(true);
            
            if (SelectedPageIndex == 0) PreviousRadial.OnToggleDisable(true);
        }

        #endregion       
    }
}
