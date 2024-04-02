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
        [HideInInspector] public float RadialScreenSpaceYOffset = 40f;

        [Header("States")]
        private IRadialManagerState _currentState;
        private readonly RadialManagerDisabled _disabledState = new();
        private readonly RadialManagerMain _mainState = new();
        private readonly RadialManagerStatusConditions _statusConditionsState = new();

        private DisplayUIInWorldSpace _uiInWorldSpaceScript;
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
            if (_currentState == _disabledState)
            {
                SelectedMiniData = miniature;
                _uiInWorldSpaceScript.WorldSpaceTarget = SelectedMiniData.transform.position;
                ChangeState(_mainState);
                return;
            }

            if (_currentState != _mainState) return;
            if (SelectedMiniData != miniature) return;
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

        public void EnableWorldSpaceDisplay(Transform menuTransform)
        {
            _uiInWorldSpaceScript.UIElementTransform = transform;
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
        }

        public void SetExhaustionLevel()
        {
            SelectedMiniData.ExhaustionLevel = ExhaustionRadialIcon.Value;
        }
        
        #endregion
        
    }
}
