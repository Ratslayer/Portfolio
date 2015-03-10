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
    /// Projectile that is thrown by the enemy.
    /// </summary>
    public class EnemyProjectile : Projectile
    {
        public EnemyProjectile()
            : base(GameContent.Fireballs[3])
        {
            color = new Vector4(1, 1, 1, 0.99f);
            damage = 10;
            angularSpeed = 150;
            Type = Damageable.HitType.Player;
        }
        public override Projectile Copy()
        {
            return new EnemyProjectile();
        }
    }
}
