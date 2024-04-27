using UI.Miniature_Radial;
using UnityEditor;
using UnityEngine;

namespace Editor.CustomEditors.Radials
{
    [CustomEditor(typeof(RadialBasic)), CanEditMultipleObjects]
    public class RadialBasicCustomCustomEditor : RadialBaseCustomEditor
    {
        private RadialBasic _radialBasic;
        
        private SerializedProperty _useColour;
        private SerializedProperty _baseHighlightedColour;
        private SerializedProperty _baseUnhighlightedColour;
        private SerializedProperty _iconHighlightedColour;
        private SerializedProperty _iconUnhighlightedColour;

        private SerializedProperty _useSprite;
        private SerializedProperty _iconHighlightedSprite;
        private SerializedProperty _iconUnhighlightedSprite;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            _GUItitle = "Radial Basic";
            _radialBasic = (RadialBasic)target;
            
            // Serialized properties:
            _useColour = serializedObject.FindProperty("UseColour");
            _baseHighlightedColour = serializedObject.FindProperty("BaseHighlightedColour");
            _baseUnhighlightedColour = serializedObject.FindProperty("BaseUnhighlightedColour");
            _iconHighlightedColour = serializedObject.FindProperty("IconHighlightedColour");
            _iconUnhighlightedColour = serializedObject.FindProperty("IconUnhighlightedColour");
            
            _useSprite = serializedObject.FindProperty("UseSprites");
            _iconHighlightedSprite = serializedObject.FindProperty("IconHighlightedSprite");
            _iconUnhighlightedSprite = serializedObject.FindProperty("IconUnhighlightedSprite");
        }

        protected override void ExecuteRadialSections()
        {
            base.ExecuteRadialSections();
            RadialFoldoutSection(BasicSection, ref RadialGlobalSettings.BasicFoldOutToggle, "Highlight/Unhighlight Settings");
        }

        protected override void AssignGUIStates()
        {
            base.AssignGUIStates();
            _radialGUIStates.Add(new RadialGUIState("Unhighlighted", _radialBasic.SetGUI, _radialBasic.UseColour ? _radialBasic.BaseUnhighlightedColour : _radialBasic.DefaultBaseColour,  _radialBasic.UseColour ? _radialBasic.IconUnhighlightedColour : _radialBasic.DefaultIconColour, _radialBasic.UseSprites ? _radialBasic.IconUnhighlightedSprite : _radialBasic.DefaultIconSprite));
            _radialGUIStates.Add(new RadialGUIState("Highlighted", _radialBasic.SetGUI, _radialBasic.UseColour ? _radialBasic.BaseHighlightedColour : _radialBasic.DefaultBaseColour,  _radialBasic.UseColour ? _radialBasic.IconHighlightedColour : _radialBasic.DefaultIconColour, _radialBasic.UseSprites ? _radialBasic.IconHighlightedSprite : _radialBasic.DefaultIconSprite));
        }

        protected void BasicSection()
        {
            EditorUtility.Indent(() =>
            {
                // Colour settings:
                _useColour.boolValue = GUILayout.Toggle(_useColour.boolValue, "Use Colours");
                if (_useColour.boolValue)
                {
                    EditorUtility.Horizontal(() =>
                    {
                        EditorGUILayout.LabelField("Base");
                        EditorGUILayout.LabelField("Icon");
                    });
                    EditorGUILayout.Space();
                    EditorUtility.Horizontal(() =>
                    {
                        EditorGUILayout.PropertyField(_baseUnhighlightedColour);
                        EditorGUILayout.PropertyField(_iconUnhighlightedColour);
                    });

                    EditorUtility.Horizontal(() =>
                    {
                        EditorGUILayout.PropertyField(_baseHighlightedColour);
                        EditorGUILayout.PropertyField(_iconHighlightedColour);
                    });
                }
                
                // Sprite Settings:
                _useSprite.boolValue = GUILayout.Toggle(_useSprite.boolValue, "Use Sprites");
                if (_useSprite.boolValue)
                {
                    EditorGUILayout.PropertyField(_iconUnhighlightedSprite);
                    EditorGUILayout.PropertyField(_iconHighlightedSprite);
                }
            }, 1);
        }
    }
}
