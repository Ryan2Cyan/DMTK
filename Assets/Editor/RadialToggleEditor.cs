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
    
    
    [CustomEditor(typeof(RadialBase)), CanEditMultipleObjects]
    public class RadialBaseEditor : UnityEditor.Editor
    {
        private RadialBase _radialBase;
        private SerializedProperty _title;
        private SerializedProperty _titleDirection;

        private Sprite _circleSprite;
        private Font _defaultFont;
        private bool _titleFoldoutToggle;
        
        private void OnEnable()
        {
            _radialBase = (RadialBase)target;
            _title = serializedObject.FindProperty("Title");
            _titleDirection = serializedObject.FindProperty("TitleDisplayDirection");
            _circleSprite = Resources.Load<Sprite>("Sprites/Circle");
            _defaultFont = Resources.Load<Font>("Fonts/Draconis");
        }
    
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            // Setup:
            GUI.skin.box.alignment = TextAnchor.MiddleCenter;
            GUI.skin.box.fontSize = 18;
            GUI.skin.box.font = _defaultFont;
            GUI.skin.box.normal.background = EditorUtility.MakeClearTexure(2, 2, Color.clear);
            
            // Title settings:
            EditorUtility.Vertical(() =>
            {
                EditorUtility.FoldOut(() =>
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
                            var isLeft = _radialBase.TitleDisplayDirection == RadialBase.RadialTitleDisplayDirection.Left;
                            
                            EditorUtility.TextBox(!isLeft ? "" : _radialBase.Title, 80f, 50f);
                            EditorUtility.Sprite(_circleSprite, 50f, 50f);
                            EditorUtility.TextBox(isLeft ? "" : _radialBase.Title, 80f, 50f);
                            
                        }, GUILayoutAlignment.Centre); 
                    });
                    
                }, ref _titleFoldoutToggle, "Title Settings"); 
            });
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
