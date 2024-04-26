using UI.Miniature_Radial;
using UnityEditor;

namespace Editor
{
    [CustomEditor(typeof(RadialInteger)), CanEditMultipleObjects]
    public class RadialIntegerCustomEditor : RadialBaseCustomEditor
    {
        private RadialInteger _radialInteger;
        
        private SerializedProperty _baseActiveColour;
        private SerializedProperty _baseActiveHighlightColour;
        private SerializedProperty _baseInactiveColour;
        
        private SerializedProperty _iconActiveColour;
        private SerializedProperty _iconInactiveColour;
        
        private SerializedProperty _valueTextColour;
        
        private SerializedProperty _minValue;
        private SerializedProperty _maxValue;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            _GUItitle = "RadialInteger";
            _radialInteger = (RadialInteger)target;
            
            // Serialized properties:
            _baseActiveColour = serializedObject.FindProperty("BaseActiveColour");
            _baseActiveHighlightColour = serializedObject.FindProperty("BaseActiveHighlightColour");
            _baseInactiveColour = serializedObject.FindProperty("BaseInactiveColour");
            
            _iconActiveColour = serializedObject.FindProperty("IconActiveColour");
            _iconInactiveColour = serializedObject.FindProperty("IconInactiveColour");
            
            _valueTextColour = serializedObject.FindProperty("ValueTextColour");
            
            _minValue = serializedObject.FindProperty("MinValue");
            _maxValue = serializedObject.FindProperty("MaxValue");
        }

        protected override void ExecuteRadialSections()
        {
            base.ExecuteRadialSections();
            RadialFoldoutSection(IntegerSection, ref RadialGlobalSettings.IntegerFoldOutToggle, "Integer Settings");
        }

        protected override void AssignGUIStates()
        {
            base.AssignGUIStates();
            _radialGUIStates.Add(new RadialGUIState("Inactive", _radialInteger.SetGUI, _radialInteger.BaseActiveColour,  _radialInteger.IconActiveColour, _radialInteger.DefaultIconSprite));
            _radialGUIStates.Add(new RadialGUIState("Active", _radialInteger.SetGUI, _radialInteger.BaseInactiveColour,  _radialInteger.IconInactiveColour, _radialInteger.DefaultIconSprite));
        }

        protected override void DebugSection()
        {
            base.DebugSection();
            EditorUtility.Horizontal(() =>
            {
                EditorGUILayout.LabelField("Integer Value: ");
                EditorGUILayout.LabelField(_radialInteger.Value.ToString());
            });
            
            EditorUtility.Horizontal(() =>
            {
                EditorGUILayout.LabelField("Active: ");
                EditorGUILayout.LabelField(_radialInteger.Active.ToString());
            });
        }

        protected void IntegerSection()
        {
            EditorUtility.Indent(() =>
            {
                EditorUtility.Horizontal(() =>
                {
                    EditorGUILayout.PropertyField(_minValue);
                    EditorGUILayout.PropertyField(_maxValue);
                });
                
                EditorUtility.Horizontal(() =>
                {
                    EditorGUILayout.LabelField("Base");
                    EditorGUILayout.LabelField("Icon");
                });
                EditorGUILayout.Space();
                EditorUtility.Horizontal(() =>
                {
                    EditorGUILayout.PropertyField(_baseInactiveColour);
                    EditorGUILayout.PropertyField(_iconInactiveColour);
                });
                    
                EditorUtility.Horizontal(() =>
                {
                    EditorGUILayout.PropertyField(_baseActiveColour);
                    EditorGUILayout.PropertyField(_iconActiveColour);
                });
            }, 1);
        }
    }
}
