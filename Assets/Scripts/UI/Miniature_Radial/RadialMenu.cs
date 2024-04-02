using UI.Utility;
using UnityEngine;

namespace UI.Miniature_Radial
{
    public class RadialMenu : MonoBehaviour
    {
        [HideInInspector] public Animator MenuAnimator;
        [HideInInspector] public CullUnityEvent cullEventScript;

        private void Awake()
        {
            MenuAnimator = GetComponent<Animator>();
            cullEventScript = GetComponent<CullUnityEvent>();
        }
    }
}
