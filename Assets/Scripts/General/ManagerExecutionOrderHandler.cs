using Input;
using Tabletop.Miniatures;
using UI;
using UnityEngine;

namespace General
{
    public class ManagerExecutionOrderHandler : MonoBehaviour
    {
        private void Update()
        {
            UIManager.Instance.OnUpdate();
            MiniatureManager.Instance.OnUpdate();
            InputManager.Instance.OnUpdate();
            
            UIManager.Instance.OnLateUpdate();
            MiniatureManager.Instance.OnLateUpdate();
        }
    }
}
