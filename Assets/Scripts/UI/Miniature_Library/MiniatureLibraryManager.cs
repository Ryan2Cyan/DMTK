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
        public AssetLabelReference MiniaturePrefabLabel;
        
        private AsyncOperationHandle<IList<MiniatureSpawnDataSO>> _miniDataAsyncHandle;  
        private AsyncOperationHandle<IList<GameObject>> _miniPrefabAsyncHandle;  
        
        #region UnityFunctions

        private void Awake()
        {
            Instance = this;
            _miniPrefabAsyncHandle = Addressables.LoadAssetsAsync<GameObject>(MiniaturePrefabLabel,
                spawnData => { });
        }

        private void OnDestroy()
        {
            Addressables.Release(_miniPrefabAsyncHandle);
        }

        private void OnEnable()
        {
            _miniDataAsyncHandle = Addressables.LoadAssetsAsync<MiniatureSpawnDataSO>(MiniatureSpawnDataLabel,
                spawnData =>
                {
                    var newButton = (MiniatureLibraryButton)DMTKLibraryButtonObjectPool.GetPooledObject();
                    newButton.BaseImage.sprite = spawnData.ButtonImage;
                    newButton.MiniatureID = spawnData.Id;
                    RectOverflowScript.Elements.Add(newButton.RectTransform);
                    RectOverflowScript.CalculateOverflow();
                });
        }

        private void OnDisable()
        {
            DMTKLibraryButtonObjectPool.ReleaseAll();
            RectOverflowScript.Elements.Clear();
            Addressables.Release(_miniDataAsyncHandle);
        }

        #endregion

        #region PublicFunctions

        public void LocateMiniatureId(string id)
        {
            foreach (var miniData in _miniDataAsyncHandle.Result)
            {
                if (id != miniData.Id) continue;
                MiniatureManager.Instance.SpawnMiniature(miniData);
            }
        }

        #endregion
    }
}
