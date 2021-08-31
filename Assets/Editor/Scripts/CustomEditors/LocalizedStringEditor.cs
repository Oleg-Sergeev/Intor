using Assets.Editor.Scripts.Utilities;
using Assets.Scripts.Utilities.Localization;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Scripts.CustomEditors
{
    [CustomEditor(typeof(LocalizedString))]
    public class LocalizedStringEditor : UnityEditor.Editor
    {
        private SerializedProperty _key;

        private TextMeshProUGUI _uiText;


        private void OnEnable()
        {
            _key = serializedObject.FindPropertyByAutoPropertyName("Key");

            _uiText = ((LocalizedString)target).GetComponent<TextMeshProUGUI>();
        }


        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUILayout.BeginHorizontal();

            GUILayout.Label(_key.displayName, GUILayout.MaxWidth(30f));

            EditorGUILayout.PropertyField(_key, GUIContent.none, true, GUILayout.ExpandWidth(true));
            var guiContent = new GUIContent("\u21A9", "Refresh key");

            if (string.IsNullOrEmpty(_key.stringValue)) SetKey(_key);
            if (GUILayout.Button(guiContent, "toolbarbutton", GUILayout.Width(20))) SetKey(_key);

            GUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }


        private void SetKey(SerializedProperty property)
        {
            property.stringValue = _uiText.text.ToUpper().Replace(" ", "_");
        }
    }
}
