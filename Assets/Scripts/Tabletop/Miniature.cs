using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tabletop
{
    public enum MiniatureType
    {
        Player,
        Monster,
        Prop,
        NPC
    }

    // [CreateAssetMenu(menuName = "DMTK/Miniature", fileName = "Miniature", order = 0)]
    public class Miniature : MonoBehaviour
    {
        public string Label;
        public Vector2 CurrentCell;
        public MiniatureType Type;
        public bool IsHidden;
        
        private MeshFilter _meshFilter;
        private Transform _miniatureTransform;
        
        #region UnityFunctions

        private void Start()
        {
            ApplyGridScale();
            
            // Register miniature to the Tabletop for saving purposes.
            Tabletop.Instance.RegisterMiniature(this);
        }

        private void OnDestroy()
        {
            // Unregister this miniature from the tabletop as it no longer needs to be saved.
            Tabletop.Instance.UnregisterMiniature(this);
        }

        #endregion

        /// <summary>
        /// Change the local scale of the GameObject containing the mesh to ensure it fits within a grid cell.
        /// </summary>
        private void ApplyGridScale()
        {
            if(_meshFilter == null) _meshFilter = GetComponentInChildren<MeshFilter>();
            if(_miniatureTransform == null) _miniatureTransform = transform.GetChild(0).transform;
            var mesh = _meshFilter.mesh;
            _miniatureTransform.localScale = Tabletop.Instance.GenerateMeshToGridScale(mesh.bounds.size);
        }
    }
}