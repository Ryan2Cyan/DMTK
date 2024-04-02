using Tabletop.Miniatures;
using TMPro;
using UI.Miniature_Radial;
using UI.Utility;
using UnityEngine;

namespace UI.Miniature_Data
{
    public class MiniatureDataUIManager : MonoBehaviour
    {
        [Header("Components")]
        public MiniatureData MiniatureData;
        public TextMeshProUGUI StatusConditionsTMP;
        
        private CullTMPComponents _cullImageComponentsScript;
        private DisplayUIInWorldSpace _displayUIInWorldSpace;
  
        #region UnityFunctions
        
        private void Awake()
        {
            _cullImageComponentsScript = GetComponent<CullTMPComponents>();
            _cullImageComponentsScript.Target = MiniatureData.transform;
            _displayUIInWorldSpace = GetComponent<DisplayUIInWorldSpace>();
            _displayUIInWorldSpace.WorldSpaceTarget = MiniatureData.transform;
            
            // Subscribe to events:
            RadialManager.OnStatusConditionChanged += StatusConditionChanged;
        }

        private void OnDestroy()
        {
            // Unsubscribe to events:
            RadialManager.OnStatusConditionChanged -= StatusConditionChanged;
        }

        #endregion

        private void StatusConditionChanged(MiniatureData miniatureData)
        {
            if (miniatureData != MiniatureData) return;
            
            var message = "";
            foreach (var statusConditionPair in miniatureData.StatusConditions)
            {
                if (statusConditionPair.Value) message += "<sprite=" + (int)statusConditionPair.Key + ">";
            }
            StatusConditionsTMP.text = message;
        }
    }
}
