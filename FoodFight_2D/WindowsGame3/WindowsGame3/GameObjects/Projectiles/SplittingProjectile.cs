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
    /// Projectile that splits into 2 more projectiles upon collision with an enemy.
    /// Dies upon collision with a wall.
    /// Setting the Projectile up as a mine will spawn 3 projectiles instead.
    /// </summary>
    public class SplittingProjectile : Projectile
    {
        public SplittingProjectile()
            : base(GameContent.Fireballs[2])
        {
            color = new Vector4(0, 0, 1, 0.99f);
            damage = 7;
            angularSpeed = 200;
            Type = Damageable.HitType.Enemy;
        }
        public override Projectile Copy()
        {
            return new SplittingProjectile();
        }
        protected override void Hit(Damageable dam)
        {
            base.Hit(dam);
            
            if (vel.Length() > 0)
            {
                //Split into 2 projectiles, at 45 degrees off velocity each.
                Vector2 dir = vel;
                dir.Normalize();

                Vector2 normal = new Vector2(dir.Y, -dir.X);
                Projectile proj = this.Copy();
                proj.pos = pos;
                proj.vel = (dir + normal) * ProjectileSpeed;
                Program.game.Add(proj);

                proj = this.Copy();
                proj.pos = pos;
                proj.vel = (dir - normal) * ProjectileSpeed;
                Program.game.Add(proj);
            }
            else
            {
                //Split into 3 projectiles, at 120 degrees each.
                for (int i = 0; i < 3; i++)
                {
                    Projectile proj = Copy();
                    proj.pos = pos;
                    proj.vel = Game1.GetRotation(90 + 120 * i) * ProjectileSpeed;
                    Program.game.Add(proj);
                }
            }
        }
    }
}
