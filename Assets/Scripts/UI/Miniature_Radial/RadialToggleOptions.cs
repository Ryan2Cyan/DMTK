namespace UI.Miniature_Radial
{
    public class RadialToggleOptions : RadialToggle
    {
        public void TurnOn()
        {
            if(!_initialised) OnInitialise();
            if (Disabled) return;
            OnHighlight();
            Toggle = true;
            _baseImage.color = BaseHighlightedOnColour;
            _iconImage.color = IconHighlightedOnColour;
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
            _titleAnimator.SetBool(Active, false);
        }
    }
}
