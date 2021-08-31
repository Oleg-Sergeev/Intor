using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.Utilities.Saving.Saver
{
    public class JsonSaver : IAsyncSaver
    {
        private readonly string _directoryPath;
        private readonly string _filePath;


        public JsonSaver()
        {
#if UNITY_EDITOR
            _directoryPath = $"{Application.dataPath}/Editor/Saves";
#else
            _directoryPath = $"{Application.persistentDataPath}/Saves";
#endif
            _filePath = $"{_directoryPath}/Save.json";
            Directory.CreateDirectory(_directoryPath);
        }


        public async Task SaveAsync(GameData saveData)
        {
            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var json = JsonConvert.SerializeObject(saveData, settings);

            using (var writer = new StreamWriter(_filePath))
            {
                await writer.WriteLineAsync(json);
            }
        }

        public async Task<GameData> LoadAsync()
        {
            if (!File.Exists(_filePath)) return new GameData();

            var json = "";

            using (var reader = new StreamReader(_filePath))
            {
                json = await reader.ReadToEndAsync();
            }

            if (string.IsNullOrEmpty(json))
            {
                Debug.LogWarning("Empty file loaded. Default game data returned");

                return new GameData();
            }

            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            };

            var gameData = JsonConvert.DeserializeObject<GameData>(json, settings);

            return gameData;
        }
    }
}
