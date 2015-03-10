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
namespace WindowsGame3
{
    /// <summary>
    /// Base class that should be extended by any GameState class.
    /// </summary>
    public class GameState
    {
        /// <summary>
        /// Function called at the State's first Update.
        /// </summary>
        public virtual void EnterState()
        {
        }
        /// <summary>
        /// Function called at the State's last Update.
        /// </summary>
        public virtual void ExitState()
        {
        }
        /// <summary>
        /// Function that processes the input from the player.
        /// </summary>
        /// <param name="gameTime">Time elapsed since last frame.</param>
        public virtual void ProcessInput(GameTime gameTime)
        {
        }
        /// <summary>
        /// Function that is called within Game's Update call.
        /// </summary>
        /// <param name="gameTime">Time elapsed since last frame.</param>
        public virtual void Update(float gameTime)
        {
        }
        /// <summary>
        /// Funciton that is called within Game's Draw call.
        /// </summary>
        public virtual void Draw()
        {
        }
    }
}
