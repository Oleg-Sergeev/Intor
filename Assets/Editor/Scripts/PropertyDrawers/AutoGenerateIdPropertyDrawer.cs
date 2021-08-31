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

            const int ExtraWidth = 3;

            var buttonRect = new Rect(position.width - ExtraWidth, position.y, position.height + ExtraWidth, position.height);
            var guiContent = new GUIContent("\u21A9", "Refresh ID");

            var enabled = GUI.enabled;
            GUI.enabled = true;
            if (GUI.Button(buttonRect, guiContent, "toolbarbutton")) GenerateId(property);
            GUI.enabled = enabled;

            var propertyRect = new Rect(position.position, new Vector2(position.width - position.height - ExtraWidth, position.height));
            EditorGUI.PropertyField(propertyRect, property, label, true);
        }


        private void GenerateId(SerializedProperty property)
        {
            property.stringValue = Guid.NewGuid().ToString();
        }
    }
}
