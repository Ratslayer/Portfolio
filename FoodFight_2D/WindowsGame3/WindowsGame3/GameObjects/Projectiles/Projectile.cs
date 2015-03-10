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
using System.Collections;
namespace WindowsGame3
{
    /// <summary>
    /// Abstract Projectile class which can be extended to allow projectiles have special effects.
    /// </summary>
    public abstract class Projectile : GameObject
    {
        /// <summary>
        /// Damage dealt by the projectile.
        /// </summary>
        public float damage;
        /// <summary>
        /// Type of Damageables it can hit.
        /// </summary>
        public Damageable.HitType Type;
        /// <summary>
        /// Internal variable that holds the HitNormal of Object to Object collision
        /// </summary>
        protected Vector2 HitNormal
        {
            get;
            private set;
        }
        /// <summary>
        /// The speed at which the Damageable will be pushed back.
        /// </summary>
        private const float PushBackSpeed = 700f;
        /// <summary>
        /// The speed at which the Projectile will move
        /// </summary>
        protected const float ProjectileSpeed = 500f;
        protected Projectile(Texture2D texture)
            : base(texture)
        {
            Bounds = new Vector2(100, 100);
            Radius = 50;
        }
        protected Projectile(Projectile proj)
            : base(proj.texture)
        {
            color = proj.color;
            Type = proj.Type;
            damage = proj.damage;
            angularSpeed = proj.angularSpeed;
            Bounds = proj.Bounds;
            Radius = proj.Radius;
        }
        /// <summary>
        /// Abstract Copy method, which must return a deep copy of the Projectile.
        /// </summary>
        /// <returns>Deep copy of the Projectile.</returns>
        public abstract Projectile Copy();
        public override void OnCollision(GameObject obj)
        {
            if (obj is Damageable && !obj.bDead)
            {
                Damageable dam = (Damageable)obj;
                if (dam.Type == Type && !dam.IsPushedBack())
                {
                    Vector2 dir = GetCollisionPoint(dam) - pos;
                    dir.Normalize();
                    HitNormal = dir;
                    dam.TakeDamage(damage);
                    dam.PushBack(HitNormal * PushBackSpeed);
                    Hit(dam);
                }
            }
            base.OnCollision(obj);
        }
        public override void OnWallCollision(Vector2 normal)
        {
            base.OnWallCollision(normal);
            HitWall(normal);
        }
        public override void Die()
        {
            base.Die();
            ParticleEmitter emitter = new ParticleEmitter(GameContent.projectile);

            emitter.pos = pos;
            emitter.color = color;
            emitter.maxDistance = 30;
            emitter.minDistance = 1;
            emitter.maxScale = 1;
            emitter.minScale = .25f;
            emitter.nEmits = 100;
            emitter.emissionRate = 0;
            emitter.moveSpeed = -200;
            emitter.target = emitter;
            emitter.bOnTimer = true;
            emitter.lifeTime = 1.0f;
            emitter.bAccelerate = false;
            Program.game.Add(emitter);

            Program.game.soundBank.PlayCue("explosion");
        }
        /// <summary>
        /// Called upon collision with a Damageable that can be hit by this Projectile.
        /// Override for special effects only.
        /// </summary>
        /// <param name="dam">The damageable that the Projectile collided against.</param>
        protected virtual void Hit(Damageable dam)
        {
            Die();
        }
        /// <summary>
        /// Called upon collision with a wall.
        /// </summary>
        /// <param name="normal">The vector normal to wall's surface.</param>
        protected virtual void HitWall(Vector2 normal)
        {
            Die();
        }
    }
}
