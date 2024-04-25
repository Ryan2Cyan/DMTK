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
        protected string _GUItitle;
        
        private RadialBase _radialBase;
        private SerializedProperty _title;
        private SerializedProperty _titleDirection;
        
        // Style variables:
        private Texture2D _boxBackground;
        private Sprite _circleSprite;
        private Font _draconisFont;
        private bool _titleFoldoutToggle;
        
        protected virtual void OnEnable()
        {
            _GUItitle = "Radial Base";
            _radialBase = (RadialBase)target;
            _title = serializedObject.FindProperty("Title");
            _titleDirection = serializedObject.FindProperty("TitleDisplayDirection");
            _circleSprite = Resources.Load<Sprite>("Sprites/Circle");
            _draconisFont = Resources.Load<Font>("Fonts/Draconis");
            _boxBackground = EditorUtility.MakeClearTexure(2, 2, Color.clear);
            _titleFoldoutToggle = true;
        }
    
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            serializedObject.Update();
            
            // Title:
            EditorUtility.DrawTitle(_GUItitle, 25, _draconisFont);
            
            // Title settings:
            EditorUtility.Vertical(() =>
            {
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
                                var isLeft = _radialBase.TitleDisplayDirection == RadialBase.RadialTitleDisplayDirection.Left;
                                
                                EditorUtility.TextBox(!isLeft ? "" : _radialBase.Title, _boxBackground, _draconisFont, 80f, 50f);
                                EditorUtility.Sprite(_circleSprite, _boxBackground,50f, 50f);
                                EditorUtility.TextBox(isLeft ? "" : _radialBase.Title, _boxBackground, _draconisFont, 80f, 50f);
                                
                            }, GUILayoutAlignment.Centre); 
                        });
                    }, 1);
                    if(_radialBase.Title == "") EditorUtility.Warning("No title field present!");
                }, ref _titleFoldoutToggle, "Title Settings"); 
            });
            
            serializedObject.ApplyModifiedProperties();
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
