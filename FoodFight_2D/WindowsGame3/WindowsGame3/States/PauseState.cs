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
    /// State that is pushed when the game is paused.
    /// </summary>
    public class PauseState : TextState
    {
        public PauseState(GameState renderState)
            : base(renderState)
        {
            bPaused = true;
            lastPos = new Vector2(500, 300);
            Add("Press", new Vector4(1, 1, 1, 1), GameContent.BigFont);
            lastPos.Y += 65;
            lastPos.X -= 490;
            Add("SPAAAAAAAAAAAAAAAAAAACE", new Vector4(1, 1, 1, 1), GameContent.BigFont);
            lastPos.Y += 65;
            lastPos.X += 350;
            Add("to resume...", new Vector4(1, 1, 1, 1), GameContent.BigFont);
        }
        /// <summary>
        /// Is the game still paused?
        /// </summary>
        private bool bPaused;
        public override void ProcessInput(GameTime gameTime)
        {
            KeyboardState keys = Keyboard.GetState();
            if (keys.IsKeyDown(Keys.Space))
            {
                bPaused = false;
                Program.game.PushState(new TransitionState(1, 20, this.renderState));
            }
            base.ProcessInput(gameTime);
        }
        public override void Update(float gameTime)
        {
            if (!bPaused)
            {
                Program.game.bPaused = false;
                Program.game.PopState();
            }
            base.Update(gameTime);
        }
    }
}
