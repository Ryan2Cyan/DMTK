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
        
        private void Start()
        {
            _meshFilter = GetComponentInChildren<MeshFilter>();
        }

        private void FixedUpdate()
        {
            Debug.Log(new Vector3(
                _meshFilter.mesh.bounds.size.x * transform.localScale.x,
                _meshFilter.mesh.bounds.size.x * transform.localScale.y,
                _meshFilter.mesh.bounds.size.x * transform.localScale.z));
        }
    }
}