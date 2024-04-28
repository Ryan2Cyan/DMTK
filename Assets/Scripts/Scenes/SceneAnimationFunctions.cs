using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes
{
    public class SceneAnimationFunctions : MonoBehaviour
    {
        public void LoadMainScene()
        {
            SceneManager.LoadScene(1);
        }
    }
}
