using System.Collections.Generic;
using Tabletop.Miniatures;
using TMPro;
using UI.UI_Interactables;
using UI.Utility;
using UI.Window;
using UnityEngine;

namespace UI.Miniature_Radial
{
    public class RadialManager : MonoBehaviour
    {
        public static RadialManager Instance;
        
        [Header("Radial Menus")]
        public RadialMenu MainRadial;
        public RadialMenu ConditionalsRadial;
        
        [Header("Window Menus")]
        public ReadKeyboardInput MaximumHealthKeyboardInput;
        public ReadKeyboardInput LabelKeyboardInput;
        public DMTKSlider CurrentHealthSlider;
        public TextMeshProUGUI CurrentHealthTMP;
        public DMTKToggleOptions TypeToggleOptions;

        [Header("Main Menu Radial Icons")] 
        public RadialBasic HitPointSettingsIcon;
        public RadialBasic CharacterSheetIcon;
        public RadialBasic StatusConditionsIcon;
        public RadialBasic LabelIcon;
        
        [Header("Status Condition Radial Icons")] 
        public RadialInteger ExhaustionRadialIcon;
        
        [HideInInspector] public MiniatureData SelectedMiniData;

        [Header("States")]
        private IRadialManagerState _currentState;
        private readonly RadialManagerDisabled _disabledState = new();
        private readonly RadialManagerMain _mainState = new();
        private readonly RadialManagerStatusConditions _statusConditionsState = new();

        private DisplayUIInWorldSpace _uiInWorldSpaceScript;
        private List<CullUnityEvent> _cullScripts;
        
        // MiniatureData setting events:
        public delegate void DMTKMiniatureDataAction(MiniatureData miniatureData);
        public static event DMTKMiniatureDataAction OnStatusConditionChanged;
        public static event DMTKMiniatureDataAction OnHitPointsChanged;
        public static event DMTKMiniatureDataAction OnLabelChanged;
        public static event DMTKMiniatureDataAction OnTypeChanged;
        
        private static readonly int Enabled = Animator.StringToHash("Enabled");

        #region UnityFunctions

        private void Awake()
        {
            Instance = this;
            _currentState = _disabledState;
            _disabledState.OnStart(this);
            _uiInWorldSpaceScript = GetComponent<DisplayUIInWorldSpace>();
            _cullScripts = new List<CullUnityEvent>(GetComponentsInChildren<CullUnityEvent>());

            ToggleTargetScripts(false);
        }

        #endregion

        #region PublicFunctions
        
        public void ChangeState_UnityEvent(int state)
        {
            switch (state)
            {
                case 0: ChangeState(_disabledState); break;
                case 1: ChangeState(_mainState); break;
                case 2: ChangeState(_statusConditionsState); break;
            }
        }
        
        public void MiniatureClicked(MiniatureData miniature)
        {
            SelectedMiniData = miniature;
            SetTargetScriptTargets(SelectedMiniData.transform);
            ChangeState(_mainState);
        }

        public void Disable()
        {
            ChangeState(_disabledState);
        }

        public void HideAll()
        {
            MainRadial.MenuAnimator.SetBool(Enabled,false);
            ConditionalsRadial.MenuAnimator.SetBool(Enabled, false);
        }

        public void EnableWorldSpaceDisplay()
        {
            ToggleTargetScripts(true);
        }

        public void DisableWorldSpaceDisplay()
        {
            ToggleTargetScripts(false);
        }

        #endregion

        #region PrivateFunctions

        private void ChangeState(IRadialManagerState state)
        {
            _currentState.OnExit(this);
            _currentState = state;
            _currentState.OnStart(this);
        }

        #endregion

        #region MiniatureDataFunctions

        public void LoadMiniatureData()
        {
            // Set hit point UI:
            MaximumHealthKeyboardInput.SetStringValue(SelectedMiniData.MaximumHitPoints.ToString());
            CurrentHealthTMP.text = SelectedMiniData.CurrentHitPoints.ToString();
            CurrentHealthSlider.maxValue = SelectedMiniData.MaximumHitPoints;
            CurrentHealthSlider.value = SelectedMiniData.CurrentHitPoints;
            
            // Set label UI:
            LabelKeyboardInput.SetStringValue(SelectedMiniData.Label);
            TypeToggleOptions.SelectOption((int)SelectedMiniData.Type);
        }
        
        public void ToggleStatusCondition(int statusCondition)
        {
            var statusConditionEnum = (StatusCondition)statusCondition;
            SelectedMiniData.StatusConditions[statusConditionEnum] = !SelectedMiniData.StatusConditions[statusConditionEnum];
            OnStatusConditionChanged?.Invoke(SelectedMiniData);
        }

        public void SetExhaustionLevel()
        {
            SelectedMiniData.ExhaustionLevel = ExhaustionRadialIcon.Value;
            OnStatusConditionChanged?.Invoke(SelectedMiniData);
        }

        public void SetMaxHitPoints()
        {
            var newMaxHitPoints = MaximumHealthKeyboardInput.GetDigitValue();
            SelectedMiniData.MaximumHitPoints = newMaxHitPoints;
            CurrentHealthSlider.maxValue = newMaxHitPoints;

            // Clamp current hit points if above the new maximum: 
            if (newMaxHitPoints >= SelectedMiniData.CurrentHitPoints)
            {
                OnHitPointsChanged?.Invoke(SelectedMiniData);
                return;
            }
            
            SelectedMiniData.CurrentHitPoints = newMaxHitPoints; 
            CurrentHealthTMP.text = MaximumHealthKeyboardInput.GetStringValue();
            CurrentHealthSlider.value = (float)SelectedMiniData.CurrentHitPoints / newMaxHitPoints;
            OnHitPointsChanged?.Invoke(SelectedMiniData);
        }

        public void SetCurrentHitPoints()
        {
            var newCurrentHitPoints = (int)CurrentHealthSlider.value;
            SelectedMiniData.CurrentHitPoints = newCurrentHitPoints;
            CurrentHealthTMP.text = newCurrentHitPoints.ToString();
            OnHitPointsChanged?.Invoke(SelectedMiniData);
        }

        public void SetLabel()
        {
            SelectedMiniData.Label = LabelKeyboardInput.GetStringValue(); 
            OnLabelChanged?.Invoke(SelectedMiniData);
        }

        public void SetType()
        {
            SelectedMiniData.Type = (MiniatureType)TypeToggleOptions.SelectedIndex;

            if (SelectedMiniData.Type == MiniatureType.Prop)
            {
                StatusConditionsIcon.OnToggleDisable(true);
                HitPointSettingsIcon.OnToggleDisable(true);
                // CharacterSheetIcon.OnToggleDisable(true);   
            }
            else
            {
                StatusConditionsIcon.OnToggleDisable(false);
                HitPointSettingsIcon.OnToggleDisable(false);
                // CharacterSheetIcon.OnToggleDisable(false);
            }
            
            OnTypeChanged?.Invoke(SelectedMiniData);
        }
        
        #endregion

        #region PrivateFunctions

        private void ToggleTargetScripts(bool toggle)
        {
            _uiInWorldSpaceScript.enabled = toggle;
            foreach (var cullScript in _cullScripts) cullScript.enabled = toggle;
        }

        private void SetTargetScriptTargets(Transform target)
        {
            _uiInWorldSpaceScript.WorldSpaceTarget = target;
            foreach (var cullScript in _cullScripts) cullScript.Target = target;
        }

        #endregion
    }
}
