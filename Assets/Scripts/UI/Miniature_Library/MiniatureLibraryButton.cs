using UI.UI_Interactables;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Miniature_Library
{
    public class MiniatureLibraryButton : SimpleButton
    {
        public Image BaseImage;
        [HideInInspector] public string MiniatureID;
        [HideInInspector] public RectTransform RectTransform;
        
        #region UnityFunctions

        protected override void Awake()
        {
            UIElementPriority = 2;
            base.Awake();
            RectTransform = GetComponent<RectTransform>();
        }
        
        
        #endregion

        #region InputFunctions

        public override void OnMouseDown()
        {
            MiniatureLibraryManager.Instance.LocateMiniatureId(MiniatureID);
        }

        public override void OnMouseEnter()
        {
            base.OnMouseEnter();
            MiniatureLibraryManager.Instance.RectOverflowScript.EnableScrolling = true;
        }

        public override void OnMouseExit()
        {
            base.OnMouseExit();
        }

        #endregion
    }
}
