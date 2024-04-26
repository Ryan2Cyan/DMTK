using UI.Miniature_Radial;
using UnityEditor;

namespace Editor
{
    [CustomEditor(typeof(RadialBasic)), CanEditMultipleObjects]
    public class RadialBasicCustomCustomEditor : RadialBaseCustomEditor
    {
        private RadialBasic _radialBasic;
        
        private SerializedProperty _baseHighlightedColour;
        private SerializedProperty _baseUnhighlightedColour;
        
        private SerializedProperty _iconHighlightedColour;
        private SerializedProperty _iconUnhighlightedColour;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            _GUItitle = "Radial Basic";
            _radialBasic = (RadialBasic)target;
            
            // Serialized properties:
            _baseHighlightedColour = serializedObject.FindProperty("BaseHighlightedColour");
            _baseUnhighlightedColour = serializedObject.FindProperty("BaseUnhighlightedColour");
            _iconHighlightedColour = serializedObject.FindProperty("IconHighlightedColour");
            _iconUnhighlightedColour = serializedObject.FindProperty("IconUnhighlightedColour");
        }

        protected override void ExecuteRadialSections()
        {
            base.ExecuteRadialSections();
            RadialFoldoutSection(BasicSection, ref RadialGlobalSettings.BasicFoldOutToggle, "Highlight/Unhighlight Settings");
        }

        protected override void AssignGUIStates()
        {
            base.AssignGUIStates();
            _radialGUIStates.Add(new RadialGUIState("Unhighlighted", _radialBasic.SetGUI, _radialBasic.BaseUnhighlightedColour,  _radialBasic.IconUnhighlightedColour, _radialBasic.DefaultIconSprite));
            _radialGUIStates.Add(new RadialGUIState("Highlighted", _radialBasic.SetGUI, _radialBasic.BaseHighlightedColour,  _radialBasic.IconHighlightedColour, _radialBasic.DefaultIconSprite));
        }

        protected void BasicSection()
        {
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
                    EditorGUILayout.PropertyField(_baseUnhighlightedColour);
                    EditorGUILayout.PropertyField(_iconUnhighlightedColour);
                });
                    
                EditorUtility.Horizontal(() =>
                {
                    EditorGUILayout.PropertyField(_baseHighlightedColour);
                    EditorGUILayout.PropertyField(_iconHighlightedColour);
                });
            }, 1);
        }
    }
}
