using UnityEngine.Events;

namespace UI.Utility
{
    public class CullUnityEvent : CullBase
    {
        public UnityEvent ToggleEvent;
        
        protected override void ToggleUIImages(bool toggle)
        {
            base.ToggleUIImages(toggle);
            ToggleEvent.Invoke();
        }
    }
}
