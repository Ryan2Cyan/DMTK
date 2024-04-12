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
        private bool _uiSelected;

        #region UnityFunctions
        
        private void Awake()
        {
            Instance = this;
            
            // NOTE: Will most likely need to load in the miniature's data from a scene save slot later:
            RegisteredMiniatures ??= new List<Miniature>();
        }
        
        private void OnEnable()
        {
            InputManager.OnMouseDown += OnMouseDown;
            InputManager.OnMouseHold += OnMouseHold;
            InputManager.OnMouseHoldCancelled += OnHoldRelease;
            InputManager.OnMouseUp += OnMouseUp;
            UIManager.DMTKUISelected += OnUISelected;
            UIManager.DMTKUIDeselected += OnUIDeselected;
        }

        private void OnDisable()
        {
            InputManager.OnMouseDown -= OnMouseDown;
            InputManager.OnMouseHold -= OnMouseHold;
            InputManager.OnMouseHoldCancelled -= OnHoldRelease;
            InputManager.OnMouseUp -= OnMouseUp;
            UIManager.DMTKUISelected -= OnUISelected;
            UIManager.DMTKUIDeselected -= OnUIDeselected;
        }

        private void FixedUpdate()
        {
            RaycastMouseOnUI();
        }
        
        #endregion

        #region InputFunctions

        private void OnMouseDown()
        {
            if (_uiSelected) return;
            if (_isMiniatureSelected) return;
            RadialManager.Instance.Disable();
        }

        private void OnMouseUp()
        {
            if (_uiSelected) return;
            if (!_isMiniatureSelected) return;
            RadialManager.Instance.MiniatureClicked(SelectedMiniature.Data);
        }
        
        private void OnMouseHold()
        {
            if (_uiSelected) return;
            if (!_isMiniatureSelected) return;
            if (_isMiniatureGrabbed) return;
            
            RadialManager.Instance.Disable();
            SelectedMiniature.OnGrab();
            GrabbedMiniature = SelectedMiniature;
            _isMiniatureGrabbed = true;
        }
        
        /// <summary> If any miniature is grabbed, release it.</summary>
        private void OnHoldRelease()
        {
            if (!_isMiniatureGrabbed) return;
            GrabbedMiniature.OnRelease();
            GrabbedMiniature = null;
            _isMiniatureGrabbed = false;
        }
        
        #endregion

        #region ManagerFunctions
        
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
                SelectedMiniature = miniature;
                _isMiniatureSelected = true;
                return;
            }

            SelectedMiniature = null;
            _isMiniatureSelected = false;
        }

        private void OnUISelected()
        {
            _uiSelected = true;
        }
        
        private void OnUIDeselected()
        {
            _uiSelected = false;
        }
        #endregion
    }
}
