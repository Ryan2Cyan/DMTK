using UI.Miniature_Radial;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    // [CustomEditor(typeof(RadialToggle))]
    // public class RadialToggleEditor : UnityEditor.Editor
    // {
    //     private RadialToggle _radialToggle;
    //     private bool _foldOutToggle;
    //
    //     private void OnEnable()
    //     {
    //         _radialToggle = (RadialToggle)target;
    //     }
    //
    //     public override void OnInspectorGUI()
    //     {
    //         // Draw title:
    //         EditorUtility.Vertical(
    //             () =>
    //             {
    //                 EditorUtility.DrawTitle("Radial Toggle", 15);
    //                 GUILayout.Label("Toggle: " + _radialToggle.Toggle);
    //                 
    //                 // GUI.color = _radialToggle.TestInt > 0 ? Color.red : Color.white;
    //                 EditorUtility.FoldOut(() => {
    //                     EditorUtility.Horizontal(() =>
    //                         {
    //                             EditorGUILayout.LabelField("Test Int", GUILayout.MaxWidth(50));
    //                             _radialToggle.TestInt = EditorGUILayout.FloatField(_radialToggle.TestInt);
    //                             EditorGUILayout.LabelField("Test Int", GUILayout.MaxWidth(50));
    //                             _radialToggle.TestInt = EditorGUILayout.FloatField(_radialToggle.TestInt);
    //                         });
    //                     EditorGUI.ProgressBar(GUILayoutUtility.GetRect(50, 50),_radialToggle.TestInt / 100f, "Test"); 
    //                     }, ref _foldOutToggle, "Title");
    //                 
    //                 if(_radialToggle.SpriteToggleOn == null) EditorUtility.Warning("Toggle on sprite is null.");
    //                 if(_radialToggle.SpriteToggleOff == null) EditorUtility.Warning("Toggle off sprite is null.");
    //             });
    //     }
    // }
    
    [CustomEditor(typeof(RadialToggle))]
    [CanEditMultipleObjects]
    public class RadialToggleEditor2 : UnityEditor.Editor
    {
        private RadialToggle _radialToggle;
        private SerializedProperty _testInt; 
    
        private void OnEnable()
        {
            _radialToggle = (RadialToggle)target;
            _testInt = serializedObject.FindProperty("TestInt");
        }
    
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            serializedObject.Update();
            EditorUtility.Vertical(() =>
            {
                EditorUtility.Horizontal(() =>
                {
                    EditorGUILayout.LabelField("Icons Icon", GUILayout.MaxWidth(200));
                    EditorUtility.FlexibleSpace(() =>
                    {
                        if (_radialToggle.imageTest != null)
                        {
                            EditorUtility.Vertical(() =>
                            {
                                GUILayout.Box("Highlighted");
                                GUILayout.Box(AssetPreview.GetAssetPreview(_radialToggle.imageTest.sprite));    
                            });
                            EditorUtility.Vertical(() =>
                            {
                                GUILayout.Box("Unhighlighted");
                                GUILayout.Box(AssetPreview.GetAssetPreview(_radialToggle.imageTest.sprite));    
                            });
                        }
                    });
                });
            });
            serializedObject.ApplyModifiedProperties();
        }
    }
}
