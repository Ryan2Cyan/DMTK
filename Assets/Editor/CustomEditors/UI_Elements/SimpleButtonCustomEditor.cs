using System;
using UI.UI_Interactables;
using UnityEditor;
using UnityEngine;

namespace Editor.CustomEditors.UI_Elements
{
    // public static class SimpleButtonGlobalSettings
    // {
    //     public static int FoldOutFontSize = 12;
    //     public static bool ComponentsFoldOutToggle = true;
    // }
    //
    // [CustomEditor(typeof(SimpleButton)), CanEditMultipleObjects]
    // public class SimpleButtonCustomEditor : UnityEditor.Editor
    // {
    //     private SimpleButton _simpleButton;
    //     private Font _draconisFont;
    //     
    //     private SerializedProperty _image;
    //     private SerializedProperty _onPress;
    //     private SerializedProperty _unhighlightedColour;
    //     private SerializedProperty _highlightedColour;
    //     private SerializedProperty _uiElementActive;
    //     private SerializedProperty _uiElementPriority;
    //     
    //     
    //     protected virtual void OnEnable()
    //     {
    //         _simpleButton = (SimpleButton)target;
    //         _draconisFont = Resources.Load<Font>("Fonts/Draconis");
    //         
    //         // Serialised properties:
    //         _image = serializedObject.FindProperty("Image");
    //         _onPress = serializedObject.FindProperty("OnPress");
    //         _unhighlightedColour = serializedObject.FindProperty("UnhighlightedColour");
    //         _highlightedColour = serializedObject.FindProperty("HighlightedColour");
    //         _uiElementActive = serializedObject.FindProperty("UIElementActive");
    //         _uiElementPriority = serializedObject.FindProperty("UIElementPriority");
    //     }
    //     
    //     public override void OnInspectorGUI()
    //     {
    //         serializedObject.Update();
    //         
    //         // Main title:
    //         EditorUtility.DrawTitle("Simple Button", 25, _draconisFont);
    //         EditorGUILayout.Space();
    //         EditorGUILayout.Space();
    //         
    //         EditorUtility.Vertical(() =>
    //         {
    //             // Sections:
    //             
    //         });
    //         
    //         serializedObject.ApplyModifiedProperties();
    //         SceneView.RepaintAll();
    //     }
    //
    //     protected virtual void FoldOutSection(Action encapsulatedFields, ref bool toggle, string title)
    //     {
    //         EditorUtility.FoldOut(() =>
    //         {
    //             EditorUtility.Indent(() =>
    //             {
    //                 EditorGUILayout.Space();
    //                 encapsulatedFields.Invoke();
    //                 EditorGUILayout.Space();
    //                 EditorGUILayout.Space();
    //             }, 1);
    //         }, ref toggle, title, SimpleButtonGlobalSettings.FoldOutFontSize);
    //     }
    //     
    //     protected virtual void ComponentsSection()
    //     {
    //         EditorGUILayout.PropertyField(_image);
    //     }
    //
    //     protected virtual void ColoursSection()
    //     {
    //         
    //     }
    // }
}
