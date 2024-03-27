using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        public GameObject RadialUI;

        public void ShowMainRadialUI(Vector3 position)
        {
            RadialUI.SetActive(true);
            RadialUI.transform.position = position;
        }
    }
}
