using System.Globalization;
using Camera;
using TMPro;
using UnityEngine;
using Utility;

namespace UI.DistanceArrow
{
    public class DistanceTravelled : MonoBehaviour, IPooledObject
    {
        [Header("Settings")] 
        public float Distance = 20f;
        public Transform Target;
        public TMP_FontAsset Font;
        public float FontSize = 20f;
        public Vector2 Offset = new (1f, 1f);

        private UnityEngine.Camera _camera;
        private TextMeshProUGUI _tmp;
        private bool _instantiated;
        
        private void Awake()
        {
            _tmp = GetComponent<TextMeshProUGUI>();
            _tmp.font = Font;
            _tmp.fontSize = FontSize;
            _camera = CameraManager.Instance.MainCamera;
        }
        
        private void Update()
        {
            if (!_instantiated) return;
            var targetPosition = _camera.WorldToScreenPoint(Target.position);
            var transform1 = transform;
            transform1.position = new Vector3(targetPosition.x + Offset.x, targetPosition.y + Offset.y, transform1.position.z);
            _tmp.text = Distance.ToString(CultureInfo.InvariantCulture) + " ft";
        }

        public void Instantiate()
        {
            gameObject.SetActive(true);
            _tmp = GetComponent<TextMeshProUGUI>();
            _tmp.font = Font;
            _tmp.fontSize = FontSize;
            _instantiated = true;
            Distance = 0f;
        }

        public void Release()
        {
            gameObject.SetActive(false);
        }
    }
}
