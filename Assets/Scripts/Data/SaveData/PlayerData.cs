using System;
using System.Collections.Generic;

namespace Assets.Scripts.Data.SaveData
{
    [Serializable]
    public class PlayerData : PositionData
    {
        public Dictionary<string, int> Slots { get; set; }


        public PlayerData(string id) : base(id)
        {
        }
    }
}
