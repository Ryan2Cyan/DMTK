using System;
using System.Collections.Generic;
using UI.Miniature_Radial;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Action = Unity.Plastic.Antlr3.Runtime.Misc.Action;

namespace Editor
{
    public class RadialGUIState
    {
        public RadialGUIState(string title, Action<Color, Color, Sprite> function, Color baseColour, Color iconColour, Sprite iconSprite)
        {
            Title = title;
            Function = function;
            BaseColour = baseColour;
            IconColour = iconColour;
            IconSprite = iconSprite;
        }
        
        public readonly string Title;
        public readonly Action<Color, Color, Sprite> Function;
        public readonly Color BaseColour;
        public readonly Color IconColour;
        public readonly Sprite IconSprite;
    }
    
    [CustomEditor(typeof(RadialBase)), CanEditMultipleObjects]
    public class RadialBaseEditor : UnityEditor.Editor
    {
        protected string _GUItitle;
        
        private RadialBase _radialBase;
        
        // Serialised Values:
        private SerializedProperty _title;
        private SerializedProperty _titleDirection;
        private SerializedProperty _onPressEvent;
        private SerializedProperty _defaultIconSprite;
        private SerializedProperty _defaultBaseColour;
        private SerializedProperty _defaultIconColour;
        private SerializedProperty _disabledBaseColour;
        private SerializedProperty _disabledIconColour;
        private SerializedProperty _disableOnEnable;
        private SerializedProperty _debugActive;

        protected readonly List<RadialGUIState> _radialGUIStates = new();
        
        // Style variables:
        private Texture2D _boxBackground;
        private Sprite _circleSprite;
        private Font _draconisFont;
        private int _foldOutFontSize;
        
        protected virtual void OnEnable()
        {
            _GUItitle = "Radial Base";
            _radialBase = (RadialBase)target;
            _radialBase.BaseImage = _radialBase.transform.GetChild(1).GetComponent<Image>();
            _radialBase.IconImage = _radialBase.transform.GetChild(2).GetComponent<Image>();
            
            // Serialised properties:
            _title = serializedObject.FindProperty("Title");
            _titleDirection = serializedObject.FindProperty("TitleDisplayDirection");
            _onPressEvent = serializedObject.FindProperty("OnPressEvent");
            _defaultIconSprite = serializedObject.FindProperty("DefaultIconSprite");
            _defaultBaseColour = serializedObject.FindProperty("DefaultBaseColour");
            _defaultIconColour = serializedObject.FindProperty("DefaultIconColour");
            _disabledBaseColour = serializedObject.FindProperty("DisabledBaseColour");
            _disabledIconColour = serializedObject.FindProperty("DisabledIconColour");
            _disableOnEnable = serializedObject.FindProperty("DisableOnEnable");
            _debugActive = serializedObject.FindProperty("DebugActive");
            
            // Style assets:
            _circleSprite = Resources.Load<Sprite>("Sprites/Circle");
            _draconisFont = Resources.Load<Font>("Fonts/Draconis");
            _boxBackground = EditorUtility.MakeClearTexture(2, 2, Color.clear);
            _foldOutFontSize = 12;
        }
    
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            AssignGUIStates();
            
            // Main title:
            EditorUtility.DrawTitle(_GUItitle, 25, _draconisFont);
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            
            EditorUtility.Vertical(() =>
            {
                // States:
                RadialStatesSection();
                
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                
                // Sections:
                ExecuteRadialSections();
            });
                
            _radialGUIStates.Clear();
            serializedObject.ApplyModifiedProperties();
            SceneView.RepaintAll();
        }

        #region RadialSections

        private void RadialStatesSection()
        {
            EditorUtility.Indent(() =>
            {
                EditorUtility.Horizontal(() =>
                {
                    EditorUtility.Align(() =>
                    {
                        foreach (var guiState in _radialGUIStates)
                        {
                            EditorUtility.Vertical(() =>
                            {
                                if (_radialBase.IconImage != null)
                                {
                                    EditorUtility.TextButton(guiState.Title, (() =>
                                    {
                                        guiState.Function.Invoke(guiState.BaseColour, guiState.IconColour, guiState.IconSprite);
                                    }), _draconisFont, 12, 75f, 50f);
                                }
                            });
                        }
                    }, GUILayoutAlignment.Centre);
                });
            }, 1);
        }

        protected virtual void ExecuteRadialSections()
        {
            RadialFoldoutSection(DefaultSettingsSection, ref RadialGlobalSettings.DefaultFoldOutToggle, "Default Settings");
            RadialFoldoutSection(TitleSection, ref RadialGlobalSettings.TitleFoldOutToggle, "Title Settings");
            RadialFoldoutSection(DisabledSettingsSection, ref RadialGlobalSettings.DisableFoldOutToggle, "Disabled Settings");
            RadialFoldoutSection(EventsSection, ref RadialGlobalSettings.EventsFoldOutToggle, "Events Settings");
            RadialFoldoutSection(DebugSection, ref RadialGlobalSettings.DebugFoldOutToggle, "Debug Settings");
        }
        
        protected virtual void DefaultSettingsSection()
        {
            GUILayout.Label("Colours");
            EditorGUILayout.PropertyField(_defaultBaseColour);
            EditorGUILayout.PropertyField(_defaultIconColour);
            EditorGUILayout.Space();
            GUILayout.Label("Sprites");
            EditorGUILayout.PropertyField(_defaultIconSprite);
            if(_radialBase.DefaultIconSprite == null) EditorUtility.Warning("Default Icon Sprite not assigned!");
        }
        
        protected virtual void DisabledSettingsSection()
        {
            EditorGUILayout.PropertyField(_disabledBaseColour);
            EditorGUILayout.PropertyField(_disabledIconColour);
            EditorGUILayout.PropertyField(_disableOnEnable);
        }
        
        protected virtual void EventsSection()
        {
            EditorGUILayout.PropertyField(_onPressEvent);
        }
        
        protected virtual void DebugSection()
        {
            EditorGUILayout.PropertyField(_debugActive);
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            
            EditorUtility.Horizontal(() =>
            {
                EditorGUILayout.LabelField("Interactable: ");
                EditorGUILayout.LabelField(_radialBase.Disabled.ToString());
            });
            
            EditorUtility.Horizontal(() =>
            {
                EditorGUILayout.LabelField("Interactable: ");
                EditorGUILayout.LabelField(_radialBase.Interactable.ToString());
            });
            
            EditorUtility.Horizontal(() =>
            {
                EditorGUILayout.LabelField("Highlighted: ");
                EditorGUILayout.LabelField(_radialBase.Highlighted.ToString());
            });
            
            EditorUtility.Horizontal(() =>
            {
                EditorGUILayout.LabelField("UI Element Active: ");
                EditorGUILayout.LabelField(_radialBase.UIElementActive.ToString());
            });
        }

        protected virtual void TitleSection()
        {
            EditorUtility.Align(() =>
            {
                EditorGUILayout.PropertyField(_title);
                EditorGUILayout.PropertyField(_titleDirection);
            }, GUILayoutAlignment.Right);
            EditorGUILayout.Space();

            EditorGUILayout.Space();
            EditorUtility.Horizontal(() =>
            {
                EditorUtility.Align(() =>
                {
                    var isLeft = _radialBase.TitleDisplayDirection ==
                                 RadialBase.RadialTitleDisplayDirection.Left;

                    EditorUtility.TextBox(!isLeft ? "" : _radialBase.Title, _boxBackground, _draconisFont, 18,
                        80f, 50f);
                    EditorUtility.Sprite(_circleSprite, _boxBackground, 50f, 50f);
                    EditorUtility.TextBox(isLeft ? "" : _radialBase.Title, _boxBackground, _draconisFont, 18,
                        80f, 50f);

                }, GUILayoutAlignment.Centre);
            });
            if (_radialBase.Title == "") EditorUtility.Warning("No title field present!");
        }
        
        #endregion
        
        #region GeneralFunctions
        
        protected void RadialFoldoutSection(Action encapsulatedFields, ref bool foldOutToggle, string title)
        {
            EditorUtility.FoldOut(() =>
            {
                EditorUtility.Indent(() =>
                {
                    EditorGUILayout.Space();
                    encapsulatedFields.Invoke();
                    EditorGUILayout.Space();
                    EditorGUILayout.Space();
                }, 1);
            }, ref foldOutToggle, title, _foldOutFontSize);
        }
        
        protected virtual void AssignGUIStates()
        {
            _radialGUIStates.Add(new RadialGUIState("Default", _radialBase.SetGUI, _radialBase.DefaultBaseColour, _radialBase.DefaultIconColour, _radialBase.IconImage.sprite));
            _radialGUIStates.Add(new RadialGUIState("Disabled", _radialBase.SetGUI, _radialBase.DisabledBaseColour, _radialBase.DisabledIconColour, _radialBase.IconImage.sprite));
        }
        
        #endregion
    }
    
    [CustomEditor(typeof(RadialToggle)), CanEditMultipleObjects]
    public class RadialToggleEditor : RadialBaseEditor
    {
        private RadialToggle _radialToggle;
        
        private SerializedProperty _toggleColours;
        private SerializedProperty _baseOffHighlightColour;
        private SerializedProperty _baseOffUnhighlightColour;
        private SerializedProperty _baseOnHighlightColour;
        private SerializedProperty _baseOnUnhighlightColour;
        
        private SerializedProperty _iconOffHighlightColour;
        private SerializedProperty _iconOffUnhighlightColour;
        private SerializedProperty _iconOnHighlightColour;
        private SerializedProperty _iconOnUnhighlightColour;
        
        private SerializedProperty _toggleSprite;
        private SerializedProperty _spriteToggleOn;
        private SerializedProperty _spriteToggleOff;
        
        protected bool _showToggleSprites;

        protected override void OnEnable()
        {
            base.OnEnable();
            _GUItitle = "Radial Toggle";
            _radialToggle = (RadialToggle)target;
            
            // Serialized properties:
            _toggleColours = serializedObject.FindProperty("ToggleColours");
            _baseOffHighlightColour = serializedObject.FindProperty("BaseHighlightOffColour");
            _baseOffUnhighlightColour = serializedObject.FindProperty("BaseUnhighlightOffColour");
            _baseOnHighlightColour = serializedObject.FindProperty("BaseHighlightOnColour");
            _baseOnUnhighlightColour = serializedObject.FindProperty("BaseUnhighlightOnColour");
            
            _iconOffHighlightColour = serializedObject.FindProperty("IconHighlightOffColour");
            _iconOffUnhighlightColour = serializedObject.FindProperty("IconUnhighlightOffColour");
            _iconOnHighlightColour = serializedObject.FindProperty("IconHighlightOnColour");
            _iconOnUnhighlightColour = serializedObject.FindProperty("IconUnhighlightOnColour");
            
            _toggleSprite = serializedObject.FindProperty("ToggleSprite");
            _spriteToggleOn = serializedObject.FindProperty("SpriteToggleOn");
            _spriteToggleOff = serializedObject.FindProperty("SpriteToggleOff");
        }
        
        protected override void AssignGUIStates()
        {
            base.AssignGUIStates();
            var useSprites = _radialToggle.ToggleSprite;
            var useColours = _radialToggle.ToggleColours;
            _radialGUIStates.Add(new RadialGUIState("Toggle Off (UH)", _radialToggle.SetGUI, useColours ? _radialToggle.BaseUnhighlightOffColour : _radialToggle.DefaultBaseColour, useColours ? _radialToggle.IconUnhighlightOffColour : _radialToggle.DefaultIconColour, useSprites ? _radialToggle.SpriteToggleOff : _radialToggle.DefaultIconSprite));
            _radialGUIStates.Add(new RadialGUIState("Toggle On (UH)", _radialToggle.SetGUI, useColours ? _radialToggle.BaseUnhighlightOnColour : _radialToggle.DefaultBaseColour, useColours ? _radialToggle.IconUnhighlightOnColour : _radialToggle.DefaultIconColour, useSprites ? _radialToggle.SpriteToggleOn : _radialToggle.DefaultIconSprite));
            _radialGUIStates.Add(new RadialGUIState("Toggle Off (H)", _radialToggle.SetGUI, useColours ? _radialToggle.BaseHighlightOffColour : _radialToggle.DefaultBaseColour, useColours ? _radialToggle.IconHighlightOffColour : _radialToggle.DefaultIconColour, useSprites ? _radialToggle.SpriteToggleOff : _radialToggle.DefaultIconSprite));
            _radialGUIStates.Add(new RadialGUIState("Toggle On (H)", _radialToggle.SetGUI, useColours ? _radialToggle.BaseHighlightOnColour : _radialToggle.DefaultBaseColour, useColours ? _radialToggle.IconHighlightOnColour : _radialToggle.DefaultIconColour, useSprites ? _radialToggle.SpriteToggleOn : _radialToggle.DefaultIconSprite));
        }
        
        protected virtual void ToggleSection()
        {
            _toggleColours.boolValue = GUILayout.Toggle(_toggleColours.boolValue, "Use Colours");
            if(_toggleColours.boolValue) 
            {
                EditorGUILayout.Space();
                EditorUtility.Indent(() =>
                {
                    EditorUtility.Horizontal(() =>
                    {
                        EditorGUILayout.LabelField("Base");
                        EditorGUILayout.LabelField("Icon");
                    });
                    EditorGUILayout.Space();
                    EditorUtility.Horizontal(() =>
                    {
                        EditorGUILayout.PropertyField(_baseOffUnhighlightColour);
                        EditorGUILayout.PropertyField(_iconOffUnhighlightColour);
                    });
                    
                    EditorUtility.Horizontal(() =>
                    {
                        EditorGUILayout.PropertyField(_baseOnUnhighlightColour);
                        EditorGUILayout.PropertyField(_iconOnUnhighlightColour);
                    });
                    
                    EditorUtility.Horizontal(() =>
                    {
                        EditorGUILayout.PropertyField(_baseOffHighlightColour);
                        EditorGUILayout.PropertyField(_iconOffHighlightColour);
                    });
                    
                    EditorUtility.Horizontal(() =>
                    {
                        EditorGUILayout.PropertyField(_baseOnHighlightColour);
                        EditorGUILayout.PropertyField(_iconOnHighlightColour);
                    });
                }, 1);
            }
            
            _toggleSprite.boolValue = GUILayout.Toggle(_toggleSprite.boolValue, "Use Icon Sprites");
            if (_toggleSprite.boolValue)
            {
                EditorGUILayout.Space();
                EditorUtility.Indent(() =>
                {
                    EditorGUILayout.PropertyField(_spriteToggleOff);
                    EditorGUILayout.PropertyField(_spriteToggleOn);
                }, 1);
                if(_radialToggle.SpriteToggleOff == null) EditorUtility.Warning("Toggle Off Sprite not assigned!");
                if(_radialToggle.SpriteToggleOn == null) EditorUtility.Warning("Toggle On Sprite not assigned!");
            }
        }

        protected override void DebugSection()
        {
            base.DebugSection();
            EditorUtility.Horizontal(() =>
            {
                EditorGUILayout.LabelField("Toggle: ");
                EditorGUILayout.LabelField(_radialToggle.Toggle.ToString());
            });
        }

        protected override void ExecuteRadialSections()
        {
            base.ExecuteRadialSections();
            RadialFoldoutSection(ToggleSection, ref RadialGlobalSettings.ToggleFoldOutToggle, "Toggle Settings");
        }
    }

    public static class RadialGlobalSettings
    {
        public static bool DefaultFoldOutToggle = true;
        public static bool TitleFoldOutToggle = true;
        public static bool DisableFoldOutToggle = true;
        public static bool EventsFoldOutToggle = true;
        public static bool DebugFoldOutToggle = true;
        public static bool ToggleFoldOutToggle = true;
    }
}
