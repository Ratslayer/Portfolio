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
    public class RsBoss : RsEnemy
    {

        float StageChangeThreshold = .15f;
        float StageChangeCount = 0;
        public RsBoss(RsActor.Desc desc, RsShipData shipData) 
            : base(desc, shipData)
        {
            BoundingSphereOnlyForAsenicsUse.Radius = Radius * .25f;
            DamageOnCollision = 10000;
            DescDataPair<RsWeaponData> pair = new DescDataPair<RsWeaponData>("Weapons\\" + ActiveUpgrade.Weapon);
            if (Attributes.Health > 3000)
            {
                ActiveWeapon = new RsWeapon(pair.Desc, pair.Data, this);
                ActiveWeapon.BoundingSphereOnlyForAsenicsUse.Radius = Radius;
            }
           
        }
        public override void Update(GameTime gameTime)
        {
            if (StageChangeCount >= StageChangeThreshold * MaxHealth)
            {
                Cloak(gameTime);
                if (Material.DiffuseFactor <= 0)
                {

                     StageChangeCount = 0;
                    float minDistance = RsGameInfo.Gyroscope.Rings[RsGameInfo.Gyroscope.iLastRing - 1].Radius + (Radius*0.5f) + 5;
                    float maxDistance = minDistance + 10;
                    //the ships don't collide with the gyros on spawn
                    Pos = RsUtil.GetRandomVector(minDistance, maxDistance);
                    FacePoint(Vector3.Zero);

                    SafeStraight();
                }

            }
            else Uncloak(gameTime);

            base.Update(gameTime);
        }

        public void Cloak(GameTime gameTime)
        {
            Material.DiffuseFactor -= 0.001f * gameTime.ElapsedGameTime.Milliseconds;
            if (Material.DiffuseFactor < 0) Material.DiffuseFactor = 0;
        }

        public void Uncloak(GameTime gameTime)
        {
            Material.DiffuseFactor += 0.001f * gameTime.ElapsedGameTime.Milliseconds;
            if (Material.DiffuseFactor > .8f) Material.DiffuseFactor = .8f;
        }

        public override void TakeDamage(float damage)
        {
            StageChangeCount += damage;
            base.TakeDamage(damage);
        }

        public override void ReactToCollision(RsGameObject obj)
        {
            if (obj is RsPlanet || obj is RsShield)
                RsGameInfo.Planet.Health = 0;
            base.ReactToCollision(obj);
        }

        public override void Frost()
        {
            return; //boss cant be frozen LOL SUCKER
        }

        public override void Die()
        {
            RsScene.CreateExplosion(10, Pos, 0.4f, Velocity);
            RsShipFactory.BossDead = true;
            base.Die();
        }
    }
}
