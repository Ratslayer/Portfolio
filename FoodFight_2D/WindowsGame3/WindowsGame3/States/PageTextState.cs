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
    /// State that allows to display text in scrolling pages.
    /// </summary>
    public abstract class PageTextState : TextState
    {
        public PageTextState(GameState renderState)
            : base(renderState)
        {
        }
        /// <summary>
        /// Page id.
        /// </summary>
        protected int iPage = 0;
        /// <summary>
        /// Boolean that prevents multiple pages to be scrolled on 1 press.
        /// </summary>
        private bool bPressedContinue = true;
        public override void ProcessInput(GameTime gameTime)
        {
            KeyboardState keys = Keyboard.GetState();
            if (keys.IsKeyDown(Keys.Space) && !bPressedContinue)
            {
                iPage++;
                bPressedContinue = true;
                CreatePage();
            }
            else if (keys.IsKeyUp(Keys.Space))
                bPressedContinue = false;
            base.ProcessInput(gameTime);
        }
        protected abstract void CreatePage();
    }
}
