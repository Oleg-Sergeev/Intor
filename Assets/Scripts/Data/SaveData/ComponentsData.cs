using System;
using System.Collections.Generic;

namespace Assets.Scripts.Data.SaveData
{
    [Serializable]
    public class ComponentsData : PositionData
    {
        public List<Type> ComponentsToAdd { get; set; }
        public List<Type> ComponentsToRemove { get; set; }


        public ComponentsData(string id) : base(id)
        {

        }
    }
}
