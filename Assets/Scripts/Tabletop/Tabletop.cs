
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility;

namespace Tabletop
{
    public class Tabletop : MonoBehaviour
    {
        public static Tabletop Instance;
        
        [Header("Settings")]
        public Color TabletopColour;
        public Vector2Int TabletopSize;
        public float CellSpacing;
        public float MiniatureScale;
        public float DistancePerCell;
        
        [Header("Required")]
        public ObjectPool DistanceIndicatorsPool;
        public GameObject CellPrefab;
        public LayerMask TabletopLayerMask;
        
        private List<List<TabletopCell>> _gridCells;
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;

        [ContextMenu("Generate Grid")]
        private void GenerateGrid()
        {
            if(_meshFilter == null) _meshFilter = GetComponent<MeshFilter>();
            if(_meshRenderer == null) _meshRenderer = GetComponent<MeshRenderer>();
            _meshFilter.mesh = GenerateAsymmetricalGridMesh(TabletopSize, CellSpacing, ref _gridCells);
            _meshRenderer.material = new Material(Shader.Find("Universal Render Pipeline/2D/Sprite-Unlit-Default")) { color = TabletopColour };
        }

        #region UnityFunctions

        private void Awake()
        {
            Instance = this;
            GenerateGrid();
        }

        // private void FixedUpdate()
        // {
        //     foreach (var gridPositionList in _gridCells)
        //     {
        //         foreach (var cell in gridPositionList)
        //         {
        //             Debug.DrawLine(new Vector3(cell.Position.x, 0f, cell.Position.y), new Vector3(cell.Position.x, 1f, cell.Position.y), Color.yellow);                    
        //         }
        //     }
        // }

        #endregion

        #region TabletopFunctions

        /// <summary>
        /// Calculates the local scale modification value required to scale a miniature's mesh down to grid cell
        /// size.
        /// </summary>
        /// <param name="currentSize">The current local size of the miniature's mesh [see Mesh.bounds.size]. </param>
        /// <returns>Current scale to grid cell size local scale modifier.</returns>
        public Vector3 GenerateMeshToGridScale(Vector3 currentSize)
        {
            return new Vector3(
                1f / currentSize.x * MiniatureScale,
                1f / currentSize.y * MiniatureScale,
                1f / currentSize.z * MiniatureScale
                );
        }
        
        /// <summary>
        /// Generates all points within the grid, with the origin being in the centre of the grid. Indices are
        /// incremented by one for each new vertex.
        /// </summary>
        /// <param name="gridSize">Dimensions of the asymmetrical grid, the size.x component corresponding to the
        /// width (x), and size.y corresponding to the depth (z).</param>
        /// <param name="spacing">The distance between each cell, determining the overall surface size of the grid.</param>
        /// <param name="positions">Stores the central grid position corresponding to each grid cell. </param>
        /// <returns>Generated asymmetrical grid mesh. Can be assigned to the mesh of a MeshFilter component.</returns>
        private Mesh GenerateAsymmetricalGridMesh(Vector2Int gridSize, float spacing, ref List<List<TabletopCell>> positions)
        {
            // Source: https://gist.github.com/mdomrach/a66602ee85ce45f8860c36b2ad31ea14
            
            var mesh = new Mesh();
            var vertices = new List<Vector3>();
            var indices = new List<int>();
            positions = new List<List<TabletopCell>>();
            
            var minimum = new Vector2(spacing * gridSize.x / 2f, spacing * gridSize.y / 2f);

            for (var i = 0; i <= gridSize.x; i++)
            {
                var jPositions = new List<TabletopCell>();
                for (var j = 0; j <= gridSize.y; j++)
                {
                    var capturePosition = true;
                    var X = new Vector2(i * spacing - minimum.x, (i + 1) * spacing - minimum.x);
                    var Z = new Vector2(j * spacing - minimum.y, (j + 1) * spacing - minimum.y);

                    if (i != gridSize.x)
                    {
                        vertices.Add(new Vector3(X.x, 0f, Z.x));
                        vertices.Add(new Vector3(X.y, 0, Z.x));
                    }
                    else capturePosition = false;

                    if (j == gridSize.y) continue;
                    vertices.Add(new Vector3(X.x, 0, Z.x));
                    vertices.Add(new Vector3(X.x, 0, Z.y));

                    if (!capturePosition) continue;
                    var topLeft = new Vector2(X.x, Z.y);
                    var bottomRight = new Vector2(X.y, Z.x);
                    var cellPosition = topLeft + (bottomRight - topLeft) / 2f;
                    
                    // Spawn in prefab to have visual representation of the cell in-game.
                    var newCell = Instantiate(CellPrefab, transform).GetComponent<TabletopCell>();
                    newCell.Position = cellPosition;
                    newCell.transform.position = new Vector3(cellPosition.x, 0f, cellPosition.y);
                    newCell.name = "Cell_[" + i + "," + j + "]";
                    newCell.Coordinate = new Vector2Int(i, j);
                    newCell.SetCellState(newCell.DisabledState);
                    jPositions.Add(newCell);
                    
                }
                positions.Add(jPositions);
            }

            var indicesCount = vertices.Count;
            for (var i = 0; i < indicesCount; i++) indices.Add(i);
            
            mesh.vertices = vertices.ToArray();
            mesh.SetIndices(indices.ToArray(), MeshTopology.Lines, 0);
            return mesh;
        }
        

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

        /// <summary>
        /// Finds the closest neighboring cell (including diagonals) to the 'currentPosition'. Neighboring cells
        /// are derived from the 'currentCell' parameter. Used to reduce the amount of cells searched when
        /// moving miniatures.
        /// </summary>
        /// <param name="currentCell">Central cell of which neighboring cells will derive from.</param>
        /// <param name="currentPosition">Position to conduct distance checks.</param>
        /// <returns>The closest cell to the inputted 'currentPosition'.</returns>
        public TabletopCell GetClosestNeighboringCell(TabletopCell currentCell, Vector2 currentPosition)
        {
            var cellModifiers = new List<Vector2Int>
            {
                new(-1, 1),
                new(0, 1),
                new(1, 1),

                new(-1, 0),
                new(0, 0),
                new(1, 0),

                new(-1, -1),
                new(0, -1),
                new(1, -1),
                
                new(-2, 2),
                new(-1, 2),
                new(0, 2),
                new(1, 2),
                new(2, 2),
                
                new(-2, 1),
                new(2, 1),
                
                new(-2, 0),
                new(2, 0),
                
                new(-2, -1),
                new(2, -1),
                
                new(-2, -2),
                new(-1, -2),
                new(0, -2),
                new(1, -2),
                new(2, -2),
            };
            
            var closestDistance = float.PositiveInfinity;
            TabletopCell closestCell = null;
            var foundCell = false;
            foreach (var modifier in cellModifiers)
            {
                // Apply coordinate modifier and check the coordinate is still within the grid:
                var newCoordinate = new Vector2Int(currentCell.Coordinate.x + modifier.x, currentCell.Coordinate.y + modifier.y);
                if (newCoordinate.x < 0) continue;
                if(newCoordinate.x >= TabletopSize.x) continue;
                if (newCoordinate.y < 0) continue;
                if(newCoordinate.y >= TabletopSize.x) continue;
                
                // Check if the cell is closer that the current closest:
                var newCell = _gridCells[newCoordinate.x][newCoordinate.y];
                
                var distance = Vector2.Distance(new Vector2(currentPosition.x, currentPosition.y), newCell.Position);
                if (distance >= closestDistance) continue;
                closestDistance = distance;
                closestCell = newCell;
                foundCell = true;
            }

            return !foundCell ? currentCell : closestCell;
        }

        /// <summary>
        /// Returns the mouse's current position in relation the the tabletop grid/collider.
        /// </summary>
        /// <returns>If the cursor intersects the tabletop then the intersection point is returned, otherwise
        /// returns Vector3.negativeInfinity.</returns>
        public bool GetTabletopMousePosition(ref Vector3 position)
        {
            var hit = DMTKPhysicsUtility.PhysicsMouseRayCast(TabletopLayerMask);
            if (!hit.collider) return false;
            position = hit.point;
            return true;
        }

        public List<TabletopCell> GetShortestPath(TabletopCell start, TabletopCell end)
        {
            return DistanceArrowPathfinder.AStarPathfinder(start, end, TabletopSize, _gridCells);
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
            
            // Set distance indicator:
            
        }

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
        #endregion
    }
}
