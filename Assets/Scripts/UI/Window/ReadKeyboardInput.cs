using System.Collections;
using Input;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Window
{
    public class ReadKeyboardInput : UIElement, IInputElement
    {
        [Header("Settings")] 
        public Color ActiveColour;
        public Color InactiveColour;
        public int MaximumCharacters;
        public bool NumbersOnly;
        public bool ClearOnActive;
        
        [Header("Components")]
        public TextMeshProUGUI TMPText;
        public Image ImageUI;

        [Header("Event")] 
        public UnityEvent OnNewValueSet;

        private string _cachedValue;
        private bool _active;

        #region PublicFunctions

        public int GetDigitValue()
        {
            return !NumbersOnly ? 0 : int.Parse(TMPText.text);
        }

        public string GetStringValue()
        {
            return TMPText.text;
        }

        public void SetStringValue(string value)
        {
            TMPText.text = value;
        }
        
        #endregion
        
        #region InputFunctions
        
        public void OnMouseEnter()
        {
            if (_active) return;
            ImageUI.color = ActiveColour;
        }

        public void OnMouseExit()
        {
            if (_active) return;
            ImageUI.color = InactiveColour;
        }
        

        public void OnMouseDown()
        {
            if(!_active) Activate();
            else Deactivate();
        }

        public void OnMouseUp() { }
        public void OnDrag() { }
        
        #endregion

        #region PrivateFunctions
        
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
            _cachedValue = TMPText.text;
            if (ClearOnActive) TMPText.text = "|";
            else TMPText.text += "|";
            ImageUI.color = ActiveColour;
            var characterAdded = false;

            while (_active)
            {
                foreach (var character in UnityEngine.Input.inputString)
                {
                    switch (character)
                    {
                        // has backspace/delete been pressed?
                        case '\b':
                        {
                            DeleteCharacter();
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
                            AddCharacter(character);
                        }break;
                    }
                }
                yield return null;
            }

            if (characterAdded)
            {
                TMPText.text = TMPText.text.Substring(0, TMPText.text.Length - 1);
                if(TMPText.text.Length == 0) TMPText.text = "0";
                OnNewValueSet?.Invoke();
            }
            else
            {
                TMPText.text = _cachedValue;
            }
            
            yield return null;
            yield break;

            void DeleteCharacter()
            {
                if(TMPText.text.Length == 1) return;
                TMPText.text = TMPText.text.Remove(TMPText.text.Length - 2, 1);
            }
            
            void AddCharacter(char character)
            {
                // Check if the entered character is a number:
                if (NumbersOnly)
                {
                    if (!char.IsDigit(character)) return;
                }
                if(TMPText.text.Length > MaximumCharacters) return;
                
                // Insert new character before carriage:
                TMPText.text = TMPText.text.Insert(TMPText.text.Length - 1, character.ToString());
                characterAdded = true;
            }
        }
        
        #endregion
    }
}
