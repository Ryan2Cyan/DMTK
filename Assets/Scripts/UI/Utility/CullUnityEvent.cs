using UnityEngine.Events;

namespace UI.Utility
{
    public class CullUnityEvent : CullBase
    {
        public UnityEvent OnCullEvent;
        
        protected override void ToggleUIImages(bool toggle)
        {
            base.ToggleUIImages(toggle);
            if(!toggle) OnCullEvent.Invoke();
        }
    }
}
