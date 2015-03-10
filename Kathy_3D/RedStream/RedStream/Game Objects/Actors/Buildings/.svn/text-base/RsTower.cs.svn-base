using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
namespace RedStream
{
    public class RsTower : RsActor
    {
        public RsTowerData Attributes;
        protected RsUpgradeData ActiveUpgrade;
        protected RsWeapon ActiveWeapon;

        public Boolean PlaceableOnGyro
        {
            get { return Attributes.TowerType == RsTowerData.RsTowerType.Offensive; }
        }

        public RsWeapon Weapon
        {
            get { return ActiveWeapon; }
        }

        public RsTower(RsActor.Desc desc, RsSocket owner, RsTowerData towerData)
            : base(desc)
        {
            Attributes = towerData;

            ActiveUpgrade = (RsUpgradeData)RedStream.Content.GetObjectAttributes("Upgrades\\" + Attributes.InitialUpgrade);

            DescDataPair<RsWeaponData> pair = new DescDataPair<RsWeaponData>("Weapons\\" + ActiveUpgrade.Weapon);
            ActiveWeapon = new RsWeapon(pair.Desc, pair.Data, this);
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Die()
        {
            RsScene.CreateExplosion(5, Pos, 0.4f, new Vector3());
            Weapon.Delete();
            base.Die();
        }
    }
}
