using System.Collections.Generic;
using System.Linq;
using Assets.Editor.Scripts.Utilities;
using Assets.Scripts.Extensions;
using Assets.Scripts.Utilities.Localization;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Scripts.CustomEditors
{
    [CustomEditor(typeof(LocalizationController))]
    public class LocalizationControllerEditor : UnityEditor.Editor
    {
        private const string DefaultLanguage = "EN";

        private Dictionary<string, Dictionary<string, string>> _locales;

        private HashSet<LocalizedString> _localizedStrings;

        private LocalizedString _newString;
        private LocalizedString _stringToDelete;

        private List<string> _languages;

        private List<Vector2> _scrolls;

        private string _currentLanguage = DefaultLanguage;

        private int _index = 0;




        private void OnEnable()
        {
            _localizedStrings = new HashSet<LocalizedString>();

            var temp = FindObjectsOfType<LocalizedString>(true);

            foreach (var item in temp) _localizedStrings.Add(item);


            var csv = LocalizationController.LoadCsvEditor();

            if (csv == null) return;

            _locales = new Dictionary<string, Dictionary<string, string>>();
            _languages = new List<string>();

            var langs = csv[0];

            for (int i = 0; i < langs.Length; i++)
            {
                var locales = csv.ToDictionary(lines => lines[0], lines => lines[i]);

                _locales.Add(langs[i].ToUpper(), locales);

                _languages.Add(langs[i]);
            }
            _languages.RemoveAt(0);


            _scrolls = new List<Vector2>();
            for (int i = 0; i < _locales[DefaultLanguage].Count; i++) _scrolls.Add(Vector2.zero);
        }

        public override void OnInspectorGUI()
        {
            DrawLocalizableStrings();


            if (_locales == null)
            {
                EditorGUILayout.HelpBox("Localization data not loaded", MessageType.Warning);

                return;
            }

            EditorGUILayout.Space(20);

            DrawCsv();
        }


        private void DrawLocalizableStrings()
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                foreach (var localizedString in _localizedStrings)
                {
                    using (new EditorGUILayout.HorizontalScope("box"))
                    {
                        var so = new SerializedObject(localizedString);

                        var prop = so.FindPropertyByAutoPropertyName("Key");

                        so.Update();

                        if (string.IsNullOrEmpty(prop.stringValue))
                            prop.stringValue = localizedString.GetComponent<TextMeshProUGUI>().text.ToUpper().Replace(" ", "_");

                        EditorGUILayout.LabelField(prop.displayName, GUILayout.MaxWidth(30f));

                        EditorGUILayout.PropertyField(prop, GUIContent.none, GUILayout.ExpandWidth(true));

                        so.ApplyModifiedProperties();

                        using (new EditorGUI.DisabledScope(true))
                        {
                            EditorGUILayout.ObjectField(localizedString, typeof(LocalizedString), true);
                        }

                        var deleteGuiContent = new GUIContent("-", "Delete localized string");
                        if (GUILayout.Button(deleteGuiContent))
                        {
                            _stringToDelete = localizedString;
                        }
                    }
                }
            }

            using (new EditorGUILayout.HorizontalScope("box"))
            {
                EditorGUILayout.LabelField("New localized string");

                TextMeshProUGUI newString = null;
                newString = (TextMeshProUGUI)EditorGUILayout.ObjectField(newString, typeof(TextMeshProUGUI), true);

                if (newString != null)
                {
                    if (!newString.TryGetComponent<LocalizedString>(out _))
                    {
                        var localizedString = newString.gameObject.AddComponent<LocalizedString>();
                        _localizedStrings.Add(localizedString);
                    }
                    else
                    {
                        Debug.LogWarning("The localized string is already in the list");
                    }
                }

                if (_stringToDelete != null)
                {
                    _localizedStrings.Remove(_stringToDelete);
                    DestroyImmediate(_stringToDelete);
                }
            }
        }

        private void DrawCsv()
        {
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                _index = EditorGUILayout.Popup("Select language", _index, _languages.ToArray());
                _currentLanguage = _languages[_index];

                EditorGUILayout.Space(20);

                var newKeys = new Dictionary<string, string>();
                var texts = new Dictionary<string, string>();

                var keyToDelete = "";

                int i = 0;
                foreach (var locale in _locales[_currentLanguage])
                {
                    using (new EditorGUILayout.VerticalScope("box"))
                    {
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            using (new EditorGUILayout.VerticalScope())
                            {
                                newKeys[locale.Key] = EditorGUILayout.TextField(locale.Key, GUILayout.MaxWidth(1000.0f));

                                if (GUILayout.Button("Delete item"))
                                    keyToDelete = locale.Key;
                            }

                            _scrolls[i] = EditorGUILayout.BeginScrollView(_scrolls[i], GUILayout.Height(70));

                            var guiStyle = new GUIStyle(EditorStyles.textArea)
                            {
                                wordWrap = true
                            };

                            texts[locale.Key] = EditorGUILayout.TextArea(locale.Value, guiStyle, GUILayout.MaxWidth(1000.0f), GUILayout.ExpandHeight(true));

                            EditorGUILayout.EndScrollView();
                        }

                        EditorGUILayout.Space();
                    }
                    
                    i++;
                }

                foreach (var text in texts)
                    _locales[_currentLanguage][text.Key] = text.Value;

                foreach (var key in newKeys)
                {
                    foreach (var lang in _locales.Keys)
                        _locales[lang].ChangeKey(key.Key, key.Value);

                    _locales["LANG"][key.Value] = key.Value;
                }

                foreach (var lang in _locales.Keys)
                    _locales[lang].Remove(keyToDelete);


                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Add new key")) AddNewKey();

                    if (GUILayout.Button("Save CSV")) SaveCsv();
                }
            }
        }


        private void AddNewKey()
        {
            foreach (var lang in _locales.Keys)
                _locales[lang].Add("New key", lang);
            _locales["LANG"]["New key"] = "New key";

            _scrolls.Add(Vector2.zero);
        }

        private void SaveCsv()
        {
            var csvToSave = new List<string>();

            var keys = _locales[DefaultLanguage].Keys;

            foreach (var key in keys)
            {
                csvToSave.Add(string.Join(LocalizationController.CommaSubstituteEditor.ToString(), _locales.Select(locale => _locales[locale.Key][key])));
            }

            LocalizationController.SaveCsvEditor(csvToSave);
        }
    }
}
