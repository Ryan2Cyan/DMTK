using TMPro;
using UnityEngine;

namespace UI.Miniature_Radial
{
    public class RadialInteger : RadialBase
    {
        [Header("Base Colour Settings")]
        public Color BaseActiveColour;
        public Color BaseActiveHighlightColour;
        public Color BaseInactiveColour;
        
        [Header("Icon Colour Settings")]
        public Color IconActiveColour;
        public Color IconInactiveColour;
        
        [Header("Value Text Colour Settings")] 
        public Color ValueTextColour;
        
        [Header("Settings")] 
        public int MaxValue;
        public int MinValue;

        [HideInInspector] public int Value;
        
        private TextMeshProUGUI _valueText;
        public bool _active;

        protected override void Awake()
        {
            OnInitialise();
        }

        protected override void OnInitialise()
        {
            base.OnInitialise();
            _valueText = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
            _valueText.color = ValueTextColour;
            _valueText.enabled = false;
            _baseImage.color = BaseInactiveColour;
            _iconImage.color = IconInactiveColour;
        }

        protected override void OnHighlight()
        {
            base.OnHighlight();
            _valueText.enabled = true;
            if (_active) _baseImage.color = BaseActiveHighlightColour;
        }

        protected override void OnUnhighlight()
        {
            base.OnUnhighlight();
            _valueText.enabled = false;
            if (_active) _baseImage.color = BaseActiveColour;
        }

        protected override void OnPress()
        {
            Value++;
            if (Value > MaxValue) Value = MinValue;
            _valueText.text = Value.ToString();
            _active = Value != MinValue;

            if (_active)
            {
                _baseImage.color = BaseActiveHighlightColour;
                _iconImage.color = IconActiveColour;
            }
            else
            {
                _baseImage.color = BaseInactiveColour;
                _iconImage.color = IconInactiveColour;
            }
            base.OnPress();
        }
    }
}
