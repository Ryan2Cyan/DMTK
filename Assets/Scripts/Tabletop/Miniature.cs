using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tabletop
{
    public enum MiniatureType
    {
        Player,
        Monster,
        Prop,
        NPC
    }
    
    public class Miniature : MonoBehaviour
    {
        [NonSerialized] public BoxCollider Collider; 
        public TabletopCell CurrentCell;
        public bool Grabbed;
        
        private MeshFilter _meshFilter;
        private IEnumerator _currentRoutine;
        private Transform _miniatureTransform;
        private const float _grabbedYOffset = 0.5f;

        #region UnityFunctions

        private void Start()
        {
            if(_meshFilter == null) _meshFilter = GetComponentInChildren<MeshFilter>();
            if(_miniatureTransform == null) _miniatureTransform = transform.GetChild(0).transform;
            if (_miniatureTransform.GetComponent<BoxCollider>() == null) Collider = _miniatureTransform.gameObject.AddComponent<BoxCollider>();
            
            // Scale the miniature to be coherent with the tabletop grid:
            var mesh = _meshFilter.mesh;
            _miniatureTransform.localScale = Tabletop.Instance.GenerateMeshToGridScale(mesh.bounds.size);
            
            MiniatureManager.Instance.RegisterMiniature(this);
            if (!Tabletop.Instance.AssignClosestToGridCentre(ref CurrentCell)) Debug.Log("Unable" + " to spawn miniature as all grid cells are occupied.");
            else
            {
                SetCell(CurrentCell);
                transform.position = new Vector3(CurrentCell.Position.x, 0f, CurrentCell.Position.y);
            }
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
            Grabbed = true;
            var cellPosition = CurrentCell.Position;
            _currentRoutine = LerpPosition(new Vector3(cellPosition.x, transform.position.y, cellPosition.y), 
                new Vector3(cellPosition.x, _grabbedYOffset, cellPosition.y), 0.2f);
            StopCoroutine(_currentRoutine);
            StartCoroutine(_currentRoutine);
        }
        
        /// <summary>
        /// Called when the user unselects with left-mouse click.
        /// </summary>
        public void OnRelease()
        {
            Grabbed = false;
            var cellPosition = CurrentCell.Position;
            CurrentCell.SetState(CellAppearance.Enabled);
            StopCoroutine(_currentRoutine);
            _currentRoutine = LerpPosition(new Vector3(cellPosition.x, transform.position.y, cellPosition.y), 
                new Vector3(cellPosition.x, 0f, cellPosition.y), 0.2f);
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
                transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / executionTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }  
            transform.position = endPosition;
            if (Grabbed) StartCoroutine(MoveToGrabbedPosition());
            yield return null;   
        }
        
        /// <summary>
        /// Moves to the current mouse position on the tabletop if a valid position is found. Cannot move to an already
        /// occupied cell.
        /// </summary>
        private IEnumerator MoveToGrabbedPosition()
        {
            var currentCell = CurrentCell;
            while (Grabbed)
            {
                var mousePosition = transform.position;
                var valid = Tabletop.Instance.GetTabletopMousePosition(ref mousePosition);
                if (valid)
                {
                    transform.position = new Vector3(mousePosition.x, transform.position.y, mousePosition.z);
                    var newCell = Tabletop.Instance.GetClosestNeighboringCell(currentCell, new Vector2(transform.position.x, transform.position.z));
                    if (newCell != currentCell)
                    {
                        // Ensure the cell isn't taken by another miniature:
                        if (!newCell.IsOccupied) SetCell(newCell);
                        currentCell = newCell;
                    }
                }

                yield return null;
            }  
            yield return null;   
        }
        
        /// <summary>
        /// Sets the CurrentCell and updates the miniature to correspond to the cell assigned.
        /// </summary>
        /// <param name="cell">Assigned cell.</param>
        private void SetCell(TabletopCell cell)
        {
            if (cell == null) return;
            if(CurrentCell != null) CurrentCell.SetState(CellAppearance.Disabled);
            CurrentCell = cell;
            CurrentCell.SetState(CellAppearance.Enabled);
        }
    }
}