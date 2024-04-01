using System.Collections.Generic;
using Input;
using UI;
using UI.Miniature_Radial;
using UnityEngine;
using Utility;

namespace Tabletop.Miniatures
{
    public class MiniatureManager : MonoBehaviour
    {
        public static MiniatureManager Instance;
        public List<Miniature> RegisteredMiniatures;

        private void Awake()
        {
            Instance = this;
            
            // NOTE: Will most likely need to load in the miniature's data from a scene save slot later:
            RegisteredMiniatures ??= new List<Miniature>();
        }
        
        private void OnEnable()
        {
            InputManager.OnMouseHold += CheckGrabMiniature;
            InputManager.OnMouseHoldCancelled += CheckReleaseMiniature;
            InputManager.OnMouseUp += CheckClickMiniature;
        }

        private void OnDisable()
        {
            InputManager.OnMouseHold -= CheckGrabMiniature;
            InputManager.OnMouseHoldCancelled -= CheckReleaseMiniature;
            InputManager.OnMouseUp -= CheckClickMiniature;
        }

        /// <summary>Cache miniature to be saved.</summary>
        /// <param name="miniature">Miniature to register.</param>
        public void RegisterMiniature(Miniature miniature)
        {
            RegisteredMiniatures.Add(miniature);
        }

        /// <summary>
        /// Remove miniature from cache (will no longer be saved).</summary>
        /// <param name="miniature">Miniature to unregister.</param>
        public void UnregisterMiniature(Miniature miniature)
        {
            RegisteredMiniatures.Remove(miniature);
        }

        private void CheckClickMiniature()
        {
            var hit = DMTKPhysicsUtility.PhysicsMouseRayCast();
            foreach (var miniature in RegisteredMiniatures)
            {
                if(miniature.Grabbed) continue;
                if (miniature.Collider != hit.collider) continue;
                RadialUIManager.Instance.MiniatureClicked(miniature.Data);
                return;
            }
        }
        
        /// <summary> Perform physics ray cast and check if a cursor is selecting a registered miniature. If selected
        /// grab it.</summary>
        private void CheckGrabMiniature()
        {
            RadialUIManager.Instance.MiniatureGrabbed();
            var hit = DMTKPhysicsUtility.PhysicsMouseRayCast();
            foreach (var miniature in RegisteredMiniatures)
            {
                if (miniature.Collider != hit.collider) continue;
                miniature.OnGrab();
                return;
            }
        }
        
        /// <summary> If any miniature is grabbed, release it.</summary>
        private void CheckReleaseMiniature()
        {
            foreach (var miniature in RegisteredMiniatures)
            {
                if (!miniature.Grabbed) continue;
                miniature.OnRelease();
                return;
            }
        }
    }
}
