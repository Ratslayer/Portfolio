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
    /// During this state, the level is being initialized.
    /// Used for smooth transitions, where the level has to be seemlessly loaded.
    /// </summary>
    public class LoadLevelState : DeferredRenderState
    {
        public LoadLevelState(GameState renderState)
            : base(renderState)
        {
        }
        public override void Update(float gameTime)
        {
            Program.game.level.InitLevel();
            Program.game.PopState();
            base.Update(gameTime);
        }
    }
}
