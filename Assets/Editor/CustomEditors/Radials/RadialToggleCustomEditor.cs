using UI.Miniature_Radial;
using UnityEditor;
using UnityEngine;

namespace Editor.CustomEditors.Radials
{
    [CustomEditor(typeof(RadialToggle)), CanEditMultipleObjects]
    public class RadialToggleCustomCustomEditor : RadialBaseCustomEditor
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
}
