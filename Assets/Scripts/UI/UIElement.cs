using UnityEngine;

namespace UI
{
    public class UIElement : MonoBehaviour
    {
        #region UnityFunctions

        private void OnEnable()
        {
            UIManager.Instance.RegisterElement(this);           
        }

        private void OnDisable()
        {
            UIManager.Instance.UnregisterElement(this);
        }

        #endregion
    }
}
