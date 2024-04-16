using System.Collections.Generic;
using UI.UI_Interactables;
using UI.Utility;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Utility;

namespace UI.Miniature_Library
{
    public class MiniatureLibraryManager : MonoBehaviour
    {
        [Header("Components")]
        public RectOverflow RectOverflowScript;
        public ObjectPool DMTKLibraryButtonObjectPool; 
        public AssetLabelReference MiniatureIconLabel;
        public AssetLabelReference MiniaturePrefabLabel;

        private AsyncOperationHandle<IList<Sprite>> _miniImageAsyncHandle;  
        #region UnityFunctions

        private void OnEnable()
        {
            _miniImageAsyncHandle = Addressables.LoadAssetsAsync<Sprite>(MiniatureIconLabel, miniSprite =>
            {
                var newButton = (DMTKSimpleButton) DMTKLibraryButtonObjectPool.GetPooledObject();
                newButton.BaseImage.sprite = miniSprite;
                RectOverflowScript.RectElements.Add(newButton.RectTransform);
            });
        }

        private void OnDisable()
        {
            DMTKLibraryButtonObjectPool.ReleaseAll();
            RectOverflowScript.RectElements.Clear();
            Addressables.Release(_miniImageAsyncHandle);
        }

        #endregion
    }
}
