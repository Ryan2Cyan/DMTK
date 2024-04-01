using UnityEngine;

namespace UI.Miniature_Radial
{
    public interface IRadialUIManagerState
    {
        public void OnStart(RadialUIManager radialUIManager) {}
        public void OnExit(RadialUIManager radialUIManager) {}
    }

    public class RadialUIManagerDisabled : IRadialUIManagerState
    {
        public void OnStart(RadialUIManager radialUIManager)
        {
            radialUIManager.HideAll();
        }
    }
    
    public class RadialUIManagerMain : IRadialUIManagerState
    {
        private static readonly int Enabled = Animator.StringToHash("Enabled");

        public void OnStart(RadialUIManager radialUIManager)
        {
            radialUIManager.MainRadial.SetBool(Enabled, true);
            Vector3 screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, radialUIManager.SelectedMiniData.transform.position);
            radialUIManager.transform.position = new Vector3(screenPosition.x, screenPosition.y + radialUIManager.RadialScreenSpaceYOffset + screenPosition.z);
        }

        public void OnExit(RadialUIManager radialUIManager)
        {
            radialUIManager.MainRadial.SetBool(Enabled, false);
        }
    }
    
    public class RadialUIManagerStatusConditions : IRadialUIManagerState
    {
        private static readonly int Enabled = Animator.StringToHash("Enabled");

        public void OnStart(RadialUIManager radialUIManager)
        {
            radialUIManager.ConditionalsRadial.SetBool(Enabled, true);
        }

        public void OnExit(RadialUIManager radialUIManager)
        {
            radialUIManager.ConditionalsRadial.SetBool(Enabled, false);
        }
    }
}