using UI.UI_Interactables;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Miniature_Library
{
    public class MiniatureLibraryButton : DMTKSimpleButton
    {
        public Image BaseImage;
        [HideInInspector] public string MiniatureID;
        [HideInInspector] public RectTransform RectTransform;
        
        #region UnityFunctions

        protected override void Awake()
        {
            base.Awake();
            RectTransform = GetComponent<RectTransform>();
        }
        
        #endregion

        #region InputFunctions

        public override void OnMouseDown()
        {
            MiniatureLibraryManager.Instance.SpawnMiniature(MiniatureID);
        }

        #endregion
    }
}
