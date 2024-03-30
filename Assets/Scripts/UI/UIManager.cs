using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        public GameObject RadialUI;
        public Canvas MainCanvas;

        private Animator RadialAnimator;

        private const float radialScreenSpaceYOffset = 25f;
        
        private static readonly int Reset = Animator.StringToHash("Reset");
        
        private void Awake()
        {
            Instance = this;
            RadialAnimator = RadialUI.GetComponent<Animator>();
        }

        public void ShowMainRadialUI(Vector3 position)
        {
            RadialUI.SetActive(true);
            RadialAnimator.SetTrigger(Reset);
            Vector3 screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, position);
            RadialUI.transform.position = new Vector3(screenPosition.x, screenPosition.y + radialScreenSpaceYOffset + screenPosition.z);
        }
        
        public void HideMainRadialUI()
        {
            RadialUI.SetActive(false);
        }
    }
}
