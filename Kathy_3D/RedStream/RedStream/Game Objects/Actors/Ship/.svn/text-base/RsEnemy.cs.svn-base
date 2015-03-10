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
    public partial class RsEnemy : RsDamageableActor
    {
        protected RsShipData Attributes;
        protected RsUpgradeData ActiveUpgrade;
        protected RsWeapon ActiveWeapon;

        private float CurrentSpeed;
        protected float DamageOnCollision = 500;
        private Vector3 Direction;
        public Vector3 OrbitAxis;
        bool frosted = false;

        public RsWeapon Weapon
        {
            get { return ActiveWeapon; }
        }
        

        public RsEnemy(RsActor.Desc desc, RsShipData shipData)
            : base(desc, shipData)
        {
            /* Load the attributes */
            Attributes = shipData;
            /* Load the upgrade */
            ActiveUpgrade = (RsUpgradeData)RedStream.Content.GetObjectAttributes("Upgrades\\" + Attributes.InitialUpgrade);

            /* Load the weapon */
            DescDataPair<RsWeaponData> pair = new DescDataPair<RsWeaponData>("Weapons\\" + ActiveUpgrade.Weapon);
            if (Attributes.ShipType != RsShipData.Mode.Straight)
                ActiveWeapon = new RsWeapon(pair.Desc, pair.Data, this);
            SafeStraight();
            Move();
        }
        public void SafeStraight()
        {
            /* Check ship behavior */
            if (Attributes.ShipType == RsShipData.Mode.Straight ||
                Attributes.ShipType == RsShipData.Mode.HitAndRun)
            {
                /* Go straight */
                Straight();
            }
        }
        private Vector3 GetVelocity(Vector3 direction)
        {
            direction.Normalize();
            return direction * CurrentSpeed;
        }

        private void Stop()
        {
            CurrentSpeed = 0;
        }
        private void Move()
        {
            CurrentSpeed = Attributes.Speed * ActiveUpgrade.SpeedModifier;
        }

        private void Straight()
        {
            Direction = Vector3.Zero - Pos;
            FaceDirection(Direction);
        }


        private void Orbit()
        {
            FacePoint(Vector3.Zero);
            Direction = Vector3.Cross(Forward, OrbitAxis);
            
        }

        private void PassiveOrbit()
        {
            FacePoint(Vector3.Zero);
            Direction = Vector3.Cross(Forward, OrbitAxis);
            FaceDirection(Direction);
        }

        public virtual void Frost()
        {
            if (RsUtil.GetRandomFloat(0, 1) < 0.4f)
            {
                frosted = true;
                Material.Color = Color.Blue.ToVector4();
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Health <= 0)
            {
                Killed();
            }
            if (Attributes.ShipType == RsShipData.Mode.Orbit)
            {
                Orbit();
            }

            //if (Attributes.ShipType == RsShipData.Mode.HitAndRun)
            //{
            //    PassiveOrbit();
            //}

            if (Attributes.ShipType == RsShipData.Mode.HitAndRun && (Pos.Length() <= 20))
            {
                Direction = -Direction;
                FaceDirection(Direction);
            }


            if (Attributes.ShipType == RsShipData.Mode.HitAndRun && (Pos.Length() > 160))
            {
                //Orbit();
                Die();
            }

            //if ((RsShipData.Mode)Attributes.ShipType != RsShipData.Mode.Straight)
            //{

            //        ShootPlanet();            
            //}

            Velocity = GetVelocity(Direction);
            if (frosted) Velocity *= 0.25f;

            

            base.Update(gameTime);
        }

        //public void ShootPlanet()
        //{
        //    ActiveWeapon.Shoot(Vector3.Zero - Pos);
           
        //}

        public override void ReactToCollision(RsGameObject obj)
        {
            if (obj is RsPlanet || obj is RsShield)
            {
                Pos = RsPhysics.GetCollisionPoint(obj, this);
                ((RsDamageableActor)obj).TakeDamage(DamageOnCollision);
                Die();
            }
            if (obj is RsGyro)
            {
                RsGyro gyro = (RsGyro)obj;
                if (gyro.HitsGyro(ref BoundingSphereOnlyForAsenicsUse))
                {
                    gyro.Break();
                    Die();
                }
            }
            base.ReactToCollision(obj);
        }

        public void Killed()
        {
            RsGameInfo.Score += Attributes.Points;
            RsGameInfo.ShipsKilled++;
            Die();
        }

        public override void Die()
        {
            
               
            
            if(Weapon!=null)
                Weapon.Delete();
            base.Die();
        }
    }
}
