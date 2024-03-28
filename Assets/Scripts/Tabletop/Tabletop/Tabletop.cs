using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Utility;

namespace Tabletop.Tabletop
{
    public class Tabletop : MonoBehaviour
    {
        public static Tabletop Instance;
        
        [Header("Settings")]
        public Color TabletopColour;
        public Vector2Int TabletopSize;
        public float CellSpacing;
        public float DistancePerCell;
        
        [Header("Required")]
        public ObjectPool DistanceIndicatorsPool;
        public GameObject CellPrefab;
        public LayerMask TabletopLayerMask;
        [HideInInspector] public Vector2 GridMinimumPosition;
        [HideInInspector] public Vector2 GridMaximumPosition;
        
        private List<List<TabletopCell>> _gridCells;
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        private const int _diagonalMoveValue = 14;
        private const int _nonDiagonalMoveValue = 10;
        
        
        #region UnityFunctions
        private void Awake()
        {
            Instance = this;
            if(_meshFilter == null) _meshFilter = GetComponent<MeshFilter>();
            if(_meshRenderer == null) _meshRenderer = GetComponent<MeshRenderer>();
            _gridCells = new List<List<TabletopCell>>();
            _meshFilter.mesh = GenerateAsymmetricalGridMesh(TabletopSize, CellSpacing, ref _gridCells);
            _meshRenderer.material = new Material(Shader.Find("Unlit/NewUnlitShader")) { color = TabletopColour };
            
            // Calculate minimum (bottom-left) and maximum (top-right) positions of the grid:
            var position = transform.position;
            var distanceFromOrigin = new Vector2(position.x + TabletopSize.x / 2f * CellSpacing, position.z + TabletopSize.y / 2f * CellSpacing);
            GridMinimumPosition = new Vector2(-distanceFromOrigin.x, -distanceFromOrigin.y);
            GridMaximumPosition = new Vector2(distanceFromOrigin.x, distanceFromOrigin.y);
        }
        #endregion

        #region PublicFunctions
        /// <summary>
        /// Attempts to assign the closest unoccupied cell within the grid. This is done via distance checks
        /// using each cell's central position and comparing it to the centre of the tabletop grid.
        /// </summary>
        /// <param name="miniatureCell">Inputted cell to be assigned.</param>
        /// <returns>True if a corresponding cell was found, false if all cells in the tabletop are occupied.</returns>
        public bool AssignClosestToGridCentre(ref TabletopCell miniatureCell)
        {
            var closestDistance = float.PositiveInfinity;
            var tabletopPosition = transform.position;
            foreach (var rows in _gridCells)
            {
                foreach (var cell in rows)
                {
                    if(cell.IsOccupied) continue;
                    var distance = Vector2.Distance(new Vector2(tabletopPosition.x, tabletopPosition.z), cell.Position);
                    if (distance >= closestDistance) continue;
                    closestDistance = distance;
                    miniatureCell = cell;
                }
            }

            return miniatureCell != null;
        }

        /// <summary>Finds closest neighboring cell relative to a cell position.</summary>
        /// <param name="currentCell">Central cell of which neighboring cells will derive from.</param>
        /// <param name="currentPosition">Current world position.</param>
        /// <param name="minX"></param>
        /// <param name="maxX"></param>
        /// <param name="minY"></param>
        /// <param name="maxY"></param>
        /// <returns>The closest cell to the inputted 'currentPosition'.</returns>
        public TabletopCell GetClosestNeighboringCell(TabletopCell currentCell, Vector2 currentPosition, int minX, int maxX, int minY, int maxY)
        {
            // Get neighbours function excludes the current cell, so using it as a base:
            var closestDistance = Vector2.Distance(currentPosition, currentCell.Position);
            var closestCell = currentCell.Coordinate;
            
            var foundCell = false;
            foreach (var neighbour in GetNeighbouringCellCoordinates(currentCell.Coordinate, minX, maxX, minY, maxY))
            {
                // Check if closer that the current closest:
                var distance = Vector2.Distance(currentPosition, _gridCells[neighbour.x][neighbour.y].Position);
                if (distance >= closestDistance) continue;
                
                // If closer, cache neighbour:
                closestDistance = distance;
                closestCell = neighbour;
                foundCell = true;
            }

            return !foundCell ? currentCell : _gridCells[closestCell.x][closestCell.y];
        }

        /// <summary>For a given coordinate, get all neighboring cell coordinates.</summary>
        /// <param name="currentCellCoordinate">Origin coordinate to begin search.</param>
        /// <param name="minX">Minimum x-axis search range.</param>
        /// <param name="maxX">Maximum x-axis search range.</param>
        /// <param name="minY">Minimum y-axis search range.</param>
        /// <param name="maxY">Maximum y-axis search range.</param>
        /// <returns>All valid neighbouring cell coordinates within search range.</returns>
        /// <remarks>Source: https://github.com/SebLague/Pathfinding/blob/master/Episode%2003%20-%20astar/Assets/Scripts/Grid.cs#L35</remarks>
        public static List<Vector2Int> GetNeighbouringCellCoordinates(Vector2Int currentCellCoordinate, int minX, int maxX, int minY, int maxY)
        {
            var neighbours = new List<Vector2Int>();
            for (var x = minX; x <= maxX; x++)
            {
                for (var y = minY; y <= maxY; y++)
                {
                    if(x == 0 && y == 0) continue;
                    var check = new Vector2Int(currentCellCoordinate.x + x, currentCellCoordinate.y + y);
                    if(check.x >= 0 && check.x < Instance.TabletopSize.x && check.y >= 0 && check.y < Instance.TabletopSize.y) neighbours.Add(check);
                }
            }
            return neighbours;
        }
        
        /// <summary>Attempt to return mouse position in tabletop grid space.</summary>
        /// <param name="position">Store result tabletop space position.</param>
        /// <returns>True if tabletop space position is found, false otherwise.</returns>
        public static bool GetTabletopMousePosition(ref Vector3 position)
        {
            var hit = DMTKPhysicsUtility.PhysicsMouseRayCast(Instance.TabletopLayerMask);
            if (!hit.transform) return false;
            position = hit.point;
            return true;
        }
        
        /// <summary>Get shortest path between two cells.</summary>
        /// <param name="start">Starting cell.</param>
        /// <param name="end">End cell.</param>
        /// <returns>Each cell within the shortest path.</returns>
        public List<TabletopCell> GetShortestPath(TabletopCell start, TabletopCell end)
        {
            return DistanceArrowPathfinder.AStarPathfinder(start, end, _gridCells);
        }

        public static void DisplayDistanceArrow(List<TabletopCell> path)
        {
            switch (path.Count)
            {
                case < 1:
                    return;
                case <= 1:
                    path.First().SetCellState(path.First().PathStartIdleState);
                    return;
            }

            var lastIndex = path.Count - 1;
            
            // Set beginning of arrow:
            path.First().PathStartState.Direction = GetCellTraverseDirection(path[0].Coordinate, path[1].Coordinate);
            path.First().SetCellState(path.First().PathStartState);

            // Set end of arrow:
            path.Last().PathEndState.Direction = GetCellTraverseDirection(path[lastIndex - 1].Coordinate, path[lastIndex].Coordinate);
            path.Last().SetCellState(path.Last().PathEndState);
            
            // Set body of arrow:
            for (var i = 1; i < lastIndex; i++)
            {
                var currentCell = path[i];
                currentCell.PathState.Direction0 = GetCellTraverseDirection(currentCell.Coordinate, path[i - 1].Coordinate);
                currentCell.PathState.Direction1 = GetCellTraverseDirection(currentCell.Coordinate, path[i + 1].Coordinate);
                currentCell.SetCellState(currentCell.PathState);
            }
        }
        
        #endregion
        
        #region PrivateFunctions
        /// <summary>Generate asymmetrical grid mesh with at the centre. Unique index generated for each vertex.</summary>
        /// <param name="gridSize">Grid dimensions, the size.x being width (x), size.y being depth (z).</param>
        /// <param name="spacing">Distance between cells.</param>
        /// <param name="cellPositions">Store position of each cell. </param>
        /// <returns>Generated asymmetrical grid mesh. Can be assigned to the mesh of a MeshFilter component.</returns>
        /// <remarks>Source: https://gist.github.com/mdomrach/a66602ee85ce45f8860c36b2ad31ea14</remarks>
        private Mesh GenerateAsymmetricalGridMesh(Vector2Int gridSize, float spacing, [NotNull] ref List<List<TabletopCell>> cellPositions)
        {
            if (cellPositions == null) throw new ArgumentNullException(nameof(cellPositions));
            var mesh = new Mesh();
            var vertices = new List<Vector3>();
            var indices = new List<int>();
            
            // Calculate bottom-left corner of first cell: 
            var minimum = new Vector2(spacing * gridSize.x / 2f, spacing * gridSize.y / 2f);
            for (var i = 0; i <= gridSize.x; i++)
            {
                var jPositions = new List<TabletopCell>();
                for (var j = 0; j <= gridSize.y; j++)
                {
                    // Only store cell position if all corners are drawn:
                    var capturePosition = true;
                    
                    // Find coordinates for bottom-left, bottom-right, and top-left vertices:
                    var X = new Vector2(i * spacing - minimum.x, (i + 1) * spacing - minimum.x);
                    var Z = new Vector2(j * spacing - minimum.y, (j + 1) * spacing - minimum.y);

                    // Generate line on x-axis:
                    if (i != gridSize.x)
                    {
                        vertices.Add(new Vector3(X.x, 0f, Z.x));    // Bottom-left 
                        vertices.Add(new Vector3(X.y, 0, Z.x));     // Bottom-right
                    }
                    else capturePosition = false;

                    // Generate line on z-axis:
                    if (j == gridSize.y) continue;
                    vertices.Add(new Vector3(X.x, 0, Z.x));         // Bottom-left
                    vertices.Add(new Vector3(X.x, 0, Z.y));         // Top-left

                    if (!capturePosition) continue;
                    
                    // Find centre position of cell:
                    var topLeft = new Vector2(X.x, Z.y);
                    var bottomRight = new Vector2(X.y, Z.x);
                    var cellPosition = topLeft + (bottomRight - topLeft) / 2f;
                    
                    // Spawn cell prefab:
                    var newCell = Instantiate(CellPrefab, transform).GetComponent<TabletopCell>();
                    newCell.Position = cellPosition;
                    newCell.transform.position = new Vector3(cellPosition.x, 0f, cellPosition.y);
                    newCell.name = "Cell_[" + i + "," + j + "]";
                    newCell.Coordinate = new Vector2Int(i, j);
                    newCell.SetCellState(newCell.DisabledState);
                    jPositions.Add(newCell);
                    
                }
                cellPositions.Add(jPositions);
            }

            var indicesCount = vertices.Count;
            for (var i = 0; i < indicesCount; i++) indices.Add(i);
            
            mesh.vertices = vertices.ToArray();
            mesh.SetIndices(indices.ToArray(), MeshTopology.Lines, 0);
            return mesh;
        }

        /// <summary>Get direction from one coordinate relative to another.</summary>
        /// <param name="start">Starting point coordinates.</param>
        /// <param name="end">End point coordinates.</param>
        /// <returns>Direction of the end coordinates relative to the start coordinates.</returns>
        private static Direction GetCellTraverseDirection(Vector2Int start, Vector2Int end)
        {
            var difference = new Vector2Int(end.x - start.x, end.y - start.y);
            
            // Non-diagonal movement:
            if (difference == Vector2Int.zero) return Direction.None;
            if (difference.x == 0) return difference.y < 0 ? Direction.Down : Direction.Up;
            if (difference.y == 0) return difference.x < 0 ? Direction.Left : Direction.Right;

            // Diagonal movement:
            return difference switch
            {
                { x: < 0, y: > 0 } => Direction.TopLeft,
                { x: < 0, y: < 0 } => Direction.BottomLeft,
                { x: > 0, y: > 0 } => Direction.TopRight,
                { x: > 0, y: < 0 } => Direction.BottomRight,
                _ => Direction.None
            };
        }
        
        /// <summary>Calculate distance between two cell coordinates. Horizontal and vertical movements are 10 per node, whereas diagonal
        /// movements are 10 * sqrt(2).</summary>
        /// <param name="node1">First node coordinates.</param>
        /// <param name="node2">Second node coordinates.</param>
        /// <returns>Distance between node1 and node2.</returns>
        public static int CalculateCellDistance(Vector2Int node1, Vector2Int node2)
        {
            var difference = new Vector2Int(Mathf.Abs(node1.x - node2.x), Mathf.Abs(node1.y - node2.y));
            if (difference.x > difference.y) return difference.y * _diagonalMoveValue + (difference.x - difference.y) * _nonDiagonalMoveValue;
            return difference.x * _diagonalMoveValue + (difference.y - difference.x) * _nonDiagonalMoveValue;
        }
        #endregion
    }
}
