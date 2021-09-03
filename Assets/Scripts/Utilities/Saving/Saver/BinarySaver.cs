using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Assets.Scripts.Utilities.Saving.Saver.Surrogates;
using UnityEngine;

namespace Assets.Scripts.Utilities.Saving.Saver
{
    public class BinarySaver : BaseSaver
    {
        private readonly BinaryFormatter _formatter;


        public BinarySaver()
        {
            FilePath = $"{DirectoryPath}/Save.zg";


            var surrogateSelector = new SurrogateSelector();
            surrogateSelector.AddSurrogate(
                typeof(Vector3),
                new StreamingContext(StreamingContextStates.All),
                new Vector3Serializer()
                );

            _formatter = new BinaryFormatter()
            {
                SurrogateSelector = surrogateSelector
            };
        }

        public override async Task SaveAsync(GameData saveData)
        {
            using (var stream = File.Open(FilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
            {
                var bytes = ConvertGameDataToByteArray(saveData);

                await stream.WriteAsync(bytes, 0, bytes.Length);
            }
        }

        public override async Task<GameData> LoadAsync()
        {
            if (!File.Exists(FilePath)) return new GameData();

            using (var stream = File.Open(FilePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read))
            {
                var bytes = new byte[stream.Length];
                var count = (int)stream.Length;

                await stream.ReadAsync(bytes, 0, count);

                GameData gameData = ConvertByteArrayToGameData(bytes);

                return gameData;
            }
        }



        private byte[] ConvertGameDataToByteArray(GameData obj)
        {
            if (obj == null) return null;

            using (var ms = new MemoryStream())
            {
                _formatter.Serialize(ms, obj);

                return ms.ToArray();
            }
        }

        private GameData ConvertByteArrayToGameData(byte[] byteArray)
        {
            if (byteArray == null) return null;

            using (var ms = new MemoryStream(byteArray))
            {
                var gameData = (GameData)_formatter.Deserialize(ms);

                return gameData;
            }
        }
    }
}
