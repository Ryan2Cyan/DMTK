using System;
using System.Collections.Generic;
using UI.Miniature_Radial;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Action = Unity.Plastic.Antlr3.Runtime.Misc.Action;

namespace Editor
{
    public static class RadialGlobalSettings
    {
        public static bool DefaultFoldOutToggle = true;
        public static bool TitleFoldOutToggle = true;
        public static bool DisableFoldOutToggle = true;
        public static bool EventsFoldOutToggle = true;
        public static bool DebugFoldOutToggle = true;
        public static bool ToggleFoldOutToggle = true;
        public static bool BasicFoldOutToggle = true;
        public static bool IntegerFoldOutToggle = true;
    }
    
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
    public class RadialBaseCustomEditor : UnityEditor.Editor
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
                                    EditorUtility.TextButton(guiState.Title, () =>
                                    {
                                        guiState.Function.Invoke(guiState.BaseColour, guiState.IconColour, guiState.IconSprite);
                                    }, _draconisFont, 12, 75f, 50f);
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
                EditorGUILayout.LabelField("Disabled: ");
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
}