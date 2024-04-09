using System.Collections;
using Input;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Window
{
    public class ReadKeyboardInput : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Settings")] 
        public Color ActiveColour;
        public Color InactiveColour;
        
        [Header("Components")]
        public TextMeshProUGUI TMPText;
        public Image ImageUI;
        public enum KeyboardInputType
        {
            
        }
        
        private bool _active;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_active) return;
            ImageUI.color = ActiveColour;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_active) return;
            ImageUI.color = InactiveColour;
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if(!_active) Activate();
            else Deactivate();
        }

        private void Activate()
        {
            _active = true;
            StartCoroutine(ReadKeyboard());
        }
        
        private void Deactivate()
        {
            ImageUI.color = InactiveColour;
            _active = false;
        }

        private IEnumerator ReadKeyboard()
        {
            while (_active)
            {
                foreach (var character in UnityEngine.Input.inputString)
                {
                    switch (character)
                    {
                        // has backspace/delete been pressed?
                        case '\b':
                        {
                            if (TMPText.text.Length != 0) TMPText.text = TMPText.text.Substring(0, TMPText.text.Length - 1);
                        }break;
                        case '\n':
                        {
                            Deactivate();
                        } break;
                        case '\r':
                        {
                            Deactivate();
                        }break;
                        default:
                        {
                            TMPText.text += character;
                        }break;
                    }
                }
                yield return null;
            }

            yield return null;
        }
    }
}
