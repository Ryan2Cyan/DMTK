using UnityEngine;

namespace Tabletop.Miniatures
{
    public class ScriptableObjectIdAttribute : PropertyAttribute { }
    
    public class BaseScriptableObject : ScriptableObject
    {
        [ScriptableObjectId] public string Id;
    }
    
    [CreateAssetMenu(fileName = "MiniatureSpawnDataSO", menuName = "ScriptableObjects/MiniatureSpawnData")]
    public class MiniatureSpawnDataSO : BaseScriptableObject
    {
        public GameObject Prefab;
        public Sprite ButtonImage;
        public MiniatureType DefaultType;
    }
}
