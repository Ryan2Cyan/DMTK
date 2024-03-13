using System.Globalization;
using TMPro;
using UnityEngine;
using Utility;

namespace UI
{
    public class DistanceTravelled : MonoBehaviour, IPooledObject
    {
        [Header("Settings")] 
        public float Distance = 20f;
        public Transform Target;
        public TMP_FontAsset Font;
        public float FontSize = 20f;
        public Vector2 Offset = new Vector2(1f, 1f);
        
        private TextMeshProUGUI _tmp;
        private bool _instantiated;
        
        private void Awake()
        {
            _tmp = GetComponent<TextMeshProUGUI>();
            _tmp.font = Font;
            _tmp.fontSize = FontSize;
        }
        
        private void Update()
        {
            if (!_instantiated) return;
            var targetPosition = Camera.main.WorldToScreenPoint(Target.position);
            transform.position = new Vector3(targetPosition.x + Offset.x, targetPosition.y + Offset.y, transform.position.z);
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
