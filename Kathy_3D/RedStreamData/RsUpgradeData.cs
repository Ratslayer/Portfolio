using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedStream
{
    public class RsUpgradeData : RsData
    {
        public string Name; // Name of this upgrade
        // These are all ratios (1.0x == no change)
        public float HpModifier;      // For planet and ship upgrades (and towers, if we implement destructible towers)
        public float ShieldModifier;  // For planet upgrades (and towers, if we implement destructible towers, and ships if they'll have shields too)
        public float DamageModifier;  // For weapon upgrades
        public float SpeedModifier;   // For weapon ugprades, improves Rate-of-Fire; for ships, increases their speed
        // Visual changes once the upgrade is applied
        //public string ModelDescription; // RsActor.Desc?
        // For Ships and Towers, upgrades are associated with the weapon they contain
        public string Weapon;
        // Cost of buying this upgrade
        public int Cost;
    }
}
