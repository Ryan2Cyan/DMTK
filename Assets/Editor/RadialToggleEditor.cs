
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
        private SerializedProperty _title;
        private SerializedProperty _titleDirection;
        private SerializedProperty _disabledBaseColour;
        private SerializedProperty _disabledIconColour;

        protected List<RadialGUIState> _radialGUIStates = new();
        
        // Style variables:
        private Texture2D _boxBackground;
        private Sprite _circleSprite;
        private Font _draconisFont;
        private int _foldOutFontSize;
        
        private bool _titleFoldoutToggle;
        private bool _disableFoldoutToggle;
        private bool _statesFoldoutToggle;
        
        protected virtual void OnEnable()
        {
            _GUItitle = "Radial Base";
            _radialBase = (RadialBase)target;
            _titleFoldoutToggle = true;
            _disableFoldoutToggle = true;
            
            // Serialised properties:
            _title = serializedObject.FindProperty("Title");
            _titleDirection = serializedObject.FindProperty("TitleDisplayDirection");
            _disabledBaseColour = serializedObject.FindProperty("DisabledBaseColour");
            _disabledIconColour = serializedObject.FindProperty("DisabledIconColour");
            
            // Style assets:
            _circleSprite = Resources.Load<Sprite>("Sprites/Circle");
            _draconisFont = Resources.Load<Font>("Fonts/Draconis");
            _boxBackground = EditorUtility.MakeClearTexture(2, 2, Color.clear);
            _foldOutFontSize = 13;
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
                // Title settings:
                EditorUtility.FoldOut(() =>
                {
                    EditorUtility.Indent(() =>
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
                    }, 1);
                    if (_radialBase.Title == "") EditorUtility.Warning("No title field present!");
                }, ref _titleFoldoutToggle, "Title Settings", _foldOutFontSize);

                // States:
                EditorUtility.FoldOut(() =>
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
                }, ref _statesFoldoutToggle, "Radial States", _foldOutFontSize);
            });
            
            // Disabled settings:
            EditorUtility.FoldOut(() =>
            {
                EditorUtility.Indent(() =>
                {
                    EditorGUILayout.PropertyField(_disabledBaseColour);
                    EditorGUILayout.PropertyField(_disabledIconColour);
                    EditorGUILayout.Space();

                    EditorGUILayout.Space();
                }, 1);
            }, ref _disableFoldoutToggle, "Disabled Settings", _foldOutFontSize);
                
            _radialGUIStates.Clear();
            serializedObject.ApplyModifiedProperties();
        }

        protected void AssignGUIStates()
        {
            _radialGUIStates.Add(new RadialGUIState("Disabled", _radialBase.GUIDisable, _radialBase.DisabledBaseColour, _radialBase.DisabledIconColour));
        }
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
