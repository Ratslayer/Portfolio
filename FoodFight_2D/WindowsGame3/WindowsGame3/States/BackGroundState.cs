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
    /// State that just draws the background. Can be used by a DeferredRenderState.
    /// </summary>
    public class BackGroundState : GameState
    {
        public override void Draw()
        {
            Program.game.rMan.Draw(false);
            base.Draw();
        }
    }
}
