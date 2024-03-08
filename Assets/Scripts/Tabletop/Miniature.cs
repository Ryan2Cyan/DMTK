using System;
using System.Collections;
using SaveData;
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
        [NonSerialized] public BoxCollider Collider;
        public TabletopCell CurrentCell;
        public bool Grabbed;
        
        private MeshFilter _meshFilter;
        private IEnumerator _currentRoutine;
        private Transform _miniatureTransform;
        private const float GrabbedYOffset = 0.5f;

        #region UnityFunctions

        private void Start()
        {
            if(_meshFilter == null) _meshFilter = GetComponentInChildren<MeshFilter>();
            if(_miniatureTransform == null) _miniatureTransform = transform.GetChild(0).transform;
            if (_miniatureTransform.GetComponent<BoxCollider>() == null) Collider = _miniatureTransform.gameObject.AddComponent<BoxCollider>();
            
            ApplyGridScale();
            MiniatureManager.Instance.RegisterMiniature(this);
            if (!Tabletop.Instance.AssignClosestToGridCentre(ref CurrentCell)) Debug.Log("Unable" + " to spawn miniature as all grid cells are occupied.");
            else SetCell(CurrentCell);
        }

        private void OnDestroy()
        {
            // Unregister this miniature from the tabletop as it no longer needs to be saved.
            MiniatureManager.Instance.UnregisterMiniature(this);
        }

        #endregion

        /// <summary>
        /// Called when the user selects with left-mouse click.
        /// </summary>
        public void OnGrab()
        {
            Debug.Log("Grabbed");
            Grabbed = true;
            var cellPosition = CurrentCell.Position;
            _currentRoutine = LerpPosition(new Vector3(cellPosition.x, transform.position.y, cellPosition.y), 
                new Vector3(cellPosition.x, GrabbedYOffset, cellPosition.y), 0.2f);
            StopCoroutine(_currentRoutine);
            StartCoroutine(_currentRoutine);
        }
        
        /// <summary>
        /// Called when the user unselects with left-mouse click.
        /// </summary>
        public void OnRelease()
        {
            Debug.Log("Released");
            Grabbed = false;
            var cellPosition = CurrentCell.Position;
            _currentRoutine = LerpPosition(new Vector3(cellPosition.x, transform.position.y, cellPosition.y), 
                new Vector3(cellPosition.x, 0f, cellPosition.y), 0.2f);
            StopCoroutine(_currentRoutine);
            StartCoroutine(_currentRoutine);
        }
        
        /// <summary>
        /// Coroutine that lerps the position of the miniature to a specified new position over a specified execution time.
        /// </summary>
        /// <param name="startPosition">Starting position of the lerp.</param>
        /// <param name="endPosition">End position of the lerp.</param>
        /// <param name="executionTime">Duration of the lerp.</param>
        /// <returns></returns>
        private IEnumerator LerpPosition(Vector3 startPosition, Vector3 endPosition, float executionTime)
        {
            var elapsedTime = 0f;
            while (elapsedTime < executionTime)
            {
                Debug.Log("Move Y: " + transform.position.y);
                transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / executionTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }  
            transform.position = endPosition;
            yield return null;   
        }
        
        /// <summary>
        /// Sets the CurrentCell and updates the miniature to correspond to the cell assigned.
        /// </summary>
        /// <param name="cell">Assigned cell.</param>
        private void SetCell(TabletopCell cell)
        {
            if (cell == null) return;
            CurrentCell = cell;
            CurrentCell.SetState(CellState.Occupied);
            transform.position = new Vector3(cell.Position.x, 0f, cell.Position.y);
        }
        
        /// <summary>
        /// Change the local scale of the GameObject containing the mesh to ensure it fits within a grid cell.
        /// </summary>
        private void ApplyGridScale()
        {
            var mesh = _meshFilter.mesh;
            _miniatureTransform.localScale = Tabletop.Instance.GenerateMeshToGridScale(mesh.bounds.size);
        }
    }
}