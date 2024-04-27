using UI.Miniature_Radial;
using UnityEditor;
using UnityEngine;

namespace Editor.CustomEditors.Radials
{
    [CustomEditor(typeof(RadialToggle)), CanEditMultipleObjects]
    public class RadialToggleCustomCustomEditor : RadialBaseCustomEditor
    {
        private RadialToggle _radialToggle;
        
        private SerializedProperty _toggleTrueOnAwake;
        private SerializedProperty _useColours;
        private SerializedProperty _baseOffHighlightColour;
        private SerializedProperty _baseOffUnhighlightColour;
        private SerializedProperty _baseOnHighlightColour;
        private SerializedProperty _baseOnUnhighlightColour;
        
        private SerializedProperty _iconOffHighlightColour;
        private SerializedProperty _iconOffUnhighlightColour;
        private SerializedProperty _iconOnHighlightColour;
        private SerializedProperty _iconOnUnhighlightColour;
        
        private SerializedProperty _useSprites;
        private SerializedProperty _iconOnHighlightSprite;
        private SerializedProperty _iconOnUnhighlightSprite;
        private SerializedProperty _iconOffHighlightSprite;
        private SerializedProperty _iconOffUnhighlightSprite;

        protected override void OnEnable()
        {
            base.OnEnable();
            _GUItitle = "Radial Toggle";
            _radialToggle = (RadialToggle)target;
            
            // Serialized properties:
            _toggleTrueOnAwake = serializedObject.FindProperty("ToggleTrueOnAwake");
            
            _useColours = serializedObject.FindProperty("UseColours");
            _baseOffHighlightColour = serializedObject.FindProperty("BaseHighlightOffColour");
            _baseOffUnhighlightColour = serializedObject.FindProperty("BaseUnhighlightOffColour");
            _baseOnHighlightColour = serializedObject.FindProperty("BaseHighlightOnColour");
            _baseOnUnhighlightColour = serializedObject.FindProperty("BaseUnhighlightOnColour");
            
            _iconOffHighlightColour = serializedObject.FindProperty("IconHighlightOffColour");
            _iconOffUnhighlightColour = serializedObject.FindProperty("IconUnhighlightOffColour");
            _iconOnHighlightColour = serializedObject.FindProperty("IconHighlightOnColour");
            _iconOnUnhighlightColour = serializedObject.FindProperty("IconUnhighlightOnColour");
            
            _useSprites = serializedObject.FindProperty("UseSprites");
            _iconOnHighlightSprite = serializedObject.FindProperty("IconOnHighlightSprite");
            _iconOnUnhighlightSprite = serializedObject.FindProperty("IconOnUnhighlightSprite");
            _iconOffHighlightSprite = serializedObject.FindProperty("IconOffHighlightSprite");
            _iconOffUnhighlightSprite = serializedObject.FindProperty("IconOffUnhighlightSprite");
        }
        
        protected override void AssignGUIStates()
        {
            base.AssignGUIStates();
            var useSprites = _radialToggle.UseSprites;
            var useColours = _radialToggle.UseColours;
            _radialGUIStates.Add(new RadialGUIState("Toggle Off (UH)", _radialToggle.SetGUI, useColours ? _radialToggle.BaseUnhighlightOffColour : _radialToggle.DefaultBaseColour, useColours ? _radialToggle.IconUnhighlightOffColour : _radialToggle.DefaultIconColour, useSprites ? _radialToggle.IconOffUnhighlightSprite : _radialToggle.DefaultIconSprite));
            _radialGUIStates.Add(new RadialGUIState("Toggle On (UH)", _radialToggle.SetGUI, useColours ? _radialToggle.BaseUnhighlightOnColour : _radialToggle.DefaultBaseColour, useColours ? _radialToggle.IconUnhighlightOnColour : _radialToggle.DefaultIconColour, useSprites ? _radialToggle.IconOnUnhighlightSprite : _radialToggle.DefaultIconSprite));
            _radialGUIStates.Add(new RadialGUIState("Toggle Off (H)", _radialToggle.SetGUI, useColours ? _radialToggle.BaseHighlightOffColour : _radialToggle.DefaultBaseColour, useColours ? _radialToggle.IconHighlightOffColour : _radialToggle.DefaultIconColour, useSprites ? _radialToggle.IconOffHighlightSprite : _radialToggle.DefaultIconSprite));
            _radialGUIStates.Add(new RadialGUIState("Toggle On (H)", _radialToggle.SetGUI, useColours ? _radialToggle.BaseHighlightOnColour : _radialToggle.DefaultBaseColour, useColours ? _radialToggle.IconHighlightOnColour : _radialToggle.DefaultIconColour, useSprites ? _radialToggle.IconOnHighlightSprite : _radialToggle.DefaultIconSprite));
        }
        
        protected virtual void ToggleSection()
        {
            _toggleTrueOnAwake.boolValue = GUILayout.Toggle(_toggleTrueOnAwake.boolValue, "Toggle On On Awake");
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            
            _useColours.boolValue = GUILayout.Toggle(_useColours.boolValue, "Use Colours");
            if(_useColours.boolValue) 
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
            
            _useSprites.boolValue = GUILayout.Toggle(_useSprites.boolValue, "Use Icon Sprites");
            if (!_useSprites.boolValue) return;
            
            EditorGUILayout.Space();
            EditorUtility.Indent(() =>
            {
                EditorGUILayout.PropertyField(_iconOnHighlightSprite);
                EditorGUILayout.PropertyField(_iconOnUnhighlightSprite);
                EditorGUILayout.PropertyField(_iconOffHighlightSprite);
                EditorGUILayout.PropertyField(_iconOffUnhighlightSprite);
            }, 1);
            
            if(_radialToggle.IconOnHighlightSprite == null) EditorUtility.Warning("Icon On Highlight sprite not assigned!");
            if(_radialToggle.IconOnUnhighlightSprite == null) EditorUtility.Warning("Icon On Unhighlight sprite not assigned!");
            if(_radialToggle.IconOffHighlightSprite == null) EditorUtility.Warning("Icon Off Highlight sprite not assigned!");
            if(_radialToggle.IconOffUnhighlightSprite == null) EditorUtility.Warning("Icon Off Unhighlight sprite not assigned!");
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
}
