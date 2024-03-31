using Tabletop.Miniatures;
using UnityEngine;

namespace UI
{
    public class RadialUIManager : MonoBehaviour
    {
        public static RadialUIManager Instance;
        public GameObject MainRadial;
        public GameObject ConditionalsRadial;
        
        [HideInInspector] public Miniature SelectedMini;
        [HideInInspector] public float RadialScreenSpaceYOffset = 25f;

        [Header("States")]
        private IRadialUIManagerState _currentState;
        private readonly RadialUIManagerDisabled _disabledState = new();
        private readonly RadialUIManagerMain _mainState = new();
        private readonly RadialUIManagerStatusConditions _statusConditionsState = new();
        
        private void Awake()
        {
            Instance = this;
            _currentState = _disabledState;
            _disabledState.OnStart(this);
        }

        public void ChangeState_UnityEvent(int state)
        {
            switch (state)
            {
                case 0: ChangeState(_disabledState); break;
                case 1: ChangeState(_mainState); break;
                case 2: ChangeState(_statusConditionsState); break;
            }
        }

        private void ChangeState(IRadialUIManagerState state)
        {
            _currentState.OnExit(this);
            _currentState = state;
            _currentState.OnStart(this);
        }
        
        public void MiniatureClicked(Miniature miniature)
        {
            if (_currentState == _disabledState)
            {
                SelectedMini = miniature;
                ChangeState(_mainState);
            }
            else
            {
                if (SelectedMini != miniature) return;
                SelectedMini = null;
                ChangeState(_disabledState);
            }
        }

        public void MiniatureGrabbed()
        {
            SelectedMini = null;
            ChangeState(_disabledState);
        }
        
        public void HideAll()
        {
            MainRadial.SetActive(false);
            ConditionalsRadial.SetActive(false);
        }
    }


    public interface IRadialUIManagerState
    {
        public void OnStart(RadialUIManager radialUIManager) {}
        public void OnExit(RadialUIManager radialUIManager) {}
    }

    public class RadialUIManagerDisabled : IRadialUIManagerState
    {
        public void OnStart(RadialUIManager radialUIManager)
        {
            radialUIManager.HideAll();
        }
    }
    
    public class RadialUIManagerMain : IRadialUIManagerState
    {
        public void OnStart(RadialUIManager radialUIManager)
        {
            radialUIManager.MainRadial.SetActive(true);
            Vector3 screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, radialUIManager.SelectedMini.transform.position);
            radialUIManager.transform.position = new Vector3(screenPosition.x, screenPosition.y + radialUIManager.RadialScreenSpaceYOffset + screenPosition.z);
        }

        public void OnExit(RadialUIManager radialUIManager)
        {
            radialUIManager.MainRadial.SetActive(false);
        }
    }
    
    public class RadialUIManagerStatusConditions : IRadialUIManagerState
    {
        public void OnStart(RadialUIManager radialUIManager)
        {
            radialUIManager.ConditionalsRadial.SetActive(true);
        }

        public void OnExit(RadialUIManager radialUIManager)
        {
            radialUIManager.ConditionalsRadial.SetActive(false);
        }
    }
}
