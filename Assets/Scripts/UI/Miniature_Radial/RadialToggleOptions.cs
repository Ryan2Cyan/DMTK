namespace UI.Miniature_Radial
{
    public class RadialToggleOptions : RadialToggle
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            UIElementActive = true;
        }

        private void OnDisable()
        {
            UIElementActive = false;
        }

        public void TurnOn()
        {
            if(!_initialised) OnInitialise();
            if (Disabled) return;
            
            Toggle = true;
            _baseImage.color = BaseToggleOnColour;
            _iconImage.color = IconToggleOnColour;
            if(gameObject.activeInHierarchy) _titleAnimator.SetBool(ActiveParam, false);
        }
        
        
        public void TurnOff()
        {
            if(!_initialised) OnInitialise();
            if (Disabled) return;
            OnUnhighlight();
            Toggle = false;
            _baseImage.color = BaseToggleOffColour;
            _iconImage.color = IconToggleOffColour;
        }

        protected override void OnPress()
        {
            if (Disabled) return;
            if (Toggle) return;
            
            Toggle = true;
            if (Highlighted)
            {
                _baseImage.color = Toggle ? BaseHighlightedOnColour : BaseHighlightedOffColour;
                _iconImage.color = Toggle ? IconHighlightedOnColour : IconHighlightedOffColour;
            }
            else
            {
                _baseImage.color = Toggle ? BaseToggleOnColour : BaseToggleOffColour;
                _iconImage.color = Toggle ? IconToggleOnColour : IconToggleOffColour;
            }

            OnPressEvent.Invoke();
            _titleAnimator.SetBool(ActiveParam, false);
        }
    }
}
