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
            if (DisableOnEnable) return;
            
            Toggle = true;
            BaseImage.color = BaseUnhighlightOnColour;
            IconImage.color = IconUnhighlightOnColour;
            if(gameObject.activeInHierarchy) _titleAnimator.SetBool(ActiveParam, false);
        }
        
        
        public void TurnOff()
        {
            if(!_initialised) OnInitialise();
            if (DisableOnEnable) return;
            OnUnhighlight();
            Toggle = false;
            BaseImage.color = BaseUnhighlightOffColour;
            IconImage.color = IconUnhighlightOffColour;
        }

        protected override void OnPress()
        {
            if (DisableOnEnable) return;
            if (Toggle) return;
            
            Toggle = true;
            if (Highlighted)
            {
                BaseImage.color = Toggle ? BaseHighlightOnColour : BaseHighlightOffColour;
                IconImage.color = Toggle ? IconHighlightOnColour : IconHighlightOffColour;
            }
            else
            {
                BaseImage.color = Toggle ? BaseUnhighlightOnColour : BaseUnhighlightOffColour;
                IconImage.color = Toggle ? IconUnhighlightOnColour : IconUnhighlightOffColour;
            }

            OnPressEvent.Invoke();
            _titleAnimator.SetBool(ActiveParam, false);
        }
    }
}
