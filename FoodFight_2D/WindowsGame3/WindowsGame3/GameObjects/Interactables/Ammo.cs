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
    /// Interacting with an Ammo object will make the player Arm a projectile.
    /// </summary>
    public class Ammo : Interactable
    {
        /// <summary>
        /// Projectile archetype that will be copied to the player, once it is picked up.
        /// </summary>
        private Projectile Projectile;
        public Ammo(Projectile proj)
            : base(proj.texture)
        {
            Projectile = proj;
            color = proj.color;
            Bounds = new Vector2(32, 32);
        }
        public override void Interact()
        {
            base.Interact();
            if (!Player.HasAmmo())
            {
                Player.Arm(Projectile.Copy());
                Die();
            }
        }
    }
}
