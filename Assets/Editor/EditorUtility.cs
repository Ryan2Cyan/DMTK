using UnityEditor;
using UnityEngine;
using Action = Unity.Plastic.Antlr3.Runtime.Misc.Action;

namespace Editor
{
    public enum GUILayoutAlignment
    {
        Left,
        Right,
        Centre
    }
    
    public static class EditorUtility
    {
        public static void Vertical(Action encapsulatedFields)
        {
            GUILayout.BeginVertical();
            encapsulatedFields.Invoke();
            GUILayout.EndVertical();
        }

        public static void Horizontal(Action encapsulatedFields)
        {
            GUILayout.BeginHorizontal();
            encapsulatedFields.Invoke();
            GUILayout.EndHorizontal();
        }

        public static void Align(Action encapsulatedFields, GUILayoutAlignment alignment)
        {
            if(alignment is GUILayoutAlignment.Right or GUILayoutAlignment.Centre) GUILayout.FlexibleSpace();
            encapsulatedFields.Invoke();
            if(alignment is GUILayoutAlignment.Left or GUILayoutAlignment.Centre) GUILayout.FlexibleSpace();
        }

        public static void Indent(Action encapsulatedFields, int indent)
        {
            EditorGUI.indentLevel += indent;
            encapsulatedFields.Invoke();
            EditorGUI.indentLevel -= indent;
        }
        
        public static void DrawTitle(string titleText, int titleSize)
        {
            GUILayout.Space(10);
            Horizontal(() =>
            {
                Align(() =>
                {
                    var normalTextSize = GUI.skin.label.fontSize;
                    GUI.skin.label.fontSize = titleSize;
                    GUILayout.Label(titleText);
                    GUI.skin.label.fontSize = normalTextSize;
                }, GUILayoutAlignment.Centre);
            });
        }

        public static void FoldOut(Action encapsulatedFields, ref bool toggle, string title)
        {
            toggle = EditorGUILayout.Foldout(toggle, title);
            if(toggle) encapsulatedFields.Invoke();
        }

        public static void Sprite(Sprite sprite, float width, float height)
        {
            GUILayout.Box(AssetPreview.GetAssetPreview(sprite), GUILayout.Width(width), GUILayout.Height(height));
        }
        
        public static void TextBox(string text, float width, float height)
        {
            GUILayout.Box(text, GUILayout.Width(width), GUILayout.Height(height));
        }

        public static void Warning(string message)
        {
            EditorGUILayout.HelpBox(message, MessageType.Warning);
        }
        
        public static Texture2D MakeClearTexure( int width, int height, Color col )
        {
            Color[] pix = new Color[width * height];
            for( int i = 0; i < pix.Length; ++i )
            {
                pix[ i ] = col;
            }
            Texture2D result = new Texture2D( width, height );
            result.SetPixels( pix );
            result.Apply();
            return result;
        }
    }
}
