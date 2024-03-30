using System;
using System.Collections;
using System.Collections.Generic;
using Tabletop.Tabletop;
using UI;
using UnityEngine;

namespace Tabletop.Miniatures
{
    public class Miniature : MonoBehaviour
    {
        [NonSerialized] public BoxCollider Collider; 
        public TabletopCell CurrentCell;
        public bool Grabbed;
        
        private MeshFilter _meshFilter;
        private IEnumerator _currentRoutine;
        private Transform _miniatureTransform;
        private const float _grabbedYOffset = 0.5f;
        private const float _grabExecutionTime = 0.05f;

        #region UnityFunctions
        private void Start()
        {
            if(_meshFilter == null) _meshFilter = GetComponentInChildren<MeshFilter>();
            if(_miniatureTransform == null) _miniatureTransform = transform.GetChild(0).transform;
            if (_miniatureTransform.GetComponent<BoxCollider>() == null) Collider = gameObject.GetComponent<BoxCollider>();
            
            // Assign to the closest available cell to the centre:
            if (!Tabletop.Tabletop.Instance.AssignClosestToGridCentre(ref CurrentCell)) Debug.Log("Unable" + " to spawn miniature as all grid cells are occupied.");
            else
            {
                SetCurrentCell(CurrentCell);
                transform.position = new Vector3(CurrentCell.Position.x, 0f, CurrentCell.Position.y);
            }
            
            MiniatureManager.Instance.RegisterMiniature(this);
        }

        private void OnDestroy()
        {
            MiniatureManager.Instance.UnregisterMiniature(this);
        }

        #endregion

        #region PublicFunctions
        /// <summary> Called when the user selects with left-mouse, moving up on the y-axis.</summary>
        public void OnGrab()
        {
            Grabbed = true;
            var cellPosition = CurrentCell.Position;
            _currentRoutine = LerpPosition(new Vector3(cellPosition.x, transform.position.y, cellPosition.y), 
                new Vector3(cellPosition.x, _grabbedYOffset, cellPosition.y), _grabExecutionTime);
            StopCoroutine(_currentRoutine);
            StartCoroutine(_currentRoutine);
        }
        
        /// <summary>Called when the user releases left-mouse.</summary>
        public void OnRelease()
        {
            Grabbed = false;
        }
        #endregion

        #region PrivateFunctions
        /// <summary>Smoothly move to new position over a specified time.</summary>
        /// <param name="startPosition">Starting position before moving.</param>
        /// <param name="endPosition">Resulting position after moving.</param>
        /// <param name="executionTime">Duration of time the move occurs.</param>
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
            if(Grabbed) StartCoroutine(MoveToGrabbedPosition());
            else
            {
                if (transform.position.y != 0f)
                {
                    StopCoroutine(_currentRoutine);
                    var cellPosition = CurrentCell.Position;
                    StartCoroutine(_currentRoutine = LerpPosition(
                        new Vector3(cellPosition.x, transform.position.y, cellPosition.y),
                        new Vector3(cellPosition.x, 0f, cellPosition.y), _grabExecutionTime));
                }
            }
            yield return null;   
        }
        
        /// <summary> Move to mouse position (if valid). Cannot move to occupied cells.</summary>
        private IEnumerator MoveToGrabbedPosition()
        {
            var tabletopInstance = Tabletop.Tabletop.Instance;
            
            // Grab the position of the mouse as soon as mini is grabbed:
            var previousMousePosition = Vector3.zero;
            Tabletop.Tabletop.GetTabletopMousePosition(ref previousMousePosition);
            var miniTransform = transform;
            var miniPosition = miniTransform.position;
            miniPosition = new Vector3(previousMousePosition.x, miniPosition.y, previousMousePosition.z);
            
            miniTransform.position = miniPosition;
            
            var startingCell = CurrentCell;
            var currentCell = CurrentCell;
            
            // Fetch distance arrow UI to display traversed path:
            var path = new List<TabletopCell>{ startingCell };
            var distanceIndicator = (DistanceTravelled) tabletopInstance.DistanceIndicatorsPool.GetPooledObject();
            distanceIndicator.Target = transform;
            
            while (Grabbed)
            {
                // Check mouse position is valid (on the tabletop):
                var mousePosition = Vector3.zero;
                var valid = Tabletop.Tabletop.GetTabletopMousePosition(ref mousePosition);
                if (valid)
                {
                    // Move to mouse position:
                    var moveVector = mousePosition - previousMousePosition;
                    miniPosition = new Vector3(miniPosition.x + moveVector.x, miniPosition.y, miniPosition.z + moveVector.z);
                    miniTransform.position = miniPosition;
                    
                    // Check mouse position is in different cell:
                    var newCell = tabletopInstance.GetClosestNeighboringCell(currentCell, new Vector2(mousePosition.x, mousePosition.z), -2, 2, -2, 2);
                    if (newCell != currentCell)
                    {
                        if (!newCell.IsOccupied)
                        {
                            currentCell = newCell;
                            
                            // Calculate the path the miniature has traversed (AStar):
                            foreach (var cell in path) cell.SetCellState(cell.DisabledState);
                            path = tabletopInstance.GetShortestPath(startingCell, currentCell);
                            distanceIndicator.Distance = (path.Count - 1) * tabletopInstance.DistancePerCell;
                        }
                    }
                    previousMousePosition = mousePosition;
                }
                
                Tabletop.Tabletop.DisplayDistanceArrow(path);
                yield return null;
            }  
            
            // Reset cells within the path:
            foreach (var cell in path) cell.SetCellState(cell.DisabledState);
            
            // Set new current cell once dropped:
            SetCurrentCell(currentCell);
            var cellPosition = CurrentCell.Position;
            StopCoroutine(_currentRoutine);
            StartCoroutine(_currentRoutine = LerpPosition(new Vector3(cellPosition.x, transform.position.y, cellPosition.y), 
                new Vector3(cellPosition.x, 0f, cellPosition.y), _grabExecutionTime));
            Tabletop.Tabletop.Instance.DistanceIndicatorsPool.ReleasePooledObject(distanceIndicator);
            yield return null;   
        }
        
        private void SetCurrentCell(TabletopCell cell)
        {
            if (cell == null) return;
            if(CurrentCell != null) CurrentCell.SetCellState(CurrentCell.DisabledState);
            CurrentCell = cell;
            CurrentCell.SetCellState(CurrentCell.OccupiedState);
        }
        #endregion
    }
}