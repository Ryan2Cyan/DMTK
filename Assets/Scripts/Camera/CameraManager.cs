using System.Collections.Generic;
using UI.Miniature_Radial;
using UI.UI_Interactables;
using UnityEngine;

namespace Camera
{
    public class CameraManager : MonoBehaviour
    {
        public static CameraManager Instance;
        
        [Header("Components")]
        public UnityEngine.Camera MainCamera;
        
        [Header("UI Components")]
        public DMTKToggleOptions PresetOptions;
        public RadialToggle OrthographicRadialToggle;

        [Header("Settings")] 
        public float PerspectiveNearClip;
        public float OrthoNearClip;
        public float OrthoSize;
        
        private List<Vector3> _presetPositions = new();
        

        private const float _topVerticalOffset = 12f;
        private const float _middleVerticalOffset = 9f;
        private const float _topHorizontalOffset = 4f;
        private const float _middleHorizontalOffset = 8f;

        private void Awake()
        {
            Instance = this;
            
            var tabletopSize = (Vector2)Tabletop.Tabletop.Tabletop.Instance.TabletopSize;
            var gridScale = tabletopSize * Tabletop.Tabletop.Tabletop.Instance.CellSpacing;
            var halfGridScale = gridScale / 2f;
            var gridCentre = Tabletop.Tabletop.Tabletop.Instance.transform.position;
            
            _presetPositions = new List<Vector3>()
            {
                new (gridCentre.x, gridCentre.y + _middleVerticalOffset * 2f, gridCentre.z), // Top
                
                new (gridCentre.x - (halfGridScale.x + _topHorizontalOffset), gridCentre.y + _topVerticalOffset, gridCentre.z - (halfGridScale.y + _topHorizontalOffset)), // Top, Back-Left
                new (gridCentre.x - (halfGridScale.x + _topHorizontalOffset), gridCentre.y + _topVerticalOffset, gridCentre.z + (halfGridScale.y + _topHorizontalOffset)), // Top, Back-Right
                new (gridCentre.x + (halfGridScale.x + _topHorizontalOffset), gridCentre.y + _topVerticalOffset, gridCentre.z + (halfGridScale.y + _topHorizontalOffset)), // Top, Front-Right
                new (gridCentre.x + (halfGridScale.x + _topHorizontalOffset), gridCentre.y + _topVerticalOffset, gridCentre.z - (halfGridScale.y + _topHorizontalOffset)), // Top, Front-Left
                
                new (gridCentre.x, gridCentre.y + _middleVerticalOffset, gridCentre.z - (halfGridScale.y + _middleHorizontalOffset)), // Middle, Left
                new (gridCentre.x  + (halfGridScale.x + _middleHorizontalOffset), gridCentre.y + _middleVerticalOffset, gridCentre.z), // Middle, Front
                new (gridCentre.x, gridCentre.y + _middleVerticalOffset, gridCentre.z + (halfGridScale.y + _middleHorizontalOffset)), // Middle, Right
                new (gridCentre.x - (halfGridScale.x + _middleHorizontalOffset), gridCentre.y + _middleVerticalOffset, gridCentre.z), // Middle, Back
            };

            MainCamera.orthographicSize = OrthoSize;
            SetCameraPresetPosition();
        }

        public void SetCameraPresetPosition()
        {
            // Set preset position:
            MainCamera.transform.position = _presetPositions[PresetOptions.SelectedIndex];
            
            // Have camera look at centre of tabletop grid:
            MainCamera.transform.LookAt(Tabletop.Tabletop.Tabletop.Instance.transform);
        }

        public void ToggleOrthographic()
        {
            if (OrthographicRadialToggle.Toggle)
            {
                // Orthographic:
                MainCamera.orthographic = true;
                MainCamera.nearClipPlane = OrthoNearClip;
            }
            else
            {
                // Perspective:
                MainCamera.orthographic = false;
                MainCamera.nearClipPlane = PerspectiveNearClip;
            }
        }
    }
}
