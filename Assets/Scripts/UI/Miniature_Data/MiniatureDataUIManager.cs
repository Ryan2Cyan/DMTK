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
        public TextMeshProUGUI LabelTMP;
        public Slider HitPointsSlider;

        private Animator _animator;
        private CullTMPComponents _cullImageComponentsScript;
        private DisplayUIInWorldSpace _displayUIInWorldSpace;

        private bool _exhaustionActive;
        
        private static readonly int ShowTabData = Animator.StringToHash("ShowTabData");
        private static readonly int Enabled = Animator.StringToHash("Enabled");

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
            RadialManager.OnLabelChanged -= OnLabelChanged;
            RadialManager.OnTypeChanged -= OnTypeChanged;
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
            _animator.SetBool(Enabled, true);
            
            // Subscribe to events:
            RadialManager.OnStatusConditionChanged += OnStatusConditionChanged;
            RadialManager.OnHitPointsChanged += OnHitPointsChanged;
            RadialManager.OnLabelChanged += OnLabelChanged;
            RadialManager.OnTypeChanged += OnTypeChanged;
            
            // Set default values:
            OnStatusConditionChanged(miniatureData);
            OnHitPointsChanged(miniatureData);
            OnLabelChanged(miniatureData);
            OnTypeChanged(miniatureData);
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

        private void OnLabelChanged(MiniatureData miniatureData)
        {
            if (miniatureData != MiniatureData) return;
            LabelTMP.text = miniatureData.Label;
        }

        private void OnTypeChanged(MiniatureData miniatureData)
        {
            ToggleHideAll(miniatureData.Type == MiniatureType.Prop);
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

        private void ToggleHideAll(bool toggle)
        {
            StatusConditionsTMP.enabled = !toggle;
            LabelTMP.enabled = !toggle;
            HitPointsSlider.enabled = !toggle;
            _animator.SetBool(Enabled, !toggle);
        }
        #endregion

        public void Instantiate() { }
        public void Release() { }
    }
}
