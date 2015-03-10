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
    /// This state takes advantage of other GameState's Draw code. 
    /// This is done in the event where multiple inheritance is needed.
    /// </summary>
    public class DeferredRenderState : GameState
    {
        public DeferredRenderState(GameState renderState)
        {
            this.renderState = renderState;
        }
        protected GameState renderState;
        public override void Draw()
        {
            renderState.Draw();
            base.Draw();
        }
    }
}
