using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace RedStream
{
    public class RsWeaponData : RsActorData
    {
        public enum WeaponType { Projectile, Beam, Missile, CashGenerator, HpGenerator, ShieldGenerator }
        public string Name;
        public string Description;
        public string Sound; // SoundEffect Name
        public int DamageDealt; // Depends on parent RsUpgrade
        public float RateOfFire; // Depends on parent RsUpgrade
        public int BulletCount; // Use -1 for infinite
        public WeaponType Type; // Beam vs projectile, for example
        public int BulletSpeed;
        public float Range;
        public float FOV;
        public Vector4 BeamColor;
        public int BeamLifeTime;
    }
}
