using System.Collections.Generic;
using UnityEngine;

namespace Tabletop
{
    public class Tabletop : MonoBehaviour
    {
        public Color TabletopColour;
        public Vector2Int TabletopSize;
        public float CellSpacing;
        
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;

        private void Awake()
        {
            _meshFilter = GetComponent<MeshFilter>();
            _meshRenderer = GetComponent<MeshRenderer>();
            // _meshFilter.mesh = GenerateAsymmetricalGridMesh(TabletopSize, CellSpacing);
            _meshFilter.mesh = GenerateSymmetricalGridMesh(10);
            _meshRenderer.material = new Material(Shader.Find("Universal Render Pipeline/2D/Sprite-Unlit-Default")) { color = TabletopColour };
        }

        /// <summary>
        /// Generates all points within the grid, with the origin being in the centre of the grid. Indices are
        /// incremented by one for each new vertex.
        /// </summary>
        /// <param name="gridSize">Dimensions of the asymmetrical grid, the size.x component corresponding to the
        /// width (x), and size.y corresponding to the depth (z).</param>
        /// <param name="spacing">The distance between each cell, determining the overall surface size of the grid.</param>
        /// <returns>Generated asymmetrical grid mesh. Can be assigned to the mesh of a MeshFilter component.</returns>
        private static Mesh GenerateAsymmetricalGridMesh(Vector2Int gridSize, float spacing)
        {
            // Source: https://gist.github.com/mdomrach/a66602ee85ce45f8860c36b2ad31ea14
            
            var mesh = new Mesh();
            var vertices = new List<Vector3>();
            var indices = new List<int>();

            var minimum = new Vector2(spacing * gridSize.x / 2f, spacing * gridSize.y / 2f);
            
            for (var i = 0; i <= gridSize.x; i++)
            {
                for (var j = 0; j <= gridSize.y; j++)
                {
                    var X = new Vector2(i * spacing - minimum.x, (i + 1) * spacing - minimum.x);
                    var Z = new Vector2(j * spacing - minimum.y, (j + 1) * spacing - minimum.y);

                    if (i != gridSize.x)
                    {
                        vertices.Add(new Vector3(X.x, 0f, Z.x));
                        vertices.Add(new Vector3(X.y, 0, Z.x));
                    }

                    if (j == gridSize.y) continue;
                    vertices.Add(new Vector3(X.x, 0, Z.x));
                    vertices.Add(new Vector3(X.x, 0, Z.y));
                }
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
    }
}
