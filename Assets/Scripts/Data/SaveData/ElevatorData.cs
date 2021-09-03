using System;

namespace Assets.Scripts.Data.SaveData
{
    [Serializable]
    public class ElevatorData : PositionData
    {
        public bool IsWorking { get; set; }


        public ElevatorData(string id) : base(id)
        {
        }
    }
}
