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
    /// The pit class which can spawn enemies from its position.
    /// Does not contain a texture since it's rendered through Bubble effects.
    /// </summary>
    public class Pit : Interactable
    {
        #region Attributes
        /// <summary>
        /// Desired radius of the pit. Used in the Bubbling state, when the pit is growing to its full size. 
        /// </summary>
        public float desiredRadius;
        /// <summary>
        /// Reference to the spawned enemy.
        /// </summary>
        private Enemy spawnedEnemy;
        /// <summary>
        /// Downtime between Enemy's death and its respawn event.
        /// </summary>
        private float timeToSpawn = 0;
        #endregion

        #region Functions
        public Pit()
            : base(null)
        {
        }
        public override void Enter()
        {
            base.Interact();
            Player.TakeDamage(50);
        }
        public override void Update(float time)
        {
            
            timeToSpawn -= time;
            if (spawnedEnemy == null || (spawnedEnemy.bDead && timeToSpawn <= 0.0f))
            {
                Spawn();
                timeToSpawn = Game1.GetRandom(2, 1);
            }
            base.Update(time);
        }
        /// <summary>
        /// Spawn the enemy from the center.
        /// </summary>
        private void Spawn()
        {
            spawnedEnemy = new Enemy();
            spawnedEnemy.pos = pos;
            spawnedEnemy.desiredBounds = new Vector2(100, 160);
            spawnedEnemy.Bounds = new Vector2();
            spawnedEnemy.movSpeed = 100;
            spawnedEnemy.color = color;
            Program.game.Add(spawnedEnemy);
        }
        #endregion

    }
}
