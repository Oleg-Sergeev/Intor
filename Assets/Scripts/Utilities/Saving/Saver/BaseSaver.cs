using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Utilities.Saving.Saver
{
    public abstract class BaseSaver : IAsyncSaver
    {
        protected string DirectoryPath { get; }
        protected string FilePath { get; set; }


        public BaseSaver()
        {
#if UNITY_EDITOR
            DirectoryPath = $"{Application.dataPath}/Editor/Saves";
#else
            _directoryPath = $"{Application.persistentDataPath}/Saves";
#endif
            Directory.CreateDirectory(DirectoryPath);
        }

        public abstract Task SaveAsync(GameData saveData);
        public abstract Task<GameData> LoadAsync();
    }
}
