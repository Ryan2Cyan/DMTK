using System.Collections.Generic;
using Input;
using UnityEngine;
using Utility;

namespace Tabletop
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
            // Register to input events:
            InputManager.OnMouseDown += CheckGrabMiniature;
            InputManager.OnMouseUp += CheckReleaseMiniature;
        }

        private void OnDisable()
        {
            // Unregister to input events:
            InputManager.OnMouseDown -= CheckGrabMiniature;
            InputManager.OnMouseUp -= CheckReleaseMiniature;
        }

        /// <summary>
        /// Registers miniature into the current scene's database. This means the miniature's position and details
        /// will be saved.
        /// </summary>
        /// <param name="miniature">The registered miniature.</param>
        public void RegisterMiniature(Miniature miniature)
        {
            RegisteredMiniatures.Add(miniature);
        }

        /// <summary>
        /// Will unregister miniature from the current scene's database. It has been removed and its data will no longer be
        /// saved.
        /// </summary>
        /// <param name="miniature">The unregistered miniature.</param>
        public void UnregisterMiniature(Miniature miniature)
        {
            RegisteredMiniatures.Remove(miniature);
        }
    
        /// <summary>
        /// Event listener function that performs a physics ray cast, checking if the user's cursor is selecting
        /// a miniature within the scene.
        /// </summary>
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
        
        /// <summary>
        /// Event listener function that performs a physics ray cast, checking if the user's cursor is selecting
        /// a miniature within the scene.
        /// </summary>
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
