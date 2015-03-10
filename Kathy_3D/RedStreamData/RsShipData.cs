using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedStream
{
    public class RsShipData : RsDamageableData
    {
        public enum Mode { Straight, Orbit, HitAndRun }
        
        public string InitialUpgrade; // Initial upgrade level of this ship (includes weapon)
        public RsShipData.Mode ShipType; // check modes
        //public int Shield; // IF we want ships to have shields...
        public int Speed; // Speed of this ship (can be further modified depending on CurrentUpgrade);
        public int Points; //The amount of points awarded for destroying this ship
      //  public bool Boss;
    }
}
