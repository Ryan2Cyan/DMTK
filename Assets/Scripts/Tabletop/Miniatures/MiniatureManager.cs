using System.Collections.Generic;
using General;
using Input;
using UI;
using UI.Miniature_Data;
using UI.Miniature_Radial;
using UnityEngine;
using Utility;

namespace Tabletop.Miniatures
{
    public class MiniatureManager : MonoBehaviour, IManager<Miniature>
    {
        public static MiniatureManager Instance;
        
        [Header("Miniatures")]
        public List<Miniature> RegisteredMiniatures;
        public Miniature SelectedMiniature;
        public Miniature GrabbedMiniature;
        
        [Header("Components")]
        public ObjectPool MiniatureDataUIPool;
        public LayerMask MiniatureLayerMask;
        
        private bool _isMiniatureSelected;
        private bool _isMiniatureGrabbed;

        #region UnityFunctions
        
        private void Awake()
        {
            Instance = this;
            
            // NOTE: Will most likely need to load in the miniature's data from a scene save slot later:
            RegisteredMiniatures ??= new List<Miniature>();
        }
        
        private void OnEnable()
        {
            InputManager.OnMouseUp += OnMouseUp;
            InputManager.OnMouseHold += OnMouseHold;
            InputManager.OnMouseHoldCancelled += OnHoldRelease;
        }

        private void OnDisable()
        {
            InputManager.OnMouseUp -= OnMouseUp;
            InputManager.OnMouseHold -= OnMouseHold;
            InputManager.OnMouseHoldCancelled -= OnHoldRelease;
        }
        
        #endregion

        #region InputQueueFunctions

        private void OnMouseUp()
        {
            InputManager.Instance.QueueMiniatureInputFunction(MouseUp);
        }

        private void OnMouseHold()
        {
            InputManager.Instance.QueueMiniatureInputFunction(MouseHold);
        }

        private void OnHoldRelease()
        {
            InputManager.Instance.QueueMiniatureInputFunction(HoldRelease);
        }
        
        #endregion

        #region InputFunctions
        
        private void MouseUp()
        {
            if (UIManager.Instance.UIInteraction) return;
            
            if (SelectedMiniature == null) RadialManager.Instance.Disable();
            else RadialManager.Instance.MiniatureClicked(SelectedMiniature.Data);
        }
        
        private void MouseHold()
        {
            if (UIManager.Instance.UISelected) return;
            if (!_isMiniatureSelected) return;
            if (_isMiniatureGrabbed) return;
            
            RadialManager.Instance.Disable();
            SelectedMiniature.OnGrab();
            GrabbedMiniature = SelectedMiniature;
            _isMiniatureGrabbed = true;
        }
        
        /// <summary> If any miniature is grabbed, release it.</summary>
        private void HoldRelease()
        {
            if (!_isMiniatureGrabbed) return;
            GrabbedMiniature.OnRelease();
            GrabbedMiniature = null;
            _isMiniatureGrabbed = false;
        }
        
        #endregion

        #region ManagerFunctions

        public void OnUpdate()
        {
            RaycastMouseOnUI();
        }

        public void OnLateUpdate()
        {
        }

        public void RegisterElement(Miniature element)
        {
            RegisteredMiniatures.Add(element);
            var miniatureDataUI = (MiniatureDataUIManager)MiniatureDataUIPool.GetPooledObject();
            miniatureDataUI.Instantiate(element.Data);
        }

        public void UnregisterElement(Miniature element)
        {
            RegisteredMiniatures.Remove(element);
        }
        
        #endregion

        #region PrivateFunctions

        private void RaycastMouseOnUI()
        {
            var hit = DMTKPhysicsUtility.PhysicsMouseRayCast(MiniatureLayerMask);
            foreach (var miniature in RegisteredMiniatures)
            {
                if (miniature.Collider != hit.collider) continue;
                if(_isMiniatureSelected) SelectedMiniature.ToggleOutline(false);
                SelectedMiniature = miniature;
                if(!UIManager.Instance.UISelected) SelectedMiniature.ToggleOutline(true);
                _isMiniatureSelected = true;
                return;
            }

            if(_isMiniatureSelected) SelectedMiniature.ToggleOutline(false);
            SelectedMiniature = null;
            _isMiniatureSelected = false;
        }
        #endregion
    }
}
