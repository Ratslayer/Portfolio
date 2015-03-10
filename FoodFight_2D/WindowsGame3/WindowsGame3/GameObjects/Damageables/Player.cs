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
    /// Player class which can pick up ammo and shoot it at enemies.
    /// </summary>
    public class Player : Damageable
    {
        /// <summary>
        /// The max Health of the player.
        /// </summary>
        public const float MaxHealth = 200f;
        
        public float Points;
        private Projectile ammo;
        public bool bShooting;
        public Player()
            : base(GameContent.Player, MaxHealth, HitType.Player)
        {
            bShooting = false;
        }
        public void Shoot(Vector2 Dir)
        {
            if (HasAmmo() && !bShooting)
            {
                bShooting = true;
                /*Projectile proj = ProjectileFactory.UID.GetPlayerProjectile();
                ammoType = ProjectileFactory.ProjectileType.None;*/
                ammo.pos = pos;
                ammo.vel = Dir * 500;
                Program.game.Add(ammo);
                Program.game.soundBank.PlayCue("projectile");
                ammo = null;
            }
        }
        public void Arm(Projectile proj)
        {
            ammo = proj;
            if (HasAmmo())
                Heal(proj.damage);
        }
        public bool HasAmmo()
        {
            return ammo != null;
        }
        public override void Update(float time)
        {
            if (vel.X > 0)
                spriteEffects = SpriteEffects.FlipHorizontally;
            else spriteEffects = SpriteEffects.None;
            base.Update(time);
        }
        public override void Die()
        {
            base.Die();
            Program.game.bGameOver = true;
            Program.game.level.Reset();
        }
    }
}
