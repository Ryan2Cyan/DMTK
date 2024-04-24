using System;
using UnityEditor;
using UnityEngine;
using Action = Unity.Plastic.Antlr3.Runtime.Misc.Action;

namespace Editor
{
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

        public static void FlexibleSpace(Action encapsulatedFields)
        {
            GUILayout.FlexibleSpace();
            encapsulatedFields.Invoke();
            GUILayout.FlexibleSpace();
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
                FlexibleSpace(() =>
                {
                    var normalTextSize = GUI.skin.label.fontSize;
                    GUI.skin.label.fontSize = titleSize;
                    GUILayout.Label(titleText);
                    GUI.skin.label.fontSize = normalTextSize;
                });
            });
        }

        public static void FoldOut(Action encapsulatedFields, ref bool toggle, string title)
        {
            toggle = EditorGUILayout.Foldout(toggle, title);
            if(toggle) encapsulatedFields.Invoke();
        }

        public static void Warning(string message)
        {
            EditorGUILayout.HelpBox(message, MessageType.Warning);
        }
        
        public static Texture2D ConvertSpriteToTexture(Sprite sprite)
        {
            try
            {
                if (sprite.rect.width != sprite.texture.width)
                {
                    Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
                    Color[] colors = newText.GetPixels();
                    Color[] newColors = sprite.texture.GetPixels((int)System.Math.Ceiling(sprite.textureRect.x),
                        (int)System.Math.Ceiling(sprite.textureRect.y),
                        (int)System.Math.Ceiling(sprite.textureRect.width),
                        (int)System.Math.Ceiling(sprite.textureRect.height));
                    Debug.Log(colors.Length+"_"+ newColors.Length);
                    newText.SetPixels(newColors);
                    newText.Apply();
                    return newText;
                }
                else
                    return sprite.texture;
            }catch
            {
                return sprite.texture;
            }
        }
    }
}
