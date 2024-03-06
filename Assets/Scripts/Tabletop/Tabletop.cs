using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tabletop
{
    public class Tabletop : MonoBehaviour
    {
        public Vector2 TabletopDimensions;
        public List<Vector3> NewVertices;
        public List<Vector2> NewUV;
        public List<int> NewTriangles;

        private void Awake()
        {
            var filter = GetComponent<MeshFilter>();
            var mesh = new Mesh();
            
            var vertices = new List<Vector3>();
            var indices = new List<int>();

            for (var i = 0; i <= TabletopDimensions.x; i++)
            {
                vertices.Add(new Vector3(i, 0f,0f));
                vertices.Add(new Vector3(i, 0, TabletopDimensions.x));
                
                indices.Add(4 * i);
                indices.Add(4 * i + 1);
                
                vertices.Add(new Vector3(0, 0, i));
                vertices.Add(new Vector3(TabletopDimensions.x, 0, i));
                
                indices.Add(4 * i + 2);
                indices.Add(4 * i + 3);
            }

            mesh.vertices = vertices.ToArray();
            mesh.SetIndices(indices.ToArray(), MeshTopology.Lines, 0);
            filter.mesh = mesh;

            var meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.material = new Material(Shader.Find("Universal Render Pipeline/2D/Sprite-Unlit-Default"))
            {
                color = Color.white
            };
        }

        private void FixedUpdate()
        {
            // for (var x = 0; x < TabletopDimensions.x; x++)
            // {
            //     for (var y = 0; y < TabletopDimensions.y; y++)
            //     {
            //         // Test if a line can be drawn to the next x point.
            //         var increment = x + 1;
            //         if(increment < TabletopDimensions.x) Debug.DrawLine(new Vector3(x, 0f, y), new Vector3(increment, 0f, y), Color.green);
            //         
            //         // Test if a line can be drawn to the next z point.
            //         increment = y + 1;
            //         if(increment < TabletopDimensions.y) Debug.DrawLine(new Vector3(x, 0f, y), new Vector3(x, 0f, increment), Color.green);
            //     }
            // }
        }
    }
}
