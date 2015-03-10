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

namespace RedStream
{
    public class RsPausedHud : RsIHud
    {
        private SpriteFont font, largeFont;

        private Texture2D mainMenuTexture;
        private Texture2D mainSelectionTexture;
        private Rectangle viewportRect;

        public RsPausedHud()
        {
            font = RedStream.Content.GetFont("Courier New");
            largeFont = RedStream.Content.GetFont("Courier New Large");
            

            //main menu background
            mainMenuTexture = RedStream.Content.GetTexture("gyroMenu");
            
            // obv
            mainSelectionTexture = RedStream.Content.GetTexture("MainSelection2");


            //set a viewport variable
            viewportRect = new Rectangle(0, 0,
                RedStream.Graphics.graphics.GraphicsDevice.Viewport.Width,
                RedStream.Graphics.graphics.GraphicsDevice.Viewport.Height);
        }

        public void Draw(SpriteBatch batch)
        {
            Vector2 curPos = new Vector2(388,47);
            batch.Begin();
            batch.Draw(mainMenuTexture, viewportRect, Color.White);
            batch.DrawString(font, "Paused!", curPos, Color.Red); curPos.Y += 55;
#if !XBOX
            batch.Draw(mainSelectionTexture, new Vector2(50, 300), Color.White);
#else
            curPos.X = 10;
            curPos.Y = RedStream.Graphics.graphics.GraphicsDevice.Viewport.Height / 2 + 100;

            RsSprites.SpriteScale = 1f;
            batch.DrawString(largeFont, "Press    to play!", curPos, Color.OrangeRed);
            RsSprites.drawButtonStart(new Vector2(curPos.X + 78, curPos.Y - 0), batch);
            curPos.Y += 45;
            batch.DrawString(largeFont, "Press    for instructions", curPos, Color.OrangeRed);
            RsSprites.drawButtonA(new Vector2(curPos.X + 88, curPos.Y - 5), batch);
            curPos.Y += 45;
            batch.DrawString(largeFont, "Press    to leave :(", curPos, Color.OrangeRed);
            RsSprites.drawButtonBack(new Vector2(curPos.X + 77, curPos.Y - 0), batch);

#endif
            batch.End();
        }
        public void ProcessInput()
        {
#if XBOX
            if (RsInput.Tapped(Buttons.Start) || RsInput.Tapped(Buttons.B))
                RsStateManager.Pop();
            if (RsInput.Tapped(Buttons.A))
                RsStateManager.Push(new RsInstructionsState());
            if (RsInput.Tapped(Buttons.Y))
            {
                RsStateManager.Pop();
                RsStateManager.Pop();
            }
            if (RsInput.Down(Buttons.Back))
                RedStream.Game.Exit();
#else
            if (RsInput.Tapped(Keys.Enter))
                RsStateManager.Pop();
            if (RsInput.Tapped(Keys.Space))
                RsStateManager.Push(new RsInstructionsState());
            if (RsInput.Tapped(Keys.M))
            {
                RsStateManager.Pop();
                RsStateManager.Pop();
            }
            if (RsInput.Down(RsInput.KeyBindings.ExitKey))
                RedStream.Game.Exit();
#endif
        }
    }
}
