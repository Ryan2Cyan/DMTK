using System;
using System.Collections.Generic;
using Tabletop.Miniatures;
using UI.Utility;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
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
        public AssetReferenceGameObject AssetReferenceGameObject;
        public Transform MiniatureParent;

        private List<Transform> _childTransforms;
        private bool _initialised;
        
        private AsyncOperationHandle<IList<Sprite>> _miniDataAsyncHandle;  
        
        #region UnityFunctions

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            if (_initialised) return;
            _miniDataAsyncHandle = Addressables.LoadAssetsAsync<Sprite>(MiniatureSpawnDataLabel,
                spawnData =>
                {
                    var newButton = (MiniatureLibraryButton)DMTKLibraryButtonObjectPool.GetPooledObject();
                    newButton.BaseImage.sprite = spawnData;
                    // newButton.MiniatureID = spawnData.Id;
                    RectOverflowScript.RectElements.Add(newButton.RectTransform);
                });
            _initialised = true;

            _childTransforms = new List<Transform>(GetComponentsInChildren<Transform>());
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.I))
            {
                AssetReferenceGameObject.InstantiateAsync();
            }

            foreach (var childTransform in _childTransforms)
            {
                Debug.Log("Child: " + childTransform.gameObject.name + " Active: " + childTransform.gameObject.activeInHierarchy);
            }
        }

        private void OnDisable()
        {
            // DMTKLibraryButtonObjectPool.ReleaseAll();
            // RectOverflowScript.RectElements.Clear();
            // Addressables.Release(_miniDataAsyncHandle);
        }

        #endregion

        #region PublicFunctions

        public void SpawnMiniature(string id)
        {
            // foreach (var miniData in _miniDataAsyncHandle.Result)
            // {
            //     if (id != miniData.Id) continue;
            //
            //     Tabletop.Tabletop.Tabletop.Instance.AssignClosestToGridCentre(out var newCell);
            //     if (newCell == null) continue;
            //     
            //     var spawnedMini = Instantiate(miniData.Prefab, MiniatureParent).GetComponent<Miniature>();
            //     spawnedMini.Spawn(miniData, newCell);
            // }
        }

        #endregion
    }
}
