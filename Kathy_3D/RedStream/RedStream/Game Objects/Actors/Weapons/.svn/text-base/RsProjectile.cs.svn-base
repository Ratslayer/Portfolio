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
    public partial class RsProjectile : RsActor
    {
        public enum Allegiance { Planet, Invaders }
        public Allegiance allegiance;
        float Speed;
        float Damage;
        Vector3 Origin;
        RsActor Shooter;
        RsActor Target; //for homing missiles
        public RsProjectile(RsActor.Desc desc, RsActor shooter, Vector3 dir, float speed, float damage, RsActor target)
            : base(desc)
        {
            Damage = damage;
            Shooter = shooter;
            Target = target;
            Pos = Shooter.Pos;
            Origin = Pos;
            Speed = speed;
            SetVelocity(dir);
            if (Shooter is RsTower)
            {
                allegiance = Allegiance.Planet;
            }
            else
            {
                allegiance = Allegiance.Invaders;
            }
        }
        public override void Update(GameTime gameTime)
        {
            if (Target != null) //for homing missile
            {
                
                SetVelocity(Target.Pos - this.Pos);
                if (Target.isDead) Die();
            }

            if ((Pos-Origin).Length() > 70)
            {
                Die();
            }
            
            base.Update(gameTime);
        }

        public override void ReactToCollision(RsGameObject obj)
        {
            if (obj is RsShield && allegiance == Allegiance.Invaders)
            {
                //decrease score and whatnot
                
                RsShield shield = (RsShield)obj;
                if (shield.bActive)
                {
                    shield.TakeDamage(Damage);
                    Die();
                }
            }

            if (obj is RsPlanet && allegiance == Allegiance.Invaders)
            {
                //decrease score and whatnot
                RsPlanet planet = (RsPlanet)obj;
                planet.TakeDamage(Damage);
                Die();
            }
            if (obj is RsEnemy && allegiance == Allegiance.Planet)
            {
                //increase score and whatnot
                RsEnemy ship = (RsEnemy)obj;
                ship.TakeDamage(Damage);
                Die();
            }



            base.ReactToCollision(obj);
        }

        public void SetVelocity(Vector3 dir)
        {
            Vector3 direction = dir;
            direction.Normalize();
            
            
            Velocity = direction* Speed;
        }

        public override void Die()
        {
            RsScene.CreateExplosion(3, Pos, 0.1f, new Vector3());
            base.Die();
        }
    }
}
