using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Assets.Scripts.Utilities.Saving.Saver.Surrogates;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.Scripts.Utilities.Saving.Saver
{
    public class JsonSaver : BaseSaver
    {
        public JsonSaver()
        {
            FilePath = $"{DirectoryPath}/Save.json";
        }


        public override async Task SaveAsync(GameData saveData)
        {
            var settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Converters = new List<JsonConverter>() { new Vector3Converter() }
            };

            var json = JsonConvert.SerializeObject(saveData, settings);

            using (var writer = new StreamWriter(FilePath))
            {
                await writer.WriteLineAsync(json);
            }
        }

        public override async Task<GameData> LoadAsync()
        {
            if (!File.Exists(FilePath)) return new GameData();

            var json = "";

            using (var reader = new StreamReader(FilePath))
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
                TypeNameHandling = TypeNameHandling.Auto,
                Converters = new List<JsonConverter>() { new Vector3Converter() }
            };

            var gameData = JsonConvert.DeserializeObject<GameData>(json, settings);

            return gameData;
        }
    }
}
