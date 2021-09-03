using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Assets.Scripts.Data.Items;
using UnityEngine;

namespace Assets.Scripts.Utilities.Localization
{
    public class LocalizationController : MonoBehaviour
    {
        private const char CommaSubstitute = '\uFE19';

        private static string LocalizationPath;

        private static LocalizationController Instance;

        private static HashSet<ILocalizableText> LocalizableTexts;

        private static Dictionary<string, Dictionary<string, string>> Locales;


        public static event Action LanguageChanged;


        private void Awake()
        {
            if (Instance == null) Instance = this;
            else
            {
                Debug.LogWarning($"Removed duplicate {nameof(LocalizationController)} ({name})");
                Destroy(gameObject);
                return;
            }

            OnValidate();
        }

        private void Start()
        {
            LocalizableTexts = new HashSet<ILocalizableText>();
            Locales = new Dictionary<string, Dictionary<string, string>>();

            var localizedTexts = UnityObjectHelper.FindObjectsOfType<ILocalizableText>();
            foreach (var text in localizedTexts)
                LocalizableTexts.Add(text);

            var items = Resources.LoadAll<Item>("Items");
            foreach (var item in items)
                LocalizableTexts.Add(item);


            var csv = LoadCsv();

            var langs = csv[0];

            for (int i = 0; i < langs.Length; i++)
            {
                var locales = csv.ToDictionary(lines => lines[0], lines => lines[i]);

                Locales.Add(langs[i].ToUpper(), locales);
            }
        }


        private void OnValidate()
        {
            LocalizationPath = "Localization/LocalizationData";

#if UNITY_EDITOR
            ResourcesPath = $"{Application.dataPath}/Resources/{LocalizationPath}.csv";
#endif
        }


        public static void ChangeLanguage(string lang)
        {
            lang = lang.ToUpper();
            foreach (var localizable in LocalizableTexts)
            {
                if (Locales[lang].TryGetValue(localizable.Key, out var locale)) localizable.Localize(locale);
                else Debug.LogError($"Locale not found. Key: {localizable.Key}");
            }

            LanguageChanged?.Invoke();
        }


        private static List<string[]> LoadCsv() => Resources.Load<TextAsset>(LocalizationPath).text
                .Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(HandleLine)
                .ToList();

        private static string[] HandleLine(string line)
        {
            char[] trimmers = { ' ', '\"' };

            var arr = line
                .Trim(trimmers)
                .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < arr.Length; i++)
                arr[i] = arr[i].Trim(trimmers).Replace(CommaSubstitute, ',');

            return arr;
        }



#if UNITY_EDITOR
        private static string ResourcesPath;

        public static char CommaSubstituteEditor => CommaSubstitute;

        public static List<string[]> LoadCsvEditor() => LocalizationPath != null ? LoadCsv() : null;
        public static void SaveCsvEditor(IEnumerable<string> csv) => File.WriteAllLines(ResourcesPath, csv.Select(HandleLineEditor), Encoding.Unicode);


        private static string HandleLineEditor(string line)
        {
            char[] trimmers = { ' ', '\"' };

            var arr = line
                .Trim(trimmers)
                .Split(new char[] { CommaSubstitute }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < arr.Length; i++)
                arr[i] = arr[i].Trim(trimmers).Replace(',', CommaSubstitute);

            return string.Join(",", arr);
        }
#endif
    }
}
