using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Miniature_Radial
{
    /// <summary>Icon that appears within miniature radial UI. Can be highlighted, or un-highlighted. Clicking
    /// on a radial icon will have a different effect depending on it's event.</summary>
    public class MiniatureRadialIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [Header("Settings")]
        public string Name;
        public Color BaseSelectedColour;
        public Color BaseUnselectedColour;
        public Color IconSelectedColour;
        public Color IconUnselectedColour;
        public float MaskX;
        public enum RadialIconNameDirection { Left, Right }
        public RadialIconNameDirection Direction;
        
        [Header("On Press")] 
        public UnityEvent OnPressEvent;

        private Transform _maskTransform;
        private Transform _labelTransform;
        private TextMeshProUGUI _nameTMP;
        private Image _baseImage;
        private Material _iconMaterial;
        private IEnumerator _currentRoutine;

        private const float _nameStartPositionX = -70;
        private const float _nameRightEndPositionX = 30;
        private const float _nameLeftEndPositionX = -100;

        private static readonly int BaseColour = Shader.PropertyToID("_BaseColour");

        private void Awake()
        {
            _maskTransform = transform.GetChild(0).transform;
            _labelTransform = transform.GetChild(0).GetChild(0).transform;
            _nameTMP = GetComponentInChildren<TextMeshProUGUI>();
            _baseImage = transform.GetChild(1).GetComponent<Image>();
            var iconImage = transform.GetChild(2).GetComponent<Image>();
            
            // Create new instance of the material for each radial icon:
            _iconMaterial = new Material(iconImage.material);
            iconImage.material = _iconMaterial;
            _nameTMP.text = Name;
        }

        private void Update()
        {
            _maskTransform.localPosition = new Vector3(MaskX, _maskTransform.localPosition.y, _maskTransform.localPosition.z);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // Icon highlighted:
            _baseImage.color = BaseSelectedColour;
            _iconMaterial.SetColor(BaseColour, IconSelectedColour);
            if(_currentRoutine != null) StopCoroutine(_currentRoutine);
            StartCoroutine(_currentRoutine = MoveNameBar(_labelTransform.localPosition, new Vector3(Direction == RadialIconNameDirection.Left ? _nameLeftEndPositionX : _nameRightEndPositionX, 0, 0), 0.25f));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // Icon un-highlighted:
            _baseImage.color = BaseUnselectedColour;
            _iconMaterial.SetColor(BaseColour, IconUnselectedColour);
            if(_currentRoutine != null) StopCoroutine(_currentRoutine);
            StartCoroutine(_currentRoutine = MoveNameBar(_labelTransform.localPosition, new Vector3(_nameStartPositionX, 0, 0), 0.25f));
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnPressEvent.Invoke();
        }

        private IEnumerator MoveNameBar(Vector3 startPosition, Vector3 endPosition, float executionTime)
        {
            var elapsedTime = 0f;
            while (elapsedTime < executionTime)
            {
                _labelTransform.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / executionTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }  
            _labelTransform.localPosition = endPosition;
            yield return null;
        }
    }
}
