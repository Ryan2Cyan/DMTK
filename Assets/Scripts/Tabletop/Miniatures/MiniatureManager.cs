using System.Collections.Generic;
using Input;
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
            InputManager.OnMouseDown += CheckGrabMiniature;
            InputManager.OnMouseUp += CheckReleaseMiniature;
        }

        private void OnDisable()
        {
            InputManager.OnMouseDown -= CheckGrabMiniature;
            InputManager.OnMouseUp -= CheckReleaseMiniature;
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
    
        /// <summary> Perform physics ray cast and check if a cursor is selecting a registered miniature. If selected
        /// grab it.</summary>
        private void CheckGrabMiniature()
        {
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
