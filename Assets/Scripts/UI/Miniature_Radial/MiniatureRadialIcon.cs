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
        public Color BaseHighlightedColour;
        public Color BaseUnhighlightedColour;
        public Color IconHighlightedColour;
        public Color IconUnhighlightedColour;
        public enum RadialIconNameDirection { Left, Right }
        public RadialIconNameDirection Direction;
        
        [Header("On Press")] 
        public UnityEvent OnPressEvent;
        
        [HideInInspector] public bool Interactable;
        
        private TextMeshProUGUI _nameTMP;
        private Image _baseImage;
        private Material _iconMaterial;
        private Animator _nameAnimator;
        private IEnumerator _currentRoutine;

        private static readonly int BaseColour = Shader.PropertyToID("_BaseColour");
        private static readonly int Right = Animator.StringToHash("Right");
        private static readonly int Active = Animator.StringToHash("Active");

        private void Awake()
        {
            _nameAnimator = transform.GetChild(0).GetComponent<Animator>();
            _nameAnimator.SetBool(Right, Direction == RadialIconNameDirection.Right);
            _nameTMP = transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            _baseImage = transform.GetChild(1).GetComponent<Image>();
            var iconImage = transform.GetChild(2).GetComponent<Image>();
            
            // Create new instance of the material for each radial icon:
            _iconMaterial = new Material(iconImage.material);
            iconImage.material = _iconMaterial;
            _nameTMP.text = Name;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!Interactable) return;
            
            // Icon highlighted:
            _baseImage.color = BaseHighlightedColour;
            _iconMaterial.SetColor(BaseColour, IconHighlightedColour);
            _nameAnimator.SetBool(Active, true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!Interactable) return;
            
            // Icon un-highlighted:
            _baseImage.color = BaseUnhighlightedColour;
            _iconMaterial.SetColor(BaseColour, IconUnhighlightedColour);
            _nameAnimator.SetBool(Active, false);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!Interactable) return;
            OnPressEvent.Invoke();
        }
    }
}
