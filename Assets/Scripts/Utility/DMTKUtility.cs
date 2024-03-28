using UnityEngine;
using UnityEngine.InputSystem;

namespace Utility
{
    public static class DMTKUtility
    {
        public static bool IsInsideRange(float min, float max, float value)
        {
            if (value < min) return false;
            return !(value > max);
        }
        
        public static bool IsInsideRange(Vector2 bottomLeft, Vector2 topRight, Vector2 position)
        {
            if (position.x < bottomLeft.x) return false;
            if (position.y < bottomLeft.y) return false;
            if (position.x > topRight.x) return false;
            return !(position.y > topRight.y);
        }
    }

    public static class DMTKPhysicsUtility
    {
        /// <summary>Physics ray cast on current mouse position.</summary>
        /// <returns>Hit intersection information if hit, otherwise empty data.</returns>
        public static RaycastHit PhysicsMouseRayCast()
        {
            var mainCamera = Camera.main;
            if (!mainCamera) return new RaycastHit();
            return Physics.Raycast(mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()), out var hit) ? hit : new RaycastHit();
        }
        
        /// <summary>Physics ray cast on current mouse position.</summary>
        /// <param name="layerMask">Filter collider search.</param>
        /// <returns>Hit intersection information if hit, otherwise empty data.</returns>
        public static RaycastHit PhysicsMouseRayCast(LayerMask layerMask)
        {
            var mainCamera = Camera.main;
            if (!mainCamera) return new RaycastHit();
            return Physics.Raycast(mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()), out var hit, 100f, layerMask) ? hit : new RaycastHit();
        }
    }
}
