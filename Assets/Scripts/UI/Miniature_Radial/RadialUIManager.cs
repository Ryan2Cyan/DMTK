using Tabletop.Miniatures;
using UnityEngine;

namespace UI.Miniature_Radial
{
    public class RadialUIManager : MonoBehaviour
    {
        public static RadialUIManager Instance;
        
        [Header("Radial Menus")]
        public Animator MainRadial;
        public Animator ConditionalsRadial;

        [Header("Radial Icons")] 
        public MiniatureRadialInteger ExhaustionRadialIcon;
        
        [HideInInspector] public MiniatureData SelectedMiniData;
        
        [HideInInspector] public float RadialScreenSpaceYOffset = 40f;

        [Header("States")]
        private IRadialUIManagerState _currentState;
        private readonly RadialUIManagerDisabled _disabledState = new();
        private readonly RadialUIManagerMain _mainState = new();
        private readonly RadialUIManagerStatusConditions _statusConditionsState = new();
        
        private static readonly int Enabled = Animator.StringToHash("Enabled");

        #region UnityFunctions

        private void Awake()
        {
            Instance = this;
            _currentState = _disabledState;
            _disabledState.OnStart(this);
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
            MainRadial.SetBool(Enabled,false);
            ConditionalsRadial.SetBool(Enabled, false);
        }

        #endregion

        #region PrivateFunctions

        private void ChangeState(IRadialUIManagerState state)
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
