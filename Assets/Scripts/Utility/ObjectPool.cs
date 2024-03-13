using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utility
{
    public class ObjectPool : MonoBehaviour
    {
        public GameObject Prefab;
        private List<IPooledObject> _inUse = new List<IPooledObject>();
        private List<IPooledObject> _available = new List<IPooledObject>();
        
        public IPooledObject GetPooledObject()
        {
            // Check if there are any available objects, if yes return the first:
            if (_available.Count > 0)
            {
                var newObject0 = _available.First();
                newObject0.Instantiate();
                _available.Remove(newObject0);
                return newObject0;
            }
            
            // If no, instantiate a new object:
            var newObject = Instantiate(Prefab, transform);
            var poolComponent = newObject.GetComponent<IPooledObject>();
            poolComponent.Instantiate();
            _inUse.Add(poolComponent);
            return poolComponent;
        }

        public void ReleasePooledObject(IPooledObject pooledObject)
        {
            if (_available.Contains(pooledObject)) return;
            
            // Remove from in use list to the available list:
            _inUse.Remove(pooledObject);
            pooledObject.Release();
            _available.Add(pooledObject);
        }   
    }

    public interface IPooledObject
    {
        public void Instantiate();
        public void Release();
    }
}