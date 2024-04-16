using System;
using Tabletop.Miniatures;
using UnityEditor;
using UnityEngine;

namespace Editor
{
#if UNITY_EDITOR
    
    /// <remarks>Source: https://stackoverflow.com/questions/58984486/create-scriptable-object-with-constant-unique-id</remarks>
    [CustomPropertyDrawer(typeof(ScriptableObjectIdAttribute))]
    public class ScriptableObjectIdDrawer : PropertyDrawer 
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) 
        {
            GUI.enabled = false;
            if (string.IsNullOrEmpty(property.stringValue)) property.stringValue = Guid.NewGuid().ToString();
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
    
#endif
}
