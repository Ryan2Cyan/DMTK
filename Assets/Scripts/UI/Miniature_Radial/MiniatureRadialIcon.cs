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
        public float NameOffsetPosition = 1f;

        [Header("On Press")] 
        public UnityEvent OnPressEvent;
        
        private TextMeshProUGUI _nameTMP;
        private SpriteRenderer _baseSprite;
        private Material _spriteMaterial;
        
        private static readonly int BaseColour = Shader.PropertyToID("_BaseColour");

        private void Awake()
        {
            _nameTMP = GetComponentInChildren<TextMeshProUGUI>();
            _baseSprite = transform.GetChild(1).GetComponent<SpriteRenderer>();
            _spriteMaterial = transform.GetChild(2).GetComponent<CanvasRenderer>().GetMaterial();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // Icon highlighted:
            _baseSprite.color = BaseSelectedColour;
            _spriteMaterial.SetColor(BaseColour, IconSelectedColour);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // Icon un-highlighted:
            _baseSprite.color = BaseUnselectedColour;
            _spriteMaterial.SetColor(BaseColour, IconUnselectedColour);
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
                transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / executionTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }  
            transform.position = endPosition;
            yield return null;
        }
    }
}
