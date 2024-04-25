
using System.Collections.Generic;
using UI.Miniature_Radial;
using Unity.Plastic.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class RadialGUIState
    {
        public RadialGUIState(string title, Action function, Color baseColour, Color iconColour)
        {
            Title = title;
            Function = function;
            BaseColour = baseColour;
            IconColour = iconColour;
        }
        
        public string Title;
        public Action Function;
        public Color BaseColour;
        public Color IconColour;
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
        private SerializedProperty _disabledBaseColour;
        private SerializedProperty _disabledIconColour;
        private SerializedProperty _disableOnEnable;
        private SerializedProperty _debugActive;

        protected List<RadialGUIState> _radialGUIStates = new();
        
        // Style variables:
        private Texture2D _boxBackground;
        private Sprite _circleSprite;
        private Font _draconisFont;
        private int _foldOutFontSize;
        
        private bool _titleFoldoutToggle;
        private bool _disableFoldoutToggle;
        private bool _eventsFoldoutToggle;
        private bool _debugFoldoutToggle;
        
        protected virtual void OnEnable()
        {
            _GUItitle = "Radial Base";
            _radialBase = (RadialBase)target;
            
            // Serialised properties:
            _title = serializedObject.FindProperty("Title");
            _titleDirection = serializedObject.FindProperty("TitleDisplayDirection");
            _onPressEvent = serializedObject.FindProperty("OnPressEvent");
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
            // DrawDefaultInspector();
            serializedObject.Update();
            AssignGUIStates();
            
            // Main title:
            EditorUtility.DrawTitle(_GUItitle, 25, _draconisFont);

            EditorUtility.Vertical(() =>
            {
                // States:
                RadialStatesSection();
                
                // Sections:
                RadialFoldoutSection(TitleSection, ref _titleFoldoutToggle, "Title Settings");
                RadialFoldoutSection(DisabledSettingsSection, ref _disableFoldoutToggle, "Disabled Settings");
                RadialFoldoutSection(EventsSection, ref _eventsFoldoutToggle, "Events Settings");
                RadialFoldoutSection(DebugSection, ref _debugFoldoutToggle, "Debug Settings");
            });
                
            _radialGUIStates.Clear();
            serializedObject.ApplyModifiedProperties();
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
                        EditorUtility.Vertical(() =>
                        {
                            foreach (var guiState in _radialGUIStates)
                            {
                                EditorUtility.TextBox(guiState.Title, _boxBackground, _draconisFont, 100f, 20f);
                                EditorUtility.SpriteButton(
                                    EditorUtility.MakeRadialTexture(guiState.BaseColour, _radialBase.IconImage.sprite.texture, guiState.IconColour)
                                    , guiState.Function, 100f, 50f);
                            }
                        });
                    }, GUILayoutAlignment.Centre);
                });
            }, 1);
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

                    EditorUtility.TextBox(!isLeft ? "" : _radialBase.Title, _boxBackground, _draconisFont,
                        80f, 50f);
                    EditorUtility.Sprite(_circleSprite, _boxBackground, 50f, 50f);
                    EditorUtility.TextBox(isLeft ? "" : _radialBase.Title, _boxBackground, _draconisFont,
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
        
        protected void AssignGUIStates()
        {
            _radialGUIStates.Add(new RadialGUIState("Disabled", _radialBase.GUIDisable, _radialBase.DisabledBaseColour, _radialBase.DisabledIconColour));
        }
        
        #endregion
    }

    [CustomEditor(typeof(RadialToggle)), CanEditMultipleObjects]
    public class RadialToggleEditor : RadialBaseEditor
    {
        private RadialToggle _radialToggle;

        protected override void OnEnable()
        {
            base.OnEnable();
            _GUItitle = "Radial Toggle";
            _radialToggle = (RadialToggle)target;
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}
