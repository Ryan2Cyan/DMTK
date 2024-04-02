using UI.Utility;
using UnityEngine;

namespace UI.Miniature_Radial
{
    public class RadialMenu : MonoBehaviour
    {
        [HideInInspector] public Animator MenuAnimator;
        [HideInInspector] public CullImages CullImagesScript;

        private void Awake()
        {
            MenuAnimator = GetComponent<Animator>();
            CullImagesScript = GetComponent<CullImages>();
        }
    }
}
