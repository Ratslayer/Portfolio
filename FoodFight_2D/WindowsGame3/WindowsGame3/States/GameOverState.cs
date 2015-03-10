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
    /// Game over state that is pushed if the player loses.
    /// </summary>
    public class GameOverState : TextState
    {
        public GameOverState(GameState renderState)
            : base(renderState)
        {
            lastPos = new Vector2(500, 200);
            Add("GAME OVER", new Vector4(.6f, 0, 0, 1), GameContent.BigFont);
            lastPos += new Vector2(200, 100);
            Add("You pedophile...", new Vector4(.6f,0,0,1));
            lastPos += new Vector2(-250, 100);
            Add("Press SPAAAAAAAACE to retry...", new Vector4(.7f, .7f, .7f, 1), GameContent.MediumFont);
            lastPos += new Vector2(-50, 100);
            Add("Or press Escape to give up... Like a sissy!", new Vector4(.7f, .7f, .7f, 1), GameContent.MediumFont);
        }
        public override void ProcessInput(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                Program.game.bGameOver = false;
                Program.game.PopState();
            }
            base.ProcessInput(gameTime);
        }
        
    }
}
