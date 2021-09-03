using System;

namespace Assets.Scripts.Data.SaveData
{
    [Serializable]
    public class CameraData : PositionData
    {
        public CameraData(string id) : base(id)
        {
        }
    }
}
