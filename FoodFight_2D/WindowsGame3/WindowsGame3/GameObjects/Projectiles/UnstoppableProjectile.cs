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
    /// This projectile will not stop until it has hit a wall.
    /// Setting it up as a mine makes it effectively undestructable.
    /// </summary>
    public class UnstoppableProjectile : Projectile
    {
        public UnstoppableProjectile()
            : base(GameContent.Fireballs[0])
        {
            color = new Vector4(1, 0, 0, 0.99f);
            damage = 5;
            angularSpeed = 100;
            Type = Damageable.HitType.Enemy;
        }
        public override Projectile Copy()
        {
            return new UnstoppableProjectile();
        }
        protected override void Hit(Damageable dam)
        {
            //Overriding Hit(Damageable) removes the automatic destruction of the Projectile upon hitting an enemy.
            //super.Hit(dam);
        }
    }
}
