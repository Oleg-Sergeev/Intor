using System;
using Assets.Scripts.PropertyAttributes;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Scripts.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(AutoGenerateIdAttribute))]
    public class AutoGenerateIdPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (string.IsNullOrEmpty(property.stringValue))
                GenerateId(property);

            var textureGUIStyle = new GUIStyle(GUI.skin.button) { padding = new RectOffset(4, 4, 4, 4) };
            var buttonRect = new Rect(position.width, position.y, position.height, position.height);
            var guiContent = new GUIContent(Resources.Load<Texture2D>("reset-icon"), "Refresh ID");

            var enabled = GUI.enabled;
            GUI.enabled = true;
            if (GUI.Button(buttonRect, guiContent, textureGUIStyle)) GenerateId(property);
            GUI.enabled = enabled;

            var propertyRect = new Rect(position.position, new Vector2(position.width - position.height, position.height));
            EditorGUI.PropertyField(propertyRect, property, label, true);
        }


        private void GenerateId(SerializedProperty property)
        {
            property.stringValue = Guid.NewGuid().ToString();
        }
    }
}
