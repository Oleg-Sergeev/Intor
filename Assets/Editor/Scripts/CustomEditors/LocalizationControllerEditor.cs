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

        private static Dictionary<string, bool> FadeLocalizedStrings;
        private static Dictionary<string, bool> FadeLocales;


        private Dictionary<string, Dictionary<string, string>> _locales;

        private Dictionary<string, HashSet<LocalizedString>> _localizedStrings;

        private List<string> _languages;

        private List<Vector2> _scrollsCsv;
        private List<Vector2> _scrollsLocalizedString;

        private string _currentLanguage = DefaultLanguage;

        private int _index = 0;


        private void OnEnable()
        {
            var uniqueLocalizedStrings = new HashSet<LocalizedString>();

            var allLocalazedStrings = FindObjectsOfType<LocalizedString>(true);
            foreach (var item in allLocalazedStrings) uniqueLocalizedStrings.Add(item);

            _localizedStrings = new Dictionary<string, HashSet<LocalizedString>>();

            var s = uniqueLocalizedStrings.GroupBy(s => s.Key);

            if (FadeLocalizedStrings == null) FadeLocalizedStrings = new Dictionary<string, bool>();
            if (FadeLocales == null) FadeLocales = new Dictionary<string, bool>();

            _scrollsLocalizedString = new List<Vector2>();
            foreach (var group in s)
            {
                _localizedStrings.Add(group.Key, new HashSet<LocalizedString>());

                foreach (var item in group)
                    _localizedStrings[group.Key].Add(item);

                _scrollsLocalizedString.Add(Vector2.zero);

                if (!FadeLocalizedStrings.ContainsKey(group.Key)) FadeLocalizedStrings.Add(group.Key, true);
            }


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


            _scrollsCsv = new List<Vector2>();
            for (int i = 0; i < _locales[DefaultLanguage].Count; i++) _scrollsCsv.Add(Vector2.zero);

            foreach (var key in _locales[DefaultLanguage].Keys)
                if (!FadeLocales.ContainsKey(key)) FadeLocales.Add(key, true);
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
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button(nameof(ExpandAll))) ExpandAll(FadeLocalizedStrings);
                if (GUILayout.Button(nameof(CollapseAll))) CollapseAll(FadeLocalizedStrings);
            }


            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                var keys = _localizedStrings.Keys.ToDictionary(key => key, key => key);
                var keyToDelete = "";
                int i = 0;

                LocalizedString stringToDelete = null;
                string keyOfStringToDelete = "";

                foreach (var locilizedStrings in _localizedStrings)
                {
                    var oldKey = locilizedStrings.Key;

                    using (new EditorGUILayout.HorizontalScope("box"))
                    {
                        if (FadeLocalizedStrings[oldKey])
                        {
                            FadeLocalizedStrings[oldKey] = EditorGUILayout.Foldout(FadeLocalizedStrings[oldKey], "");
                            EditorGUILayout.Space(-60);

                            using (new EditorGUILayout.VerticalScope())
                            {
                                keys[oldKey] = EditorGUILayout.TextField(oldKey, GUILayout.ExpandWidth(true));

                                if (GUILayout.Button("Delete key"))
                                    keyToDelete = oldKey;
                            }


                            using (var scrollView = new EditorGUILayout.ScrollViewScope(_scrollsLocalizedString[i], GUILayout.Height(65)))
                            {
                                _scrollsLocalizedString[i] = scrollView.scrollPosition;


                                foreach (var item in locilizedStrings.Value)
                                {
                                    using (new EditorGUILayout.HorizontalScope())
                                    {
                                        using (new EditorGUI.DisabledScope(true))
                                        {
                                            EditorGUILayout.ObjectField(item, typeof(LocalizedString), true);
                                        }

                                        if (GUILayout.Button("-"))
                                        {
                                            stringToDelete = item;
                                            keyOfStringToDelete = oldKey;
                                        }
                                    }

                                    var so = new SerializedObject(item);

                                    var prop = so.FindPropertyByAutoPropertyName("Key");

                                    so.Update();

                                    prop.stringValue = oldKey;

                                    so.ApplyModifiedProperties();
                                }
                            }
                        }
                        else
                        {
                            FadeLocalizedStrings[oldKey] = EditorGUILayout.Foldout(FadeLocalizedStrings[oldKey], oldKey, true);
                        }
                    }

                    i++;
                }

                foreach (var key in keys)
                    _localizedStrings.ChangeKey(key.Key, key.Value);

                if (stringToDelete != null)
                {
                    _localizedStrings[keyOfStringToDelete].Remove(stringToDelete);
                    DestroyImmediate(stringToDelete);
                }

                if (!string.IsNullOrEmpty(keyToDelete))
                {
                    foreach (var localizedString in _localizedStrings[keyToDelete])
                    {
                        DestroyImmediate(localizedString);
                    }

                    _localizedStrings.Remove(keyToDelete);
                }
            }


            using (new EditorGUILayout.HorizontalScope("box"))
            {
                EditorGUILayout.LabelField("New localized string");

                TextMeshProUGUI uiTextComponent = null;
                uiTextComponent = (TextMeshProUGUI)EditorGUILayout.ObjectField(uiTextComponent, typeof(TextMeshProUGUI), true);

                if (uiTextComponent != null)
                {
                    foreach (var gameObject in DragAndDrop.objectReferences.Select(o => (GameObject)o))
                    {
                        if (!gameObject.TryGetComponent<LocalizedString>(out _))
                        {
                            var localizedString = gameObject.AddComponent<LocalizedString>();
                            var key = localizedString.GetComponent<TextMeshProUGUI>().text.ToUpper().Replace(" ", "_");

                            if (!_localizedStrings.ContainsKey(key)) _localizedStrings.Add(key, new HashSet<LocalizedString>());
                            _localizedStrings[key].Add(localizedString);

                            _scrollsLocalizedString.Add(Vector2.zero);

                            if (!FadeLocalizedStrings.ContainsKey(key)) FadeLocalizedStrings.Add(key, true);
                        }
                        else
                        {
                            Debug.LogWarning("The localized string is already in the list");
                        }
                    }
                }
            }
        }

        private void DrawCsv()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button(nameof(ExpandAll))) ExpandAll(FadeLocales);
                if (GUILayout.Button(nameof(CollapseAll))) CollapseAll(FadeLocales);
            }

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                _index = EditorGUILayout.Popup("Select language", _index, _languages.ToArray());
                _currentLanguage = _languages[_index];

                EditorGUILayout.Space(15);

                var newKeys = new Dictionary<string, string>();
                var texts = new Dictionary<string, string>();

                var keyToDelete = "";

                int i = 0;
                foreach (var locale in _locales[_currentLanguage])
                {
                    using (new EditorGUILayout.VerticalScope("box"))
                    {
                        if (FadeLocales[locale.Key])
                        {
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                FadeLocales[locale.Key] = EditorGUILayout.Foldout(FadeLocales[locale.Key], "");
                                EditorGUILayout.Space(-60);

                                using (new EditorGUILayout.VerticalScope())
                                {
                                    newKeys[locale.Key] = EditorGUILayout.TextField(locale.Key, GUILayout.MaxWidth(1000.0f));

                                    if (GUILayout.Button("Delete item"))
                                        keyToDelete = locale.Key;
                                }

                                using (var scrollView = new EditorGUILayout.ScrollViewScope(_scrollsCsv[i], GUILayout.Height(70)))
                                {
                                    _scrollsCsv[i] = scrollView.scrollPosition;

                                    var guiStyle = new GUIStyle(EditorStyles.textArea)
                                    {
                                        wordWrap = true
                                    };

                                    texts[locale.Key] = EditorGUILayout.TextArea(locale.Value, guiStyle, GUILayout.MaxWidth(1000.0f), GUILayout.ExpandHeight(true));

                                }
                            }

                            EditorGUILayout.Space();
                        }
                        else
                        {
                            FadeLocales[locale.Key] = EditorGUILayout.Foldout(FadeLocales[locale.Key], locale.Key, true);
                        }
                    }

                    i++;
                }

                foreach (var text in texts)
                    _locales[_currentLanguage][text.Key] = text.Value;

                foreach (var key in newKeys)
                {
                    foreach (var lang in _locales.Keys)
                    {
                        _locales[lang].ChangeKey(key.Key, key.Value);
                        FadeLocales.ChangeKey(key.Key, key.Value);
                    }

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

            _scrollsCsv.Add(Vector2.zero);
            FadeLocales.Add("New key", true);
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


        private static void CollapseAll(IDictionary<string, bool> fades) => SetExpand(fades, false);
        private static void ExpandAll(IDictionary<string, bool> fades) => SetExpand(fades, true);

        private static void SetExpand(IDictionary<string, bool> fades, bool expand)
        {
            var keys = fades.Keys.ToArray();
            foreach (var key in keys) fades[key] = expand;
        }
    }
}
