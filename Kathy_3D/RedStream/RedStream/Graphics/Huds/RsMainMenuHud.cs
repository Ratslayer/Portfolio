﻿using System;
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
    public class RsMainMenuHud : RsIHud
    {
        private SpriteFont font;

        private Texture2D mainMenuTexture;
        private Texture2D titleTexture;
        private Texture2D mainSelectionTexture;
        private Rectangle viewportRect;

        public RsMainMenuHud()
        {
            font = RedStream.Content.GetFont("Courier New");
            //main menu background
            mainMenuTexture = RedStream.Content.GetTexture("GyroMenu");
            //Title
            titleTexture = RedStream.Content.GetTexture("Title");
            // obv
            mainSelectionTexture = RedStream.Content.GetTexture("MainSelection");
            //set a viewport variable
            viewportRect = new Rectangle(0, 0,
                RedStream.Graphics.graphics.GraphicsDevice.Viewport.Width,
                RedStream.Graphics.graphics.GraphicsDevice.Viewport.Height);
        }

        public void Draw(SpriteBatch batch)
        {
            Vector2 curPos = new Vector2();
            batch.Begin();
            batch.DrawString(font, "FPS: " + RedStream.Game.frameRate, curPos, Color.Red); curPos.Y += 15;
            batch.Draw(mainMenuTexture, viewportRect, Color.White);
            batch.Draw(titleTexture, new Vector2(150, 10), Color.White);
            batch.Draw(mainSelectionTexture, new Vector2(15,300), Color.White);
            batch.End();
        }
        public void ProcessInput()
        {
#if XBOX
            if (RsInput.Tapped(Buttons.Start))
                RsStateManager.Push(new RsGameplayState());
            if (RsInput.Tapped(Buttons.A))
                RsStateManager.Push(new RsInstructionsState());
            if (RsInput.Down(Buttons.Back))
                RedStream.Game.Exit(); 
#else
            if (RsInput.Tapped(Keys.Enter))
                RsStateManager.Push(new RsGameplayState());
            if (RsInput.Tapped(Keys.Space))
                RsStateManager.Push(new RsInstructionsState());
            if (RsInput.Down(RsInput.KeyBindings.ExitKey))
                RedStream.Game.Exit();   
#endif
        }
    }
}
