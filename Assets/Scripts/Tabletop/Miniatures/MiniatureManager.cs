using System.Collections.Generic;
using General;
using Input;
using UI;
using UI.Miniature_Data;
using UI.Miniature_Radial;
using UnityEngine;
using UnityEngine.Serialization;
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
        public bool UIClicked;

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
            UIManager.DMTKUISelected += OnUISelected;
            UIManager.DMTKUIDeselected += OnUIDeselected;
            UIManager.DMTKUIMouseDown += OnUIMouseDown;
        }

        private void OnDisable()
        {
            InputManager.OnMouseUp -= OnMouseUp;
            InputManager.OnMouseHold -= OnMouseHold;
            InputManager.OnMouseHoldCancelled -= OnHoldRelease;
            UIManager.DMTKUISelected -= OnUISelected;
            UIManager.DMTKUIDeselected -= OnUIDeselected;
            UIManager.DMTKUIMouseDown -= OnUIMouseDown;
        }
        
        #endregion

        #region InputFunctions

        private void OnMouseUp()
        {
            InputManager.Instance.QueueInputFunction(MouseUp);
        }

        private void MouseUp()
        {
            // Disable radial if clicking anywhere but a UI element:
            Debug.Log("MouseUp: " + UIClicked);
            if (Instance.UIClicked)
            {
                Instance.UIClicked = false;
                return;
            }
            if (SelectedMiniature == null) RadialManager.Instance.Disable();
            else RadialManager.Instance.MiniatureClicked(SelectedMiniature.Data);
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
                if(!_uiSelected) SelectedMiniature.ToggleOutline(true);
                _isMiniatureSelected = true;
                return;
            }

            if(_isMiniatureSelected) SelectedMiniature.ToggleOutline(false);
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
        
        private void OnUIMouseDown()
        {
            UIClicked = true;
        }
        #endregion
    }
}
