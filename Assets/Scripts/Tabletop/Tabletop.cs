
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Tabletop
{
    public class Tabletop : MonoBehaviour
    {
        [Header("Settings")]
        public Color TabletopColour;
        public Vector2Int TabletopSize;
        public float CellSpacing;
        public float MiniatureScale;
        public GameObject CellPrefab;
        public LayerMask TabletopLayerMask;
        
        [Header("Cells")] 
        private List<List<TabletopCell>> _gridCells;
        
        public static Tabletop Instance;
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

        private void FixedUpdate()
        {
            foreach (var gridPositionList in _gridCells)
            {
                foreach (var cell in gridPositionList)
                {
                    Debug.DrawLine(new Vector3(cell.Position.x, 0f, cell.Position.y), new Vector3(cell.Position.x, 1f, cell.Position.y), Color.yellow);                    
                }
            }
        }

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
                    newCell.SetState(CellState.Unoccupied);
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
        /// Generates vertices around the edge of the grid. Indices are incremented by one for each new vertex.
        /// Additionally the origin for the grid generated will be at the grid’s centre. Can only create
        /// symmetrical grids [see "GenerateAsymmetricalGridMesh()"].
        /// </summary>
        /// <param name="size">The width (x) and depth (z) of the symmetrical grid.</param>
        /// <returns>Generated symmetrical grid mesh. Can be assigned to the mesh of a MeshFilter component.</returns>
        private static Mesh GenerateSymmetricalGridMesh(int size)
        {
            // Source: https://gist.github.com/mdomrach/a66602ee85ce45f8860c36b2ad31ea14
            
            var mesh = new Mesh();
            var vertices = new List<Vector3>();
            var indices = new List<int>();

            for (var i = 0; i <= size; i++)
            {
                vertices.Add(new Vector3(i, 0f, 0f));
                vertices.Add(new Vector3(i, 0f, size));
                indices.Add(4 * i);
                indices.Add(4 * i + 1);
                vertices.Add(new Vector3(0f, 0f, i));
                vertices.Add(new Vector3(size, 0f, i));
                indices.Add(4 * i + 2);
                indices.Add(4 * i + 3);
            }
            
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
                new(1, -1)
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

                // Check if the cell is occupied:
                var newCell = _gridCells[newCoordinate.x][newCoordinate.y];
                if(newCell.IsOccupied) continue;
                
                var distance = Vector2.Distance(new Vector2(currentPosition.x, currentPosition.y), newCell.Position);
                if (distance >= closestDistance) continue;
                closestDistance = distance;
                closestCell = newCell;
                foundCell = true;
            }

            if (foundCell)
            {
                Debug.Log(closestCell.Coordinate + " " + closestDistance);
                return closestCell;
            }
            return currentCell;
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
        
        #endregion
    }
}
