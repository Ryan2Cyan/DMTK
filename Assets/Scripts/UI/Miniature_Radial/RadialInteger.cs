using TMPro;
using UnityEngine;

namespace UI.Miniature_Radial
{
    public class RadialInteger : RadialBase
    {
        public Color BaseActiveColour;
        public Color BaseActiveHighlightColour;
        public Color BaseInactiveColour;
        
        public Color IconActiveColour;
        public Color IconInactiveColour;
        
        public Color ValueTextColour;
        
        public int MaxValue;
        public int MinValue;

        public int Value
        {
            get => _value;
            set
            {
                _value = value;
                UpdateUI();
            }
        }

        protected int _value;
        
        private TextMeshProUGUI _valueText;
        public bool Active;

        protected override void Awake()
        {
            UIElementPriority = 1;
            OnInitialise();
        }

        protected override void OnInitialise()
        {
            base.OnInitialise();
            _valueText = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
            _valueText.color = ValueTextColour;
            _valueText.enabled = false;
            BaseImage.color = BaseInactiveColour;
            IconImage.color = IconInactiveColour;
        }

        protected override void OnHighlight()
        {
            base.OnHighlight();
            _valueText.enabled = true;
            if (Active) BaseImage.color = BaseActiveHighlightColour;
        }

        protected override void OnUnhighlight()
        {
            base.OnUnhighlight();
            _valueText.enabled = false;
            if (Active) BaseImage.color = BaseActiveColour;
        }

        protected override void OnPress()
        {
            Value++;
            UpdateUI();
            base.OnPress();
        }

        private void UpdateUI()
        {
            if (Value > MaxValue) Value = MinValue;
            _valueText.text = Value.ToString();
            Active = Value != MinValue;

            if (Active)
            {
                BaseImage.color = BaseActiveHighlightColour;
                IconImage.color = IconActiveColour;
            }
            else
            {
                BaseImage.color = BaseInactiveColour;
                IconImage.color = IconInactiveColour;
            }
        }
    }
}
