using System;
using System.Threading.Tasks;

namespace Assets.Scripts.Utilities.Saving.Saver
{
    public interface IAsyncSaver
    {
        Task SaveAsync(GameData saveData);

        Task<GameData> LoadAsync();
    }
}
