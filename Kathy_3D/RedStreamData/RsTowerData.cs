using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedStream
{
    public class RsTowerData : RsActorData         // IF we want towers to be destroyable, make this RsDamageableData
    {
        public enum RsTowerType { Offensive, Defensive }
        public string Name;
        public string InitialUpgrade; // Initial upgrade level of this tower (includes weapon)
        //public int Shield; // IF we want towers to have shields...
        public int Cost; // Cost of this tower
        //public string ProximityBonus
        public RsTowerType TowerType;
        public int TechLevel;
    }
}
