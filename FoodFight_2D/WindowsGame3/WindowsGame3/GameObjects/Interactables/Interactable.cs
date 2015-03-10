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
    /// This class responds only to collision with the player.
    /// Every one of its subclasses must override the Interract function and/or the Enter function.
    /// </summary>
    public class Interactable : GameObject
    {
        /// <summary>
        /// Used to make sure that the player takes damage only upon entering the pit.
        /// </summary>
        private bool bHasPlayer = false, bHadPlayer = false;
        /// <summary>
        /// If set to true, this object won't call the Interact function and won't interact with the player.
        /// </summary>
        public bool bDisabled;
        /// <summary>
        /// Overrides bDisabled and allows the Enter function to be called.
        /// </summary>
        public bool bAllowEnter;
        /// <summary>
        /// Internal reference to the Player object. Updated on every Player-this collision.
        /// </summary>
        protected Player Player;

        public Interactable(Texture2D texture)
            : base(texture)
        {
        }
        /// <summary>
        /// Override this function if you want the player to interact with this object.
        /// </summary>
        public virtual void Interact()
        {
        }
        /// <summary>
        /// Override this function if you want an effect for when the player enters the object's area.
        /// </summary>
        public virtual void Enter()
        {
        }
        public override void OnCollision(GameObject obj)
        {
            base.OnCollision(obj);
            if (obj is Player)
            {
                Player = (Player)obj;
                if(!bDisabled)
                    Interact();
                if (!bHadPlayer && (!bDisabled || bAllowEnter))
                    Enter();
                bHasPlayer = true;
            }
        }
        public override void Update(float time)
        {
            base.Update(time);
            bHadPlayer = bHasPlayer;
            bHasPlayer = false;
        }
    }
}
