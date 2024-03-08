using UnityEngine;
using UnityEngine.InputSystem;

namespace Utility
{
    public static class Utility
    {
    }

    public static class DMTKPhysicsUtility
    {
        /// <summary>
        /// Performs a physics ray cast on the mouse's current position detecting any colliders that have intersected. 
        /// </summary>
        /// <returns>If an intersection occured the hit information is returned, otherwise empty data is returned. </returns>
        public static RaycastHit PhysicsMouseRayCast()
        {
            if (Camera.main == null) return new RaycastHit();
            return Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out var hit) ? hit : new RaycastHit();
        }
        
        public static RaycastHit PhysicsMouseRayCast(LayerMask layerMask)
        {
            if (Camera.main == null) return new RaycastHit();
            return Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out var hit, layerMask) ? hit : new RaycastHit();
        }
    }
}
