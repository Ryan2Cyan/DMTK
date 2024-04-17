using System.Collections.Generic;
using Tabletop.Miniatures;
using UI.Utility;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Utility;

namespace UI.Miniature_Library
{
    public class MiniatureLibraryManager : MonoBehaviour
    {
        public static MiniatureLibraryManager Instance;
        
        [Header("Components")]
        public RectOverflow RectOverflowScript;
        public ObjectPool DMTKLibraryButtonObjectPool; 
        public AssetLabelReference MiniatureSpawnDataLabel;
        public Transform MiniatureParent;

        
        private AsyncOperationHandle<IList<MiniatureSpawnDataSO>> _miniDataAsyncHandle;  
        
        #region UnityFunctions

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            _miniDataAsyncHandle = Addressables.LoadAssetsAsync<MiniatureSpawnDataSO>(MiniatureSpawnDataLabel, spawnData =>
            {
                var newButton = (MiniatureLibraryButton) DMTKLibraryButtonObjectPool.GetPooledObject();
                newButton.BaseImage.sprite = spawnData.ButtonImage;
                newButton.MiniatureID = spawnData.Id;
                RectOverflowScript.RectElements.Add(newButton.RectTransform);
            });
        }

        private void OnDisable()
        {
            DMTKLibraryButtonObjectPool.ReleaseAll();
            RectOverflowScript.RectElements.Clear();
            Addressables.Release(_miniDataAsyncHandle);
        }

        #endregion

        #region PublicFunctions

        public void SpawnMiniature(string id)
        {
            foreach (var miniData in _miniDataAsyncHandle.Result)
            {
                if (id != miniData.Id) continue;

                Tabletop.Tabletop.Tabletop.Instance.AssignClosestToGridCentre(out var newCell);
                if (newCell == null) continue;
                
                var spawnedMini = Instantiate(miniData.Prefab, MiniatureParent).GetComponent<Miniature>();
                spawnedMini.Spawn(miniData, newCell);
            }
        }

        #endregion
    }
}
