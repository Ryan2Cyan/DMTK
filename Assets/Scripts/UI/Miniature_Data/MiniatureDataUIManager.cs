using Input;
using Tabletop.Miniatures;
using TMPro;
using UI.Miniature_Radial;
using UI.Utility;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI.Miniature_Data
{
    public class MiniatureDataUIManager : MonoBehaviour, IPooledObject
    {
        [Header("Components")]
        public MiniatureData MiniatureData;
        public TextMeshProUGUI StatusConditionsTMP;
        public Slider HitPointsSlider;

        private Animator _animator;
        private CullTMPComponents _cullImageComponentsScript;
        private DisplayUIInWorldSpace _displayUIInWorldSpace;

        private bool _exhaustionActive;
        
        private static readonly int ShowTabData = Animator.StringToHash("ShowTabData");

        #region UnityFunctions

        private void OnEnable()
        {
            InputManager.OnTabDown += OnTabDown;
            InputManager.OnTabUp += OnTabUp;
        }
        
        private void OnDisable()
        {
            InputManager.OnTabDown -= OnTabDown;
            InputManager.OnTabUp -= OnTabUp;
        }

        private void OnDestroy()
        {
            // Unsubscribe to events:
            RadialManager.OnStatusConditionChanged -= OnStatusConditionChanged;
            RadialManager.OnHitPointsChanged -= OnHitPointsChanged;
        }

        #endregion

        public void Instantiate(MiniatureData miniatureData)
        {
            MiniatureData = miniatureData;
            var mainCamera = Camera.main;
            _cullImageComponentsScript = GetComponent<CullTMPComponents>();
            _cullImageComponentsScript.Target = MiniatureData.transform;
            _cullImageComponentsScript.Camera = mainCamera;
            _displayUIInWorldSpace = GetComponent<DisplayUIInWorldSpace>();
            _displayUIInWorldSpace.WorldSpaceTarget = MiniatureData.transform;
            _displayUIInWorldSpace.Camera = mainCamera;
            _animator = GetComponent<Animator>();
            
            // Subscribe to events:
            RadialManager.OnStatusConditionChanged += OnStatusConditionChanged;
            RadialManager.OnHitPointsChanged += OnHitPointsChanged;
            
            // Set default values:
            OnHitPointsChanged(miniatureData);
        }
        
        private void OnStatusConditionChanged(MiniatureData miniatureData)
        {
            if (miniatureData != MiniatureData) return;
            
            // Add icon for each status condition:
            var message = "";
            foreach (var statusConditionPair in miniatureData.StatusConditions)
            {
                if (statusConditionPair.Value) message += "<sprite=" + (int)statusConditionPair.Key + ">";
            }
            
            // Add exhaustion level icon:
            if (miniatureData.ExhaustionLevel > 0)
            {
                switch (miniatureData.ExhaustionLevel)
                {
                    case 1: message += "<sprite=" + 13 + ">"; break;
                    case 2: message += "<sprite=" + 14 + ">"; break;
                    case 3: message += "<sprite=" + 15 + ">"; break;
                    case 4: message += "<sprite=" + 16 + ">"; break;
                    case 5: message += "<sprite=" + 17 + ">"; break;
                    case 6: message += "<sprite=" + 18 + ">"; break;
                }
            }
            StatusConditionsTMP.text = message;
        }

        private void OnHitPointsChanged(MiniatureData miniatureData)
        {
            if (miniatureData != MiniatureData) return;
            HitPointsSlider.maxValue = miniatureData.MaximumHitPoints;
            HitPointsSlider.value = miniatureData.CurrentHitPoints;
        }
        
        #region EventFunctions

        private void OnTabDown()
        {
            _animator.SetBool(ShowTabData, true);
        }
        
        private void OnTabUp()
        {
            _animator.SetBool(ShowTabData, false);
        }

        #endregion

        public void Instantiate() { }
        public void Release() { }
    }
}
