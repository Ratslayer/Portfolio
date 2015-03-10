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
    /// This is the basic enemy class that is spawned by the pit.
    /// After spawning it will grow from 0 scale to DesiredBounds specified after its creation.
    /// Shoots projectiles at the player at random intervals.
    /// </summary>
    public class Enemy : Damageable
    {
        #region Attributes
        /// <summary>
        /// The constant that depicts Enemy's total Health.
        /// </summary>
        public const float MaxHealth = 25;
        
        /// <summary>
        /// The bounds the image should grow to, after having spawned.
        /// </summary>
        public Vector2 desiredBounds;
        /// <summary>
        /// Time elapsed since last shot.
        /// </summary>
        private float shootElapsedTime;
        /// <summary>
        /// Time after which the next shot will be fired.
        /// </summary>
        private float shootTime;
        #endregion
        #region Constructors
        public Enemy()
            : base(GameContent.enemy, MaxHealth, HitType.Enemy)
        {
            GetShootTime();
        }
        #endregion
        #region Functions
        #region Public
        public override void Update(float time)
        {
            Grow(time);
            UpdateShoot(time);
            UpdateMove(time);
            base.Update(time);
        }
        public override void OnCollision(GameObject obj)
        {
            if (obj is Player)
            {
                Player Player = (Player)obj;
                Player.TakeDamage(25);
                Die();
            }
            base.OnCollision(obj);
        }
        public override void TakeDamage(float damage)
        {
             if(!IsPushedBack())
                base.TakeDamage(damage);
        }
        public override void Die()
        {
            base.Die();
            Level.Player.Points += 25;
        }
        #endregion
        #region Private
        /// <summary>
        /// Function responsible for the growth of enemy sprite.
        /// </summary>
        /// <param name="time">Time elapsed since last frame.</param>
        private void Grow(float time)
        {
            if (desiredBounds != Bounds)
            {
                Vector2 dir = desiredBounds - Bounds;
                dir.Normalize();
                Bounds += dir * time * 300;
                if ((Bounds - desiredBounds).Length() < 3)
                    Bounds = desiredBounds;
            }
        }
        /// <summary>
        /// Function that updates the position and orientation of the enemy.
        /// </summary>
        /// <param name="time">Time elapsed since last frame.</param>
        private void UpdateMove(float time)
        {
            if (!IsPushedBack())
            {
                vel = Vector2.Normalize(Level.Player.pos - pos) * movSpeed;
                if (vel.X < 0)
                    spriteEffects = SpriteEffects.FlipHorizontally;
                else spriteEffects = SpriteEffects.None;
            }
        }
        /// <summary>
        /// Function responsible for shooting projectiles at the right moment.
        /// </summary>
        /// <param name="time">Time elapsed since last frame.</param>
        private void UpdateShoot(float time)
        {
            shootElapsedTime += time;
            if (shootElapsedTime >= .5f)
            {
                texture = GameContent.enemy;
            }
            if (shootElapsedTime >= shootTime)
            {
                Shoot();
                shootElapsedTime = 0.0f;
                GetShootTime();
            }
        }
        /// <summary>
        /// Create a projectile and shoot it in the direction of the Player.
        /// </summary>
        private void Shoot()
        {
            texture = GameContent.shoop;
            Projectile proj = ProjectileFactory.UID.GetEnemyProjectile();
            proj.vel = Vector2.Normalize(Level.Player.pos - pos)*500;
            proj.pos = pos;
            Program.game.Add(proj);
            Program.game.soundBank.PlayCue("projectile");
        }
        /// <summary>
        /// Get the downtime between this shot and the next.
        /// </summary>
        private void GetShootTime()
        {
            shootTime = (float)(1.0+Game1.random.NextDouble()*4.0);
        }
        #endregion
        #endregion
    }
}
