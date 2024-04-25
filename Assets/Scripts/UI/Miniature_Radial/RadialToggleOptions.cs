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
            BaseImage.color = BaseToggleOnColour;
            IconImage.color = IconToggleOnColour;
            if(gameObject.activeInHierarchy) _titleAnimator.SetBool(ActiveParam, false);
        }
        
        
        public void TurnOff()
        {
            if(!_initialised) OnInitialise();
            if (Disabled) return;
            OnUnhighlight();
            Toggle = false;
            BaseImage.color = BaseToggleOffColour;
            IconImage.color = IconToggleOffColour;
        }

        protected override void OnPress()
        {
            if (Disabled) return;
            if (Toggle) return;
            
            Toggle = true;
            if (Highlighted)
            {
                BaseImage.color = Toggle ? BaseHighlightedOnColour : BaseHighlightedOffColour;
                IconImage.color = Toggle ? IconHighlightedOnColour : IconHighlightedOffColour;
            }
            else
            {
                BaseImage.color = Toggle ? BaseToggleOnColour : BaseToggleOffColour;
                IconImage.color = Toggle ? IconToggleOnColour : IconToggleOffColour;
            }

            OnPressEvent.Invoke();
            _titleAnimator.SetBool(ActiveParam, false);
        }
    }
}
