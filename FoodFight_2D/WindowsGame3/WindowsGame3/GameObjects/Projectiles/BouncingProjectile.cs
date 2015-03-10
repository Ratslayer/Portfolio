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
    /// This Projectile bounces off the walls and enemy nMaxBumps times and then dies.
    /// </summary>
    public class BouncingProjectile : Projectile
    {
        /// <summary>
        /// Maximum amount of bounces this Projectile can execute.
        /// </summary>
        private const int nMaxBumps = 10;
        /// <summary>
        /// Amount of bounces left to execute.
        /// </summary>
        private int nBumpsLeft;
        public BouncingProjectile()
            : base(GameContent.Fireballs[1])
        {
            color = new Vector4(0, 1, 0, 0.99f);
            damage = 10;
            angularSpeed = 300;
            nBumpsLeft = nMaxBumps;
            Type = Damageable.HitType.Enemy;
        }
        public override Projectile Copy()
        {
            return new BouncingProjectile();
        }
        protected override void Hit(Damageable dam)
        {
            Bounce(HitNormal);
        }
        protected override void HitWall(Vector2 normal)
        {
            Bounce(normal);
        }
        /// <summary>
        /// Helper function which takes care of bouncing, as well as managing the nBumpsLeft counter.
        /// </summary>
        /// <param name="normal">The normal that is used to bounce off.</param>
        private void Bounce(Vector2 normal)
        {
            if (nBumpsLeft > 0)
            {
                if (vel.Length() > 0)
                    vel -= normal * Vector2.Dot(vel, normal) * 2;
                else vel = -normal * ProjectileSpeed;
                nBumpsLeft--;
            }
            else Die();
        }
    }
}
