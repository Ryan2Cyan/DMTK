using UnityEngine;

namespace UI.Miniature_Radial
{
    public interface IRadialManagerState
    {
        public void OnStart(RadialManager radialManager) {}
        public void OnExit(RadialManager radialManager) {}
    }

    public class RadialManagerDisabled : IRadialManagerState
    {
        public void OnStart(RadialManager radialManager)
        {
            radialManager.HideAll();
        }
    }
    
    public class RadialManagerMain : IRadialManagerState
    {
        private static readonly int Enabled = Animator.StringToHash("Enabled");

        public void OnStart(RadialManager radialManager)
        {
            radialManager.EnableWorldSpaceDisplay();
            var mainRadialAnimator = radialManager.MainRadial.MenuAnimator;
            mainRadialAnimator.Rebind();
            mainRadialAnimator.Update(0f);
            mainRadialAnimator.SetBool(Enabled, true);
            radialManager.MainRadial.cullEventScript.Target = radialManager.SelectedMiniData.transform;
            
            // Update all data to match the selected miniature:
            radialManager.LoadMiniatureData();
        }

        public void OnExit(RadialManager radialManager)
        {
            radialManager.MainRadial.MenuAnimator.SetBool(Enabled, false);
            radialManager.DisableWorldSpaceDisplay();
        }
    }
    
    public class RadialManagerStatusConditions : IRadialManagerState
    {
        private static readonly int Enabled = Animator.StringToHash("Enabled");

        public void OnStart(RadialManager radialManager)
        {
            radialManager.ConditionalsRadial.MenuAnimator.SetBool(Enabled, true);
            radialManager.EnableWorldSpaceDisplay();
            radialManager.ConditionalsRadial.cullEventScript.Target = radialManager.SelectedMiniData.transform;
        }

        public void OnExit(RadialManager radialManager)
        {
            radialManager.ConditionalsRadial.MenuAnimator.SetBool(Enabled, false);
            radialManager.DisableWorldSpaceDisplay();
        }
    }
}