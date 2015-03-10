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
    class AmmoFactory
    {
        public static AmmoFactory UID
        {
            get
            {
                if (_uid == null)
                {
                    _uid = new AmmoFactory();
                }
                return _uid;
            }
            private set
            {
            }
        }
        private static AmmoFactory _uid = null;
        private AmmoFactory()
        {
        }
        public void CreateRandomAmmoPack(Vector2 Pos)
        {
            Projectile proj = ProjectileFactory.UID.GetPlayerProjectile();
            int nAmmo = Game1.random.Next(1, 5);
            for (int i = 0; i < nAmmo; i++)
            {
                Ammo ammo = new Ammo(proj);
                ammo.pos = Game1.GetRotation(i * 360 / nAmmo)*30+Pos;
                Program.game.Add(ammo);
            }
        }
    }
}
