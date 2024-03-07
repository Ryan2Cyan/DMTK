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
        public TabletopCell CurrentCell;
        public MiniatureType Type;
        public bool IsHidden;
        
        private MeshFilter _meshFilter;
        private Transform _miniatureTransform;
        
        #region UnityFunctions

        private void Start()
        {
            ApplyGridScale();
            Tabletop.Instance.RegisterMiniature(this);
            if (!Tabletop.Instance.AssignClosestToGridCentre(ref CurrentCell)) 
            {
                Debug.Log("Unable" + " to spawn miniature as all grid cells are occupied.");
            }
            else SetCell(CurrentCell);
        }

        private void OnDestroy()
        {
            // Unregister this miniature from the tabletop as it no longer needs to be saved.
            Tabletop.Instance.UnregisterMiniature(this);
        }

        #endregion

        /// <summary>
        /// Sets the CurrentCell and updates the miniature to correspond to the cell assigned.
        /// </summary>
        /// <param name="cell">Assigned cell.</param>
        public void SetCell(TabletopCell cell)
        {
            if (cell == null) return;
            CurrentCell = cell;
            CurrentCell.IsOccupied = true;
            transform.position = new Vector3(cell.Position.x, 0f, cell.Position.y);
        }
        
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