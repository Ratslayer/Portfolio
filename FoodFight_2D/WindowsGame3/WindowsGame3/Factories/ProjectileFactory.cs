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
    public class ProjectileFactory
    {
        public static ProjectileFactory UID
        {
            get
            {
                if (_uid == null)
                {
                    _uid = new ProjectileFactory();
                }
                return _uid;
            }
            private set
            {
            }
        }
        private static ProjectileFactory _uid = null;
        private ProjectileFactory()
        {
            PlayerProjectiles = new List<Projectile>();
            PlayerProjectiles.Add(new UnstoppableProjectile());
            PlayerProjectiles.Add(new BouncingProjectile());
            PlayerProjectiles.Add(new SplittingProjectile());

            EnemyProjectile = new EnemyProjectile();
        }
        private List<Projectile> PlayerProjectiles;
        private Projectile EnemyProjectile;
        public Projectile GetPlayerProjectile()
        {
            int id = Game1.random.Next(PlayerProjectiles.Count);
            return PlayerProjectiles[id].Copy();
        }
        public Projectile GetEnemyProjectile()
        {
            return EnemyProjectile.Copy();
        }
    }
}
