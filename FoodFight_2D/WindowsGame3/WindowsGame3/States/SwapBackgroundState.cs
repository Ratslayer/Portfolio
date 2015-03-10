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
    /// State that changes backgrounds. Used for seemless transitions between backgrounds.
    /// </summary>
    public class SwapBackgroundState : DeferredRenderState
    {
        public SwapBackgroundState(string bgname, GameState renderState)
            :base(renderState)
        {
            bgName = bgname;
        }
        private string bgName;
        public override void Update(float gameTime)
        {
            GameContent.LoadBackground(bgName);
            Program.game.PopState();
            base.Update(gameTime);
        }
    }
}
