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
    public class RsWeapon : RsActor
    {
        public RsWeaponData Attributes;
        public bool Active = true;
        TimeSpan Timer = TimeSpan.Zero;
        RsActor Owner;
        RsActor.Desc WeaponDesc;
        RsPhysics.Cone ConeOfVision;
        
        
        public RsWeapon(RsActor.Desc desc, RsWeaponData data, RsActor owner) // Weapons are owned by upgrades
            : base(desc)
        {
            WeaponDesc = desc;
            Visible = false;
            Attributes = data;
            Owner = owner;
            Pos = Owner.Pos;
            Radius = Attributes.Range;
            ConeOfVision.Pos = Pos;
            ConeOfVision.Axis = this.Up;
            ConeOfVision.Angle = Attributes.FOV;
        }

        public override void Update(GameTime gameTime)
        {
            Pos = Owner.Pos;
            ConeOfVision.Pos = Pos;
            ConeOfVision.Axis = Owner.Forward;
            ConeOfVision.Angle = Attributes.FOV;

            Timer -= gameTime.ElapsedGameTime;
            if (Timer < TimeSpan.Zero)
            {
                Timer = TimeSpan.Zero;
            }


            base.Update(gameTime);
        }

        public void Shoot(RsActor target)
        {
            if (Timer > TimeSpan.Zero || !Active) return;


            switch (Attributes.Type)
            {
                case RsWeaponData.WeaponType.Projectile:
                    {
                        Vector3 direction = target.Pos - Pos;

                        RsProjectile.Desc pDesc = new RsProjectile.Desc();
                        pDesc.ModelName = WeaponDesc.ModelName;

                        pDesc.Scale = new Vector3(.5f, .5f, .5f);
                        pDesc.MaterialDesc = WeaponDesc.MaterialDesc;
                        RsProjectile bullet = new RsProjectile(pDesc, Owner, direction, Attributes.BulletSpeed, Attributes.DamageDealt, null);

                        if (Attributes.Name == "Railgun")
                            Beam(Owner, bullet);
                        else if(Attributes.Name == "Shrapnel Launcher")
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                
                                RsProjectile bullet2 = new RsProjectile(pDesc, Owner, direction + RsUtil.GetRandomVector(0.0f, 3f), Attributes.BulletSpeed + (RsUtil.GetRandomFloat(-2.5f,2.5f)), Attributes.DamageDealt, null);
                            }
                        }
                    }

                    break;


                case RsWeaponData.WeaponType.Beam:
                    {
                        // Ray ray = new Ray(Owner.Pos, direction);

                        Beam(Owner, target);
                        if(Attributes.Name == "Chain Lightning")
                            new RsWeaponNode(WeaponDesc, (RsActor)target, this, 3);

                        if (Attributes.Name == "Frost Beam")
                        {
                            RsEnemy nonBoss = (RsEnemy)target;
                            nonBoss.Frost();
                        }
                        break;
                    }

                case RsWeaponData.WeaponType.Missile:
                        {
                            Vector3 direction = target.Pos - Pos;

                            RsProjectile.Desc pDesc = new RsProjectile.Desc();
                            pDesc.ModelName = WeaponDesc.ModelName;

                            pDesc.Scale = new Vector3(.5f, .5f, .5f);
                            pDesc.MaterialDesc = WeaponDesc.MaterialDesc;
                            RsProjectile bullet = new RsProjectile(pDesc, Owner, direction, Attributes.BulletSpeed, Attributes.DamageDealt, target);
                        }
                    
                        break;

                //case RsWeaponData.WeaponType.CashGenerator:
                //        {
                //            // Generators don't have any projectiles, they just change game state
                //            RsGameInfo.Money += Attributes.DamageDealt;
                //        }

                //        break;
            }
            try
            {
                if (!Attributes.Sound.Equals("") && !Attributes.Sound.Equals("none"))
                {
                    RsContent.PlayCue(Attributes.Sound);
                }
            }
            catch (InvalidOperationException)
            {
                //No cue name
            }
            Timer = TimeSpan.FromSeconds(Attributes.RateOfFire);
        }

        public override void ReactToCollision(RsGameObject obj)
        {
            if (RsPhysics.Collides(obj.BoundingSphereOnlyForAsenicsUse, ConeOfVision))
            {
                if (Owner is RsTower && obj is RsPlanet)
                {
                    //Shoot((RsActor)obj);
                }

                if (Owner is RsEnemy && obj is RsPlanet)
                {
                    Shoot((RsActor)obj);
                }

                if (Owner is RsTower && Attributes.Name != "Point Defense Tower" && obj is RsEnemy) // FIXME: Remove this PDT hack
                {

                    Shoot((RsActor)obj);
                }

                if (Owner is RsTower && obj is RsProjectile && Attributes.Name == "Point Defense Tower") // FIXME: Remove this PDF hack
                {
                    RsProjectile projectile = (RsProjectile)obj;
                    if (projectile.allegiance != RsProjectile.Allegiance.Planet)
                    {
                        Shoot((RsActor)obj);
                        obj.Die();
                    }
                }
            }

            base.ReactToCollision(obj);
        }
        

        public void Beam(RsActor owner, RsActor target)
        {
        //make a laser beam (pew pew!)

            RsLaser.Desc lDesc = new RsLaser.Desc();
            lDesc.ModelName = WeaponDesc.ModelName;
            lDesc.Scale = Vector3.One;
            lDesc.MaterialDesc = WeaponDesc.MaterialDesc;
            lDesc.MaterialDesc.Color = Attributes.BeamColor;
            new RsLaser(lDesc, owner, target, Attributes.BeamLifeTime);

            if (target is RsDamageableActor)
            {
                RsDamageableActor dTarget = (RsDamageableActor)target;
                dTarget.TakeDamage(Attributes.DamageDealt);
            }
            
        }
        
    }
}
