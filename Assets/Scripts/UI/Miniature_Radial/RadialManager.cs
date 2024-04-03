using Input;
using Tabletop.Miniatures;
using UI.Utility;
using UnityEngine;

namespace UI.Miniature_Radial
{
    public class RadialManager : MonoBehaviour
    {
        public static RadialManager Instance;
        
        [Header("Radial Menus")]
        public RadialMenu MainRadial;
        public RadialMenu ConditionalsRadial;

        [Header("Radial Icons")] 
        public RadialInteger ExhaustionRadialIcon;
        
        [HideInInspector] public MiniatureData SelectedMiniData;

        [Header("States")]
        private IRadialManagerState _currentState;
        private readonly RadialManagerDisabled _disabledState = new();
        private readonly RadialManagerMain _mainState = new();
        private readonly RadialManagerStatusConditions _statusConditionsState = new();

        private DisplayUIInWorldSpace _uiInWorldSpaceScript;
        private bool _iconPressed;
        private bool _miniPressed;
        
        public delegate void DMTKMiniatureDataAction(MiniatureData miniatureData);
        public static event DMTKMiniatureDataAction OnStatusConditionChanged;
        
        private static readonly int Enabled = Animator.StringToHash("Enabled");

        #region UnityFunctions

        private void Awake()
        {
            Instance = this;
            _currentState = _disabledState;
            _disabledState.OnStart(this);
            _uiInWorldSpaceScript = GetComponent<DisplayUIInWorldSpace>();
            _uiInWorldSpaceScript.enabled = false;
        }

        private void OnEnable()
        {
            InputManager.OnMouseUp += OnMouseUp;
        }

        private void OnDisable()
        {
            InputManager.OnMouseUp -= OnMouseUp;
        }

        #endregion

        #region PublicFunctions

        public void IconPressed()
        {
            _iconPressed = true;
        }
        
        public void ChangeState_UnityEvent(int state)
        {
            switch (state)
            {
                case 0: ChangeState(_disabledState); break;
                case 1: ChangeState(_mainState); break;
                case 2: ChangeState(_statusConditionsState); break;
            }
            IconPressed();
        }
        
        public void MiniatureClicked(MiniatureData miniature)
        {
            _miniPressed = true;
            if (_iconPressed)
            {
                _iconPressed = false;
                return;
            }
            
            if (_currentState != _disabledState) return;
            SelectedMiniData = miniature;
            _uiInWorldSpaceScript.WorldSpaceTarget = SelectedMiniData.transform;
            ChangeState(_mainState);
        }
        
        public void OnMouseUp()
        {
            if (_iconPressed)
            {
                _iconPressed = false;
                return;
            }

            if (_miniPressed)
            {
                _miniPressed = false;
                return;
            }
            SelectedMiniData = null;
            ChangeState(_disabledState);
        }

        public void MiniatureGrabbed()
        {
            SelectedMiniData = null;
            ChangeState(_disabledState);
        }

        public void HideAll()
        {
            MainRadial.MenuAnimator.SetBool(Enabled,false);
            ConditionalsRadial.MenuAnimator.SetBool(Enabled, false);
        }

        public void EnableWorldSpaceDisplay()
        {
            _uiInWorldSpaceScript.enabled = true;
        }

        public void DisableWorldSpaceDisplay()
        {
            _uiInWorldSpaceScript.enabled = false;
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
        
        #endregion
    }
}
