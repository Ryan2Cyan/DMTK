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
            if (alignment is GUILayoutAlignment.Right or GUILayoutAlignment.Centre) GUILayout.FlexibleSpace();
            encapsulatedFields.Invoke();
            if (alignment is GUILayoutAlignment.Left or GUILayoutAlignment.Centre) GUILayout.FlexibleSpace();
        }

        public static void Indent(Action encapsulatedFields, int indent)
        {
            EditorGUI.indentLevel += indent;
            encapsulatedFields.Invoke();
            EditorGUI.indentLevel -= indent;
        }

        public static void DrawTitle(string titleText, int titleSize, Font font)
        {
            var defaultFontSize = GUI.skin.label.fontSize;
            GUI.skin.label.fontSize = titleSize;

            var defaultFont = GUI.skin.label.font;
            GUI.skin.label.font = font;

            GUILayout.Space(10);
            Horizontal(() => { Align(() => { GUILayout.Label(titleText); }, GUILayoutAlignment.Centre); });
            GUI.skin.label.fontSize = defaultFontSize;
            GUI.skin.label.font = defaultFont;
        }

        public static void FoldOut(Action encapsulatedFields, ref bool toggle, string title, int fontSize = 12, Font font = null)
        {
            toggle = EditorUtilityFoldout.Font(lambdaToggle =>
            {
                lambdaToggle = EditorUtilityFoldout.FontSize(lambdaToggle0 =>
                {
                    lambdaToggle0 = EditorUtilityFoldout.FontStyle(lambdaToggle1 => EditorGUILayout.Foldout(lambdaToggle1, title), lambdaToggle0, FontStyle.Bold);
                    return lambdaToggle0;
                }, lambdaToggle, fontSize);
                return lambdaToggle;
            }, toggle, font);
            if (toggle) encapsulatedFields.Invoke();
        }

        public static void Sprite(Sprite sprite, Texture2D background, float width, float height)
        {
            EditorUtilityBox.Alignment(
                () =>
                {
                    EditorUtilityBox.Background(
                        () =>
                        {
                            GUILayout.Box(AssetPreview.GetAssetPreview(sprite), GUILayout.Width(width),
                                GUILayout.Height(height));
                        }, background);
                }, TextAnchor.MiddleCenter);

        }

        public static void SpriteButton(Texture2D texture, Action buttonAction, float width, float height)
        {
            if (GUILayout.Button(texture, GUILayout.Width(width), GUILayout.Height(height)))
            {
                buttonAction.Invoke();
            }
        }
        
        public static void TextButton(string text, Action buttonAction, Font font, int fontSize, float width, float height)
        {
            EditorUtilityBox.FontSize(() =>
            {
                EditorUtilityButton.Font(() =>
                {
                    if (GUILayout.Button(text, GUILayout.Width(width), GUILayout.Height(height))) buttonAction.Invoke();
                }, font); 
            }, fontSize);
        }

        public static void TextBox(string text, Texture2D background, Font font, int fontSize, float width, float height)
        {
            EditorUtilityBox.Alignment(
                () =>
                {
                    EditorUtilityBox.FontSize(
                        () =>
                        {
                            EditorUtilityBox.Font(
                                () =>
                                {
                                    EditorUtilityBox.Background(
                                        () =>
                                        {
                                            EditorUtilityBox.TextColour(
                                                () =>
                                                {
                                                    GUILayout.Box(text, GUILayout.Width(width), GUILayout.Height(height));
                                                }, Color.white);
                                        }, background);
                                }, font);
                        }, fontSize);
                }, TextAnchor.MiddleCenter);
        }

        public static void Warning(string message)
        {
            EditorGUILayout.HelpBox(message, MessageType.Warning);
        }

        /// <remarks>Source: https://forum.unity.com/threads/change-gui-box-color.174609/ </remarks>
        public static Texture2D MakeClearTexture(int width, int height, Color col)
        {
            var pix = new Color[width * height];
            for (var i = 0; i < pix.Length; ++i) pix[i] = col;
            var result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
        
        public static Texture2D MakeRadialTexture(Color baseColour, Texture2D iconTexture, Color iconColour)
        {
            var width = iconTexture.width;
            var height = iconTexture.height;
            var pix = new Color[width * height];
            var iconPix = iconTexture.GetPixels();

            for (var i = 0; i < iconPix.Length; ++i)
            {
                pix[i] = iconPix[i].a > 0 ? iconColour : baseColour;
            }

            var result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
    }

    public static class EditorUtilityButton
    {
        public static void Background(Action encapsulatedFields, Texture2D background)
        {
            var defaultValue = GUI.skin.button.normal.background;
            GUI.skin.button.normal.background = background;
            encapsulatedFields.Invoke();
            GUI.skin.button.normal.background = defaultValue;
        }
        
        public static void Font(Action encapsulatedFields, Font font)
        {
            var defaultValue = GUI.skin.button.font;
            GUI.skin.button.font = font;
            encapsulatedFields.Invoke();
            GUI.skin.button.font = defaultValue;
        }
        
        public static void FontSize(Action encapsulatedFields, int fontSize)
        {
            var defaultValue = GUI.skin.button.fontSize;
            GUI.skin.button.fontSize = fontSize;
            encapsulatedFields.Invoke();
            GUI.skin.button.fontSize = defaultValue;
        }
    }
    
    public static class EditorUtilityFoldout
    {
        public delegate bool EditorFoldoutDelegate(bool toggle);
        
        public static bool FontStyle(EditorFoldoutDelegate encapsulatedFields, bool toggle, FontStyle fontStyle)
        {
            var defaultValue = EditorStyles.foldout.fontStyle;
            EditorStyles.foldout.fontStyle = fontStyle;
            var result = encapsulatedFields.Invoke(toggle);
            EditorStyles.foldout.fontStyle = defaultValue;
            return result;
        }
        
        public static bool FontSize(EditorFoldoutDelegate encapsulatedFields, bool toggle, int size)
        {
            var defaultValue = EditorStyles.foldout.fontSize;
            EditorStyles.foldout.fontSize = size;
            var result = encapsulatedFields.Invoke(toggle);
            EditorStyles.foldout.fontSize = defaultValue;
            return result;
        }
        
        public static bool Font(EditorFoldoutDelegate encapsulatedFields, bool toggle, Font font)
        {
            var defaultValue = EditorStyles.foldout.font;
            EditorStyles.foldout.font = font;
            var result = encapsulatedFields.Invoke(toggle);
            EditorStyles.foldout.font = defaultValue;
            return result;
        }
    }
    
    public static class EditorUtilityBox
    {
        // Box Styles:
        public static void Alignment(Action encapsulatedFields, TextAnchor alignment)
        {
            var defaultValue = GUI.skin.box.alignment;
            GUI.skin.box.alignment = alignment;
            encapsulatedFields.Invoke();
            GUI.skin.box.alignment = defaultValue;
        }

        public static void FontSize(Action encapsulatedFields, int size)
        {
            var defaultValue = GUI.skin.box.fontSize;
            GUI.skin.box.fontSize = size;
            encapsulatedFields.Invoke();
            GUI.skin.box.fontSize = defaultValue;
        }

        public static void Font(Action encapsulatedFields, Font font)
        {
            var defaultValue = GUI.skin.box.font;
            GUI.skin.box.font = font;
            encapsulatedFields.Invoke();
            GUI.skin.box.font = defaultValue;
        }

        public static void Background(Action encapsulatedFields, Texture2D background)
        {
            var defaultValue = GUI.skin.box.normal.background;
            GUI.skin.box.normal.background = background;
            encapsulatedFields.Invoke();
            GUI.skin.box.normal.background = defaultValue;
        }

        public static void TextColour(Action encapsulatedFields, Color colour)
        {
            var defaultValue = GUI.skin.box.normal.textColor;
            GUI.skin.box.normal.textColor = colour;
            encapsulatedFields.Invoke();
            GUI.skin.box.normal.textColor = defaultValue;
        }
    }
}
