using System;
using System.Collections.Generic;

namespace Assets.Scripts.Utilities.Saving
{
    [Serializable]
    public class GameData
    {
        public List<ItemData> ItemDatas { get; }


        public GameData()
        {
            ItemDatas = new List<ItemData>();
        }
    }
}
